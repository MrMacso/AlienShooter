using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour,IItem
{
    [SerializeField] float _useRange = 1f;

    public void Use()
    {
        var hits = Physics2D.OverlapCircleAll(transform.position, _useRange);
        foreach (var hit in hits)
        { 
            var toggleLock = hit.GetComponent<ToggleLock>();
            if (toggleLock)
            {
                toggleLock.Toggle();
                break;
            }
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        var playerInventory = collision.GetComponent<PlayerInventory>();
        if(playerInventory != null)
        {
            playerInventory.Pickup(this);
        }
    }
}
