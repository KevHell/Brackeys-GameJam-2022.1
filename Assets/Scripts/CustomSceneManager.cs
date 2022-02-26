using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CustomSceneManager : MonoBehaviour
{
    [SerializeField] private string _mainMenuName;
    
    public void LoadMainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void LoadMainGame()
    {
        GameManager.Instance.ResumeGame();
        SceneManager.LoadScene(1);
    }

    public bool MainMenuIsActive()
    {
        return SceneManager.GetActiveScene().name == _mainMenuName;
    }
}
