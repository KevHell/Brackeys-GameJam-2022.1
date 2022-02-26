using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public bool GamePaused { get; private set; }
    
    [Header("General Settings")]
    [SerializeField] private bool _dontDestroyOnLoad = true;

    [Header("References")]
    public AudioController AudioController;
    [SerializeField] private CustomSceneManager _customSceneManager;
    [FormerlySerializedAs("_mainMenuUIController")] [SerializeField] public MainMenuUIController MainMenuUIController;
    public MainGameUIController MainGameUIController;
    public RealityDistortionModule RealityDistortionModule;
    public WorldChangeManager WorldChangeManager;
    public CraftingController CraftingController;
    public InputController InputController;
    
    // Public Class References
    public CustomSceneManager CustomSceneManager { get { return _customSceneManager; } }

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

    private void Update()
    {
        // Handle ESC-press in MainMenu and MainGame
        
        
    }

    #region World Change Handling
    
    #endregion

    #region Pause/Resume Management

    public void PauseGame()
    {
        GamePaused = true;
        Time.timeScale = 0.0f;
    }
    public void ResumeGame()
    {
        GamePaused = false;
        Time.timeScale = 1.0f;
    }

    #endregion

    #region Scene Managing & Quit
    public void LoadMainMenu()
    {
        SceneManager.LoadScene(0);
    }
    public void LoadMainGame()
    {
        SceneManager.LoadScene(1);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
    #endregion

    public void GameOver()
    {
        PauseGame();
        MainGameUIController.ShowGameOverPanel();
    }
}



