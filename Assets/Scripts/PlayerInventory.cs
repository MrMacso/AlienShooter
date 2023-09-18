using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInventory : MonoBehaviour
{

    public Transform ItemPoint;
    PlayerInput _playerInput;
    IItem EquipedItem => _items.Count >= _currentItemIndex ? _items[_currentItemIndex] : null;
    List<IItem> _items = new List<IItem>();
    int _currentItemIndex;

    void Awake()
    {
        _playerInput= GetComponent<PlayerInput>();
        _playerInput.actions["Fire"].performed += UseEquipedItem;
        _playerInput.actions["EquipNext"].performed += EquipNext;

        foreach (var item in GetComponentsInChildren<IItem>())
            Pickup(item);
    }
    void OnDestroy()
    {
        _playerInput.actions["Fire"].performed -= UseEquipedItem;
        _playerInput.actions["EquipNext"].performed -= EquipNext;
    }
    void EquipNext(InputAction.CallbackContext obj)
    {
        _currentItemIndex++;
        if (_currentItemIndex >= _items.Count)
            _currentItemIndex = 0;

        ToggleEquippedItem();
    }

    private void ToggleEquippedItem()
    {
        for (int i = 0; i < _items.Count; i++)
        {
            _items[i].gameObject.SetActive(i == _currentItemIndex);
        }
    }

    void UseEquipedItem(InputAction.CallbackContext obj)
    {
        if(EquipedItem != null)
            EquipedItem.Use();
    }

    public void Pickup(IItem item)
    {
        item.transform.SetParent(ItemPoint);
        item.transform.localPosition = Vector3.zero;
        _items.Add(item);
        _currentItemIndex = _items.Count - 1;
        ToggleEquippedItem();

        var collider = item.gameObject.GetComponent<Collider2D>();
        if(collider != null)
            collider.enabled = false;
    }
}
