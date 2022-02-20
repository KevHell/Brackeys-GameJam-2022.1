using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public bool GamePaused { get; private set; }
    
    [Header("General Settings")]
    [SerializeField] private bool _dontDestroyOnLoad = true;

    [Header("References")]
    public AudioController AudioController;
    [SerializeField] private CustomSceneManager _customSceneManager;
    [SerializeField] private MainMenuUIController _mainMenuUIController;
    public MainGameUIController MainGameUIController;
    [SerializeField] private RealityDistortionModule _realityDistortionModule;
    public WorldChangeManager WorldChangeManager;
    
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
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (CustomSceneManager.MainMenuIsActive())
            {
                _mainMenuUIController.CloseActivePanel();
                _mainMenuUIController.ShowQuitPanel();
            }
            else
            {
                PauseGame();
                MainGameUIController.ShowPausePanel();
            }
        }
        
        // Handle World-Change-Input
        if (Input.GetKeyDown(KeyCode.Space) && !GamePaused)
        {
            _realityDistortionModule.ToggleModule();
        }
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
}



