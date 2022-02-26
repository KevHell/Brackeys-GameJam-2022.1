using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float _speed = 1;
    [HideInInspector] public Vector3 MovementDirection;
    [HideInInspector] public bool Go;
    [SerializeField] private Rigidbody2D _rigidbody2D;
    [SerializeField] private float _destructionDelay = 10;
    void Start()
    {
        _rigidbody2D.AddForce(transform.up * _speed, ForceMode2D.Impulse);
        StartCoroutine(nameof(DestroyAfterSeconds));
    }

    private IEnumerator DestroyAfterSeconds()
    {
        yield return new WaitForSeconds(_destructionDelay);
        
        Debug.Log("Destroy");
        DestroySelf();
    }

    protected virtual void HandleCollision(Collision2D col)
    {
        Debug.Log("Handel Collision");
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.GetComponent<EnemyController>()) HandleEnemyCollision(col);
        if (LayerMask.LayerToName(col.gameObject.layer) == "Player") HandlePlayerCollision();
        
        // Destroy self
        DestroySelf();
    }

    private void HandleEnemyCollision(Collision2D col)
    {
        // Is hit an enemy, disable it
        EnemyController enemy = col.gameObject.GetComponent<EnemyController>();
        if (enemy) enemy.Disable();
        
        // Is hit another bullet, destroy it
        Bullet otherBullet = col.gameObject.GetComponent<Bullet>();
        if (otherBullet) otherBullet.DestroySelf();
    }

    private void HandlePlayerCollision()
    {
        PlayerController.Instance.HealthController.DecreaseHealth(1);
    }

    public void DestroySelf()
    {
        Destroy(gameObject);
    }
}
