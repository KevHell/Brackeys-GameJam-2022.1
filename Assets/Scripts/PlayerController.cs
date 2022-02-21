using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance { get; private set; }
    
    [Header("General Settings")]
    [SerializeField] private bool _dontDestroyOnLoad = true;

    public PlayerMovement PlayerMovement;
    
    private void Awake()
    {
        // Initialize Singleton
        if (Instance != null && Instance != this) Destroy(gameObject);
        else 
        {
            Instance = this;
            if (_dontDestroyOnLoad) DontDestroyOnLoad(gameObject);
        }
    }
}
