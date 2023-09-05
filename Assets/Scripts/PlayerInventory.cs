using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInventory : MonoBehaviour
{

    public Transform ItemPoint;
    PlayerInput _playerInput;
    private Key _equipedKey;

    void Awake()
    {
        _playerInput= GetComponent<PlayerInput>();
        _playerInput.actions["Fire"].performed += UseEquipedItem;
    }

    void UseEquipedItem(InputAction.CallbackContext obj)
    {
        if(_equipedKey)
            _equipedKey.Use();
    }

    public void Pickup(Key key)
    {
        key.transform.SetParent(ItemPoint);
        key.transform.localPosition = Vector3.zero;
        _equipedKey = key;
    }
}
