using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dog : MonoBehaviour, ITakeDamage
{
    private void Start()
    {
        GetComponentInChildren<ShootAnimationWrapper>().OnShoot += Shoot;
    }
    void Shoot()
    {
        Debug.Log("Shooting");
    }

    public void TakeDamage()
    {
        gameObject.SetActive(false);
    }
}
