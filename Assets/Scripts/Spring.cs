using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spring : MonoBehaviour
{
    SpriteRenderer _spriteRenderer;
    AudioSource _audioSource;
    Sprite _defaultSprite;
    [SerializeField] Sprite _sprung;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _audioSource = GetComponent<AudioSource>();
        _defaultSprite = _spriteRenderer.sprite;
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            _spriteRenderer.sprite = _sprung;
             _audioSource.Play();
        }

    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
            _spriteRenderer.sprite = _defaultSprite;
    }
}
