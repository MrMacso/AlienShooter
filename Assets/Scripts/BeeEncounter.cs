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
    [SerializeField] Animator _beeAnimator;
    [SerializeField] Transform[] _beeDestinations;
    [SerializeField] float _maxIdleTime = 1f;
    [SerializeField] float _minIdleTime = 2f;

    Collider2D[] _playerHitResult = new Collider2D[10];
    List<Transform> _activeLightning;
    public int _health = 5;

    void OnValidate()
    {
        if(_lightningAnimationTime <= _delayBeforeDamage)
            _delayBeforeDamage= _lightningAnimationTime;
    }
    void OnEnable()
    {
        StartCoroutine(StartLightning());
        StartCoroutine(StartMovement());
    }

    IEnumerator StartMovement()
    {
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
        }
    }

    IEnumerator StartLightning()
    {
        foreach (var lightning in _lightnings)
        {
            lightning.gameObject.SetActive(false);
        }

        _activeLightning= new List<Transform>();

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
        if(_activeLightning.Count >= _lightnings.Count)
        {
            Debug.LogError("The number of requested lightnings exceeds the total available lightnings");
            yield break;
        }

        int index = UnityEngine.Random.Range(0, _lightnings.Count);
        var lightning = _lightnings[index];

        while(_activeLightning.Contains(lightning))
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
        _health--;
        if (_health <= 0)
            _bee.SetActive(false);
    }
}
