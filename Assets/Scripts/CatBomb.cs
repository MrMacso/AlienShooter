using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatBomb : MonoBehaviour
{
    [SerializeField]float _forceAmount = 300f;

    Rigidbody2D _rb;
    Animator _animator;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _rb.simulated = false;
        _animator = GetComponentInChildren<Animator>();
        _animator.enabled= false;
    }
    public void Launch(Vector2 direcrtion)
    {
        transform.SetParent(null);
        _rb.simulated = true;
        _rb.AddForce(direcrtion * _forceAmount);
        _animator.enabled= true;
    }
}
