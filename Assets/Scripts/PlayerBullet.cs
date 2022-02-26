using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : Bullet
{
    protected override void HandleCollision(Collision2D col)
    {
        Debug.Log("Handle Child");
        
        base.HandleCollision(col);
        
        // Is hit an enemy, disable it
        EnemyController enemy = col.gameObject.GetComponent<EnemyController>();
        if (enemy) enemy.Disable();
        
        // Is hit another bullet, destroy it
        Bullet otherBullet = col.gameObject.GetComponent<Bullet>();
        if (otherBullet) otherBullet.DestroySelf();
        
        // Destroy self
        DestroySelf();
    }
}
