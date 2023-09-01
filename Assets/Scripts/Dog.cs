using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dog : MonoBehaviour
{
    public void Shoot()
    {
        Debug.Log("Shooting");
    }

    internal void TakeDamage()
    {
        gameObject.SetActive(false);
    }
}
