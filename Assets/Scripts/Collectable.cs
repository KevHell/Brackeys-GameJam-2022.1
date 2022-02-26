using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Collectable : MonoBehaviour
{
    [SerializeField] private CollectableType _type;

    [SerializeField] private float _batteryLoadAmountMultiplier = 0.33f;
    [SerializeField] private int _healAmount = 1;
    [SerializeField] private float _xHalfBound;
    [SerializeField] private float _yHalfBound;
    [SerializeField] private float _popUpDelay;

    [SerializeField] private GameObject _badGraphicsObject;
    [SerializeField] private GameObject _goodGraphicsObject;
    [SerializeField] private GameObject _collisionObject;
    [SerializeField] private Collider2D _collider2D;
    
    private void OnTriggerEnter2D(Collider2D col)
    {
        // Do stuff depending on type
        switch (_type)
        {
            case CollectableType.Battery:
                HandleBattery();
                break;
            case CollectableType.Health:
                HandleHealth();
                break;
        }
    }

    private void HandlePickUp()
    {
        // Hide self
        _badGraphicsObject.SetActive(false);
        _goodGraphicsObject.SetActive(false);
        _collisionObject.SetActive(false);
        _collider2D.enabled = false;

        // Pop Up at random point on map
        /* Find Random point no map */
        float rdmX = Random.Range(-_xHalfBound, _xHalfBound);
        float rdmY = Random.Range(-_xHalfBound, _xHalfBound);
        transform.position = new Vector3(rdmX, rdmY, 0);

        StartCoroutine(nameof(ShowAfterSeconds));
    }

    private IEnumerator ShowAfterSeconds()
    {
        yield return new WaitForSeconds(_popUpDelay);
        
        _badGraphicsObject.SetActive(true);
        _goodGraphicsObject.SetActive(true);
        _collisionObject.SetActive(true);
        _collider2D.enabled = true;
    }
    
    private void HandleBattery()
    {
        float maxEnergy = GameManager.Instance.RealityDistortionModule._maxEnergyInSeconds;
        float energyLeft = GameManager.Instance.RealityDistortionModule.EnergySecondsLeft;

        if (energyLeft >= maxEnergy) return;
        
        float amount = GameManager.Instance.RealityDistortionModule._maxEnergyInSeconds * _batteryLoadAmountMultiplier;
        GameManager.Instance.RealityDistortionModule.LoadInstantly(amount);
        
        HandlePickUp();
    }
    private void HandleHealth()
    {
        HealthController health = PlayerController.Instance.HealthController;
        if (health.CurrentHealth >= health.MaxHealth) return;
        
        PlayerController.Instance.HealthController.IncreaseHealth(_healAmount);
        
        HandlePickUp();
    }
}

public enum CollectableType
{
    Battery,
    Health
}