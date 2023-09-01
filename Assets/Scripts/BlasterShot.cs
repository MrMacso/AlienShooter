using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlasterShot : MonoBehaviour
{
    [SerializeField] float _speed = 8.0f;
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
        var dog = collision.gameObject.GetComponent<Dog>();
        if (dog != null)
        {
            dog.TakeDamage();
        }
        gameObject.SetActive(false);
    }
}
