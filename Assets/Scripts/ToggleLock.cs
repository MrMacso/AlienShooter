using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleLock : MonoBehaviour
{
    bool _unlocked;
    SpriteRenderer _spriteRenderer;

    void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _unlocked = false;
        _spriteRenderer.color = Color.grey;
    }

    [ContextMenu(nameof(Toggle))]
    public void Toggle() 
    {
        _unlocked = !_unlocked;
        _spriteRenderer.color = _unlocked ? Color.white : Color.grey;
    }
}
