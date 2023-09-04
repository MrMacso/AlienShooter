using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Pool;

public class Blaster : MonoBehaviour
{
    [SerializeField] BlasterShot _blasterShotPrefab;
    [SerializeField] Transform _firePoint;
    
    ObjectPool<BlasterShot> _pool;
    Player _player;
    PlayerInput _playerInput;


    void Awake()
    {
        _pool = new ObjectPool<BlasterShot>(AddNewBlasterShotToPool,
            t => t.gameObject.SetActive(true),
            t => t.gameObject.SetActive(false));

        _player = GetComponent<Player>();
        _playerInput = GetComponent<PlayerInput>();
        // _playerInput.actions["Fire"].performed += TryFire;
    }

    BlasterShot AddNewBlasterShotToPool()
    {
        var shot = Instantiate(_blasterShotPrefab);
        shot.SetPool(_pool);
        return shot;
    }

    void TryFire(InputAction.CallbackContext obj)
    {
        BlasterShot shot = _pool.Get();// Instantiate(_blasterShotPrefab, _firePoint.position, Quaternion.identity);
        shot.Launch(_player.Direction, _firePoint.position);
    }

    // Update is called once per frame
    void Update()
    {
        if (_playerInput.actions["Fire"].ReadValue<float>() > 0)
        {
            BlasterShot shot = _pool.Get(); // Instantiate(_blasterShotPrefab, _firePoint.position, Quaternion.identity);
            shot.Launch(_player.Direction, _firePoint.position);
        }
    }
}
