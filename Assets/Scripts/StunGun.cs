using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class StunGun : MonoBehaviour
{
    [SerializeField] private GameObject _bulletPrefab;
    [SerializeField] private Transform _firePoint;
    [SerializeField] private int _stacks = 3;

    public void Shoot()
    {
        
        
        GameObject bulletObject = Instantiate(_bulletPrefab, _firePoint.position, _firePoint.rotation);
        Bullet bullet = bulletObject.GetComponent<Bullet>();
        Rigidbody2D rb2D = bulletObject.GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        Vector3 mousePositionWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 direction = mousePositionWorld - transform.position;
        
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90;
        
        transform.rotation = Quaternion.Euler(0,0,angle);
    }
}
