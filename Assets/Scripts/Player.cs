using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    float _jumpEndTime;
    [SerializeField] float _horizontalVelocity = 3;
    [SerializeField] float _jumpVelocity = 5;
    [SerializeField] float _jumpDuration = 0.5f;
    [SerializeField] Sprite _jumpSprite;
    [SerializeField] LayerMask _layerMask;
    [SerializeField] float _footOffset = 0.35f;
    public bool IsGrounded;
    Rigidbody2D _rb;
    SpriteRenderer _spriteRenderer;
    AudioSource _audioSource;
    float _horizontal;
    Animator _animator;
    int _jumpRemaining;

    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _audioSource = GetComponent<AudioSource>();
    }

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

        _horizontal = Input.GetAxis("Horizontal");
        Debug.Log(_horizontal);

        var vertical = _rb.velocity.y;

        if (Input.GetButtonDown("Fire1") && _jumpRemaining > 0)
        {
            _jumpEndTime = Time.time + _jumpDuration;
            _jumpRemaining--;

            _audioSource.pitch = _jumpRemaining > 0 ? 1 : 1.2f;

            _audioSource.Play();
        }

        if (Input.GetButton("Fire1") && _jumpEndTime > Time.time)
            vertical = _jumpVelocity;

        _horizontal *= _horizontalVelocity;
        _rb.velocity = new Vector2(_horizontal, vertical);

        UpdateSprite();
    }

    void UpdateGrounding()
    {
        IsGrounded = false;

        //check center
        Vector2 origin = new Vector2(transform.position.x, transform.position.y - _spriteRenderer.bounds.extents.y);
        RaycastHit2D hit = Physics2D.Raycast(origin, Vector2.down, 0.1f, _layerMask);
        if (hit.collider)
            IsGrounded = true;

        //check left
        origin = new Vector2(transform.position.x - _footOffset, transform.position.y - _spriteRenderer.bounds.extents.y);
        hit = Physics2D.Raycast(origin, Vector2.down, 0.1f, _layerMask);
        if (hit.collider)
            IsGrounded = true;

        //check right
        origin = new Vector2(transform.position.x + _footOffset, transform.position.y - _spriteRenderer.bounds.extents.y);
        hit = Physics2D.Raycast(origin, Vector2.down, 0.1f, _layerMask);
        if (hit.collider)
            IsGrounded = true;

        if (IsGrounded && GetComponent< Rigidbody2D>().velocity.y <= 0)
            _jumpRemaining = 2;

    }

    void UpdateSprite()
    {
        _animator.SetBool("IsGrounded", IsGrounded);
        _animator.SetFloat("HorizontalSpeed",Math.Abs( _horizontal));

        if(_horizontal > 0) 
            _spriteRenderer.flipX= false;
        else if (_horizontal < 0)
            _spriteRenderer.flipX = true;
    }
}
