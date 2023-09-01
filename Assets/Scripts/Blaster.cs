using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Blaster : MonoBehaviour
{
    [SerializeField] BlasterShot _blasterShotPrefab;
    [SerializeField] Transform _firePoint;
    
    Player _player;
    PlayerInput _playerInput;

    void Awake()
    {
        _player = GetComponent<Player>();
        _playerInput = GetComponent<PlayerInput>();
        _playerInput.actions["Fire"].performed += TryFire;
    }

    void TryFire(InputAction.CallbackContext obj)
    {
        BlasterShot shot = Instantiate(_blasterShotPrefab, _firePoint.position, Quaternion.identity);
        shot.Launch(_player.Direction);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
