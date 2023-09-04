using Cinemachine;
using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    [SerializeField] float _maxHorizonalSpeed = 5;
    [SerializeField] float _jumpVelocity = 5;
    [SerializeField] float _jumpDuration = 0.5f;
    [SerializeField] Sprite _jumpSprite;
    [SerializeField] LayerMask _layerMask;
    [SerializeField] float _footOffset = 0.35f;
    [SerializeField] float _groundAcceleration = 25;
    [SerializeField] float _snowAcceleration = 1;
    [SerializeField] AudioClip _coinSfx;
    [SerializeField] AudioClip _hurtSfx;
    [SerializeField] float _knockbackVelocity = 300;
    [SerializeField] Collider2D _duckCollider;
    [SerializeField] Collider2D _standingCollider;

    public bool IsGrounded;
    public bool IsOnSnow;

    Rigidbody2D _rb;
    SpriteRenderer _spriteRenderer;
    AudioSource _audioSource;
    PlayerInput _playerInput;
    Animator _animator;

    float _horizontal;
    int _jumpRemaining;
    float _jumpEndTime;

    PlayerData _playerData = new PlayerData();

    public event Action CoinsChanged;
    public event Action HealthChanged;

    public int Coins { get => _playerData.Coins; private set => _playerData.Coins = value; }
    public int Health => _playerData.Health;

    public Vector2 Direction { get; private set; } = Vector2.right;

    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponentInChildren<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _audioSource = GetComponent<AudioSource>();
        _playerInput = GetComponent<PlayerInput>();
        FindObjectOfType<PlayerCanvas>().Bind(this);

    }
    void OnEnable() => FindObjectOfType<CinemachineTargetGroup>()?.AddMember(transform, 1f, 1f);
    void OnDisable() => FindObjectOfType<CinemachineTargetGroup>()?.RemoveMember(transform);

    void OnDrawGizmos()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        Gizmos.color = Color.red;

        Vector2 origin = new Vector2(transform.position.x, transform.position.y - spriteRenderer.bounds.extents.y);
        Gizmos.DrawLine(origin, origin + Vector2.down * 0.1f);
        //draw left foot
        origin = new Vector2(transform.position.x - _footOffset, transform.position.y - spriteRenderer.bounds.extents.y);
        Gizmos.DrawLine(origin, origin + Vector2.down * 0.1f);
        //draw right foot
        origin = new Vector2(transform.position.x + _footOffset, transform.position.y - spriteRenderer.bounds.extents.y);
        Gizmos.DrawLine(origin, origin + Vector2.down * 0.1f);
    }

    // Update is called once per frame
    void Update()
    {
        UpdateGrounding();
        var input = _playerInput.actions["Move"].ReadValue<Vector2>();
        var horizontalInput = input.x;
        var verticalInput = input.y;

        var vertical = _rb.velocity.y;

        if (_playerInput.actions["Jump"].WasPerformedThisFrame() && _jumpRemaining > 0)
        {
            _jumpEndTime = Time.time + _jumpDuration;
            _jumpRemaining--;

            _audioSource.pitch = _jumpRemaining > 0 ? 1 : 1.2f;

            _audioSource.Play();
        }

        if (_playerInput.actions["Jump"].ReadValue<float>() > 0 && _jumpEndTime > Time.time)
            vertical = _jumpVelocity;

        var desiredHorizontal = horizontalInput * _maxHorizonalSpeed;
        var acceleration = IsOnSnow ? _snowAcceleration : _groundAcceleration;

        _animator.SetBool("Duck", verticalInput < 0);

        var isDucking = _animator.GetBool("IsDucking");
        if (isDucking)
            desiredHorizontal = 0;
        _duckCollider.enabled = isDucking;
        _standingCollider.enabled = !isDucking;

        if (desiredHorizontal > _horizontal)
        {
            _horizontal += acceleration * Time.deltaTime;
            if (_horizontal > desiredHorizontal)
                _horizontal = desiredHorizontal;
        }
        else if (desiredHorizontal < _horizontal)
        {
            _horizontal -= acceleration * Time.deltaTime;
            if (_horizontal < desiredHorizontal)
                _horizontal = desiredHorizontal;
        }
        _rb.velocity = new Vector2(_horizontal, vertical);

        UpdateAnimation();
        UpdateDirection();
    }

    void UpdateGrounding()
    {
        IsGrounded = false;
        IsOnSnow = false;

        //check center
        Vector2 origin = new Vector2(transform.position.x, transform.position.y - _spriteRenderer.bounds.extents.y);
        RaycastHit2D hit = Physics2D.Raycast(origin, Vector2.down, 0.1f, _layerMask);
        if (hit.collider)
        {
            IsGrounded = true;
            IsOnSnow = hit.collider.CompareTag("Snow");
        }

        //check left
        origin = new Vector2(transform.position.x - _footOffset, transform.position.y - _spriteRenderer.bounds.extents.y);
        hit = Physics2D.Raycast(origin, Vector2.down, 0.1f, _layerMask);
        if (hit.collider)
        {
            IsGrounded = true;
            IsOnSnow = hit.collider.CompareTag("Snow");
        }

        //check right
        origin = new Vector2(transform.position.x + _footOffset, transform.position.y - _spriteRenderer.bounds.extents.y);
        hit = Physics2D.Raycast(origin, Vector2.down, 0.1f, _layerMask);
        if (hit.collider)
        {
            IsGrounded = true;
            IsOnSnow = hit.collider.CompareTag("Snow");
        }

        if (IsGrounded && GetComponent< Rigidbody2D>().velocity.y <= 0)
            _jumpRemaining = 2;

    }
    void UpdateAnimation()
    {
        _animator.SetBool("Jump", !IsGrounded);
        _animator.SetBool("Move", _horizontal != 0f);
    }
    private void UpdateDirection()
    {
        if (_horizontal > 0)
        {
            _animator.transform.rotation = Quaternion.identity;
            Direction = Vector2.right;
        }
        else if (_horizontal < 0)
        {
            _animator.transform.rotation = Quaternion.Euler(0, 180, 0);
            Direction = Vector2.left;
        }
    }
    public void AddPoint()
    {
        Coins++;
        _audioSource.PlayOneShot(_coinSfx);
        CoinsChanged?.Invoke();
    }
    public void Bind(PlayerData playerData)
    {
        _playerData= playerData;
    }
    public void TakeDamage(Vector2 hitNormal)
    {
        _playerData.Health--;
        if (_playerData.Health <= 0)
        { 
            SceneManager.LoadScene(0);
            return;
        }
        _rb.AddForce(-hitNormal * _knockbackVelocity);
        _audioSource.PlayOneShot(_hurtSfx);
        HealthChanged?.Invoke();
    }
    public void StopJump()
    {
        _jumpEndTime = Time.time;
    }

    public void Bounce(Vector2 normal, float bounciness)
    {
        _rb.AddForce(-normal * bounciness);
    }
}
