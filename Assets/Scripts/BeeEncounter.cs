using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeeEncounter : MonoBehaviour
{
    [SerializeField] List<Transform> _lightnings;
    [SerializeField] float _delayBeforeDamage = 1.5f;
    [SerializeField] float _lightningAnimationTime = 2f;
    [SerializeField] float _delayBetweenLightning = 1f;
    [SerializeField] float _lightningRadius = 1f;
    [SerializeField] LayerMask _playerLayer;

    Collider2D[] _playerHitResult = new Collider2D[10];
    void OnValidate()
    {
        if(_lightningAnimationTime <= _delayBeforeDamage)
            _delayBeforeDamage= _lightningAnimationTime;
    }
    void OnEnable()
    {
        StartCoroutine(StartEncounter());
    }

    IEnumerator StartEncounter()
    {
        while (true)
        {
            foreach (var lightning in _lightnings)
            {
                lightning.gameObject.SetActive(false);
            }
            yield return null;

            int index = UnityEngine.Random.Range(0, _lightnings.Count);
            _lightnings[index].gameObject.SetActive(true);
            yield return new WaitForSeconds(_delayBeforeDamage);
            DamagePlayersInrange(_lightnings[index]);
            yield return new WaitForSeconds(_lightningAnimationTime - _delayBeforeDamage);
            _lightnings[index].gameObject.SetActive(false);
            yield return new WaitForSeconds(_delayBetweenLightning);
        }
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
}
