using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Frog : MonoBehaviour
{
    Rigidbody2D _rb;
    SpriteRenderer _spriteRenderer;
    Sprite _defaultSprite;

    [SerializeField] float _jumpDelay = 3;
    [SerializeField] Vector2 _jumpForce;
    [SerializeField] Sprite _jumpSprite;
    [SerializeField] int _jumps = 2;

    int _jumpRemaining;

    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _defaultSprite = GetComponent<SpriteRenderer>().sprite;
        InvokeRepeating("Jump", _jumpDelay, _jumpDelay);
        _jumpRemaining = _jumps;
    }

    void Jump()
    {
        if(_jumpRemaining == 0)
        {
            _jumpForce *= new Vector2(-1, 1);
            _jumpRemaining = _jumps;
        }
        _rb.AddForce(_jumpForce);
        _spriteRenderer.flipX = _jumpForce.x > 0;
        _spriteRenderer.sprite = _jumpSprite;
        _jumpRemaining--;
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        _spriteRenderer.sprite = _defaultSprite;
    }
}
