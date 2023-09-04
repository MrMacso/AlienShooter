using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class BlasterShot : MonoBehaviour
{
    [SerializeField] float _speed = 8.0f;
    [SerializeField] GameObject _impactExplosion;
    [SerializeField] float _maxLifetime = 4f;

    Rigidbody2D _rb;
    Vector2 _direction = Vector2.right;
    ObjectPool<BlasterShot> _pool;
    float _selfDestructTime;

    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    void SelfDestruct()
    {
        _pool.Release(this);
    }

    // Update is called once per frame
    void Update()
    {
        _rb.velocity = _direction * _speed;
        if(Time.time >= _selfDestructTime)
            SelfDestruct();
    }

    public void Launch(Vector2 direction, Vector2 position)
    {
        transform.position = position;
        _direction = direction;
        transform.rotation = _direction == Vector2.left ? Quaternion.Euler(0, 180, 0) : Quaternion.identity;
        _selfDestructTime = Time.time + _maxLifetime;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        var damageable = collision.gameObject.GetComponent<ITakeDamage>();
        if (damageable != null)
            damageable.TakeDamage();

        var explosion = Instantiate(_impactExplosion, collision.contacts[0].point, Quaternion.identity);
        Destroy(explosion.gameObject, 0.9f);

        SelfDestruct();
    }

    public void SetPool(ObjectPool<BlasterShot> pool)
    {
        _pool = pool;
    }
}
