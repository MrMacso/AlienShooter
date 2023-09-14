using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BeeEncounter : MonoBehaviour, ITakeDamage
{
    [SerializeField] List<Transform> _lightnings;
    [SerializeField] float _delayBeforeDamage = 1.5f;
    [SerializeField] float _lightningAnimationTime = 2f;
    [SerializeField] float _delayBetweenLightning = 1f;
    [SerializeField] float _delayBetweenStrikes = 0.25f;
    [SerializeField] float _lightningRadius = 1f;
    [SerializeField] LayerMask _playerLayer;
    [SerializeField] int _numberOfLightnings = 1;
    [SerializeField] GameObject _bee;
    [SerializeField] GameObject _beeLaser;
    [SerializeField] Animator _beeAnimator;
    [SerializeField] Rigidbody2D _beeRigidbody;
    [SerializeField] Transform[] _beeDestinations;
    [SerializeField] float _maxIdleTime = 1f;
    [SerializeField] float _minIdleTime = 2f;
    [SerializeField] int _maxHealth = 50;
    [SerializeField] Water _water;
    [SerializeField] Collider2D _floodGroundCollider;

    Collider2D[] _playerHitResult = new Collider2D[10];
    List<Transform> _activeLightning;
    int _currentHealth;
    bool _shotStarted;
    bool _shotFinished;

    void OnValidate()
    {
        if (_lightningAnimationTime <= _delayBeforeDamage)
            _delayBeforeDamage = _lightningAnimationTime;
    }
    void OnEnable()
    {
        _currentHealth = _maxHealth;
        StartCoroutine(StartLightning());
        StartCoroutine(StartMovement());
        var wrapper = GetComponentInChildren<ShootAnimationWrapper>();
        wrapper.OnShoot += () => _shotStarted = true;
        wrapper.OnReload += () => _shotFinished = true;
    }

    IEnumerator StartMovement()
    {
        _beeLaser.SetActive(false);
        GrabBag<Transform> grabBag = new GrabBag<Transform>(_beeDestinations);
        while (true)
        {
            var destination = grabBag.Grab();
            if (destination == null)
            {
                Debug.LogError("Unable to choose a random destination for the Bee. Stopping Movement");
                yield break;
            }

            _beeAnimator.SetBool("Move", true);
            while (Vector2.Distance(_bee.transform.position, destination.position) > 0.1f)
            {
                _bee.transform.position = Vector2.MoveTowards(_bee.transform.position,
                                            destination.position, Time.deltaTime);
                yield return null;
            }
            _beeAnimator.SetBool("Move", false);
            yield return new WaitForSeconds(UnityEngine.Random.Range(_minIdleTime, _maxIdleTime));
            _beeAnimator.SetTrigger("Fire");

            yield return new WaitUntil(() => _shotStarted);
            _shotStarted = false;
            _beeLaser.SetActive(true);

            yield return new WaitUntil(() => _shotFinished);
            _shotFinished = false;
            _beeLaser.SetActive(false);
        }
    }

    IEnumerator StartLightning()
    {
        foreach (var lightning in _lightnings)
        {
            lightning.gameObject.SetActive(false);
        }

        _activeLightning = new List<Transform>();

        while (true)
        {
            for (int i = 0; i < _numberOfLightnings; i++)
            {
                yield return SpawnNewLightning();
            }

            yield return new WaitUntil(() => _activeLightning.All(t => !t.gameObject.activeSelf));
            _activeLightning.Clear();
        }
    }

    IEnumerator SpawnNewLightning()
    {
        if (_activeLightning.Count >= _lightnings.Count)
        {
            Debug.LogError("The number of requested lightnings exceeds the total available lightnings");
            yield break;
        }

        int index = UnityEngine.Random.Range(0, _lightnings.Count);
        var lightning = _lightnings[index];

        while (_activeLightning.Contains(lightning))
        {
            index = UnityEngine.Random.Range(0, _lightnings.Count);
            lightning = _lightnings[index];
        }

        StartCoroutine(ShowLightning(lightning));
        _activeLightning.Add(lightning);

        yield return new WaitForSeconds(_delayBetweenStrikes);
    }

    IEnumerator ShowLightning(Transform ligntning)
    {
        ligntning.gameObject.SetActive(true);
        yield return new WaitForSeconds(_delayBeforeDamage);
        DamagePlayersInrange(ligntning);
        yield return new WaitForSeconds(_lightningAnimationTime - _delayBeforeDamage);
        ligntning.gameObject.SetActive(false);
        yield return new WaitForSeconds(_delayBetweenLightning);
    }

    void DamagePlayersInrange(Transform lightning)
    {
        int hits = Physics2D.OverlapCircleNonAlloc(
            lightning.position,
            _lightningRadius,
            _playerHitResult,
            _playerLayer);
        for (int i = 0; i < hits; i++)
        {
            _playerHitResult[i].GetComponent<Player>().TakeDamage(Vector3.zero);
        }
    }

    public void TakeDamage()
    {
        _currentHealth--;
        if (_currentHealth == _maxHealth / 2)
        {
            StartCoroutine(ToggleFlood(true));
        }
        if (_currentHealth <= 0)
        {
            StopAllCoroutines();
            StartCoroutine(ToggleFlood(false));
            _beeAnimator.SetBool("Dead", true);
            _beeRigidbody.bodyType = RigidbodyType2D.Dynamic;
            foreach (var collider in _bee.GetComponentsInChildren<Collider2D>())
            {
                collider.gameObject.layer = LayerMask.NameToLayer("Dead");
            }
        }
        else
            _beeAnimator.SetTrigger("Hit");
    }

    IEnumerator ToggleFlood(bool enableFlood)
    {
        float initialWaterY = _water.transform.position.y;
        var targetWaterY = enableFlood ? initialWaterY + 1 : _water.transform.position.y - 1;
        float duration = 1f;
        float elapsedTime = 0;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float progress = elapsedTime / duration;
            float y = Mathf.Lerp(initialWaterY, targetWaterY, progress);
            var destination = new Vector3(_water.transform.position.x, y, _water.transform.position.z);
            _water.transform.position = destination;
            yield return null;
        }
        _floodGroundCollider.enabled = !enableFlood;
        _water.SetSpeed(enableFlood ? 5f : 0f);
    }


    [ContextMenu(nameof(HalfHealth))]
    void HalfHealth()
    {
        _currentHealth = _maxHealth / 2;
        _currentHealth++;
        TakeDamage();
    }

    [ContextMenu(nameof(Kill))]
    void Kill()
    {
        _currentHealth = 1;
        TakeDamage();
    }
    [ContextMenu(nameof(FullHealth))]
    void FullHealth()
    {
        _currentHealth = _maxHealth;
    }
}
