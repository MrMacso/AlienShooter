using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water : MonoBehaviour
{
    AudioSource _audioSource;

    void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        _audioSource?.Play();
    }
    public void SetSpeed(float speed)
    { 
        GetComponent<BuoyancyEffector2D>().flowMagnitude= speed;
    }
}
