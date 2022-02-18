using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CustomSceneManager : MonoBehaviour
{
    [SerializeField] private string _mainMenuName;
    
    public void LoadMainMenu()
    {
        
    }

    public bool MainMenuIsActive()
    {
        return SceneManager.GetActiveScene().name == _mainMenuName;
    }
}
