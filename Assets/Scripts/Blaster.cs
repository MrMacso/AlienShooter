using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Pool;

public class Blaster : MonoBehaviour, IItem
{
    [SerializeField] Transform _firePoint;

    Player _player;

    void Awake()
    {
        _player = GetComponentInParent<Player>();
    }
    void Fire()
    {
        BlasterShot shot = PoolManager.Instance.GetBlasterShot();
        shot.Launch(_player.Direction, _firePoint.position);
    }

    public void Use()
    {
        if(GameManager.CinematicPlaying == false)
        Fire();
    }
}
