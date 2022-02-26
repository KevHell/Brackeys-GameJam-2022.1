using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HealthController : MonoBehaviour
{
    public int MaxHealth;
    public int CurrentHealth;

    public UnityEvent OnDeath = new UnityEvent();

    private void Start()
    {
        CurrentHealth = MaxHealth;
    }

    public void IncreaseHealth(int amount)
    {
        CurrentHealth += amount;
        if (CurrentHealth > MaxHealth) CurrentHealth = MaxHealth;
        GameManager.Instance.MainGameUIController.UpdateHealth(CurrentHealth);
    }

    public void DecreaseHealth(int amount)
    {
        CurrentHealth -= amount;
        if (CurrentHealth <= 0) OnDeath.Invoke();
        GameManager.Instance.MainGameUIController.UpdateHealth(CurrentHealth);
    }
}
