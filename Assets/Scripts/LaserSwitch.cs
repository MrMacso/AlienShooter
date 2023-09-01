using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LaserSwitch : MonoBehaviour
{
    [SerializeField] Sprite _left;
    [SerializeField] Sprite _right;

    [SerializeField] UnityEvent _on;
    [SerializeField] UnityEvent _off;

    SpriteRenderer _spriteRenderer;
    private bool _isOn;
    void Awake()
    {
        _spriteRenderer= GetComponent<SpriteRenderer>();
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        var player = collision.GetComponent<Player>();

        if (player == null)
            return;

        var rigidbody = player.GetComponent<Rigidbody2D>();
        if (rigidbody.velocity.x > 0)
            TurnOn();
        else if (rigidbody.velocity.x < 0)
            TurnOff();
    }

    private void TurnOff()
    {
        if (_isOn)
        {
            _isOn = false;
            _off.Invoke();
            _spriteRenderer.sprite = _left;

            Debug.Log("OFF");
        }
    }

    private void TurnOn()
    {
        if (!_isOn)
        {
            _isOn = true;
            _spriteRenderer.sprite = _right;
            _on.Invoke();
            Debug.Log("ON");
        }
    }
}

