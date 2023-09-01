using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField] Vector2 _direction= Vector2.left;
    [SerializeField] float _distance = 10f;
    [SerializeField] SpriteRenderer _laserBurst;
    LineRenderer _lineRenderer;
    bool _isOn;

    void Awake()
    {
        _lineRenderer= GetComponent<LineRenderer>();
        Toggle(false);
    }
    public void Toggle(bool state)
    {
        _isOn = state;
        _lineRenderer.enabled = state;
    }
    void Update()
    {
        if (!_isOn)
        {
            _laserBurst.enabled = false;
            return;
        }
        
        var endPoint = (Vector2)transform.position + (_direction * _distance);

        var firstThing = Physics2D.Raycast(transform.position, _direction, _distance);
        if (firstThing.collider)
        { 
            endPoint = firstThing.point;
            _laserBurst.transform.position = endPoint;
            _laserBurst.enabled = true;
            _laserBurst.transform.localScale = Vector3.one * (0.75f + Mathf.PingPong(Time.time, 0.5f));

            var laserDamageable = firstThing.collider.GetComponent<ITakeLaserDamage>();
            if (laserDamageable != null)
                laserDamageable.TakeLaserDamage();
        }
        else
            _laserBurst.enabled = false;

        _lineRenderer.SetPosition(1, endPoint);
    }
}
