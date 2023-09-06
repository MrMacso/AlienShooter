using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatBomb : MonoBehaviour
{
    Rigidbody2D _rb;
    float _forceAmount = 300f;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }
    public void Launch(Vector2 direcrtion)
    {
        transform.SetParent(null);
        _rb.AddForce(direcrtion * _forceAmount);
    }
}
