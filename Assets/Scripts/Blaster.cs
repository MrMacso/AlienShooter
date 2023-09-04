using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Pool;

public class Blaster : MonoBehaviour
{
    [SerializeField] Transform _firePoint;

    Player _player;
    PlayerInput _playerInput;

    void Awake()
    {
        _player = GetComponent<Player>();
        _playerInput = GetComponent<PlayerInput>();
        // _playerInput.actions["Fire"].performed += TryFire;
    }
    void TryFire(InputAction.CallbackContext obj)
    {
        BlasterShot shot = PoolManager.Instance.GetBlasterShot();
        shot.Launch(_player.Direction, _firePoint.position);
    }

    // Update is called once per frame
    void Update()
    {
        if (_playerInput.actions["Fire"].ReadValue<float>() > 0)
        {
            BlasterShot shot = PoolManager.Instance.GetBlasterShot(); 
            shot.Launch(_player.Direction, _firePoint.position);
        }
    }
}
