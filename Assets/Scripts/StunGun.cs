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
    [SerializeField] private float _reloadTime = 1.5f;

    private bool _canShoot = true;

    public void Shoot()
    {
        if (!_canShoot) return;

        _stacks--;
        if (_stacks == 0)
        {
            _canShoot = false;
            GameManager.Instance.MainGameUIController.SetDisableBulletColor();
            StartCoroutine(nameof(LoadWeapon));
        }
        
        GameManager.Instance.MainGameUIController.UpdateBullets(_stacks);
        
        GameObject bulletObject = Instantiate(_bulletPrefab, _firePoint.position, _firePoint.rotation);
    }

    private void Update()
    {
        Vector3 mousePositionWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 direction = mousePositionWorld - transform.position;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90;

        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    private IEnumerator LoadWeapon()
    {
        yield return new WaitForSeconds(_reloadTime);
        _stacks++;
        GameManager.Instance.MainGameUIController.UpdateBullets(_stacks);
        
        yield return new WaitForSeconds(_reloadTime);
        _stacks++;
        GameManager.Instance.MainGameUIController.UpdateBullets(_stacks);
        
        yield return new WaitForSeconds(_reloadTime);
        _stacks++;
        GameManager.Instance.MainGameUIController.UpdateBullets(_stacks);

        GameManager.Instance.MainGameUIController.SetDefaultBulletColor();
        _canShoot = true;
    }
}