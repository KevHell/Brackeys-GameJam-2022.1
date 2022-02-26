using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float _speed = 1;
    [HideInInspector] public Vector3 MovementDirection;
    [HideInInspector] public bool Go;
    [SerializeField] private Rigidbody2D _rigidbody2D;
    void Start()
    {
        _rigidbody2D.AddForce(transform.up * _speed, ForceMode2D.Impulse);
    }

    protected virtual void HandleCollision()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        HandleCollision();
    }
}
