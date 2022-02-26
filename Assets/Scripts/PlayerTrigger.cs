using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTrigger : MonoBehaviour
{
    [SerializeField] private EnemyController _enemyController;
    
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.GetComponent<PlayerController>())
        {
            _enemyController.TryShooting();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.GetComponent<PlayerController>())
        {
            _enemyController.StopShooting();
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.GetComponent<PlayerController>())
        {
            _enemyController.TryShooting();
        }
    }
}
