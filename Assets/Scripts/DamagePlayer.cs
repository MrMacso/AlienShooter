using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DamagePlayer : MonoBehaviour
{
    [SerializeField] bool _ignoreFromTop;
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (_ignoreFromTop && Vector2.Dot(collision.contacts[0].normal, Vector2.down) > 0.5)
            return;

        if (collision.collider.CompareTag("Player"))
        {
            var player = collision.collider.GetComponent<Player>();
            if (player != null)
            {
                player.TakeDamage(collision.contacts[0].normal);
            }
        }
    }
}
