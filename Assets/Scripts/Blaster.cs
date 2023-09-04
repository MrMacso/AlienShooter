using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Pool;

public class Blaster : MonoBehaviour
{
    [SerializeField] Transform _firePoint;

    Player _player;
    PlayerInput _playerInput;
    Animator _animator;

    void Awake()
    {
        _animator = GetComponentInChildren<Animator>();
        _player = GetComponent<Player>();
        _playerInput = GetComponent<PlayerInput>();
        _playerInput.actions["Fire"].performed += TryFire;
    }
    void TryFire(InputAction.CallbackContext obj)
    {
        Fire();
    }

    private void Fire()
    {
        BlasterShot shot = PoolManager.Instance.GetBlasterShot();
        shot.Launch(_player.Direction, _firePoint.position);
        _animator.SetTrigger("Fire");
    }

    void Update()
    {
        //if (_playerInput.actions["Fire"].ReadValue<float>() > 0)
        //    Fire();
    }
}
