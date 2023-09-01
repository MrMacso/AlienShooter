using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlasterShot : MonoBehaviour
{
    [SerializeField] float _speed = 8.0f;
    [SerializeField] GameObject _impactExplosion;
    Rigidbody2D _rb;
    Vector2 _direction = Vector2.right;

    // Start is called before the first frame update
    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        _rb.velocity= _direction * _speed;
    }
    public void Launch(Vector2 direction)
    {
        _direction = direction;
        transform.rotation = _direction == Vector2.left ?  Quaternion.Euler(0f, 180f, 0f) : Quaternion.identity;
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        var damageable = collision.gameObject.GetComponent<ITakeDamage>();
        if (damageable != null)
            damageable.TakeDamage();

        var explosion = Instantiate(_impactExplosion, collision.contacts[0].point, Quaternion.identity);
        Destroy(explosion.gameObject, 0.9f);

        gameObject.SetActive(false);
    }
}
