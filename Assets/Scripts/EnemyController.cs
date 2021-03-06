using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private bool _active = true;
    [SerializeField] private float _deactivationDuration;
    [SerializeField] private float _shootRate;
    [SerializeField] private Transform _fireTransform;
    [SerializeField] private GameObject _enemyBulletPrefab;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private SpriteRenderer _goodSpriteRenderer;
    [SerializeField] private Sprite _disabledSprite;
    [SerializeField] private Sprite _enabledSprite;
    
    private bool _isShooting;

    public void Disable()
    {
        _active = false;
        _isShooting = false;

        // Play Particle

        // Play Sound

        // Start Coroutine
        StartCoroutine(ActivateAfterSeconds());
        _spriteRenderer.sprite = _disabledSprite;
        _goodSpriteRenderer.sprite = _disabledSprite;
    }

    private IEnumerator ActivateAfterSeconds()
    {
        yield return new WaitForSeconds(_deactivationDuration);
        
        // Stop Particle
        
        // Stop Sound
        
        // Activate
        _active = true;
        _spriteRenderer.sprite = _enabledSprite;
        _goodSpriteRenderer.sprite = _enabledSprite;
    }

    public void TryShooting()
    {
        if (!_active || _isShooting) return;
        _isShooting = true;
        StartCoroutine(nameof(ShootAtPlayer));
    }

    public void StopShooting()
    {
        StopCoroutine(ShootAtPlayer());
        _isShooting = false;
    }

    private IEnumerator ShootAtPlayer()
    {
        Vector3 playerPosition = PlayerController.Instance.transform.position;
        Vector3 direction = playerPosition - _fireTransform.position;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90;

        _fireTransform.rotation = Quaternion.Euler(0, 0, angle);


        if (_active && _isShooting)
        {
            Instantiate(_enemyBulletPrefab, _fireTransform.position, _fireTransform.rotation);
            
            yield return new WaitForSeconds(_shootRate);
            StartCoroutine(nameof(ShootAtPlayer));
        }
    }
}
