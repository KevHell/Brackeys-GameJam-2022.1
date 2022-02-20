using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainGameUIController : MonoBehaviour
{
    [Header("Panel References")]
    [SerializeField] private GameObject _hud;
    [SerializeField] private GameObject _pausePanel;
    [SerializeField] private GameObject _optionsPanel;
    [SerializeField] private GameObject _quitPanel;
    private GameObject _activePanel;

    private void Start()
    {
        _activePanel = _hud;
    }

    public void ShowHud()
    {
        ActivatePanel(_hud);
    }
    
    public void ShowPausePanel()
    {
        ActivatePanel(_pausePanel);
    }
    
    public void ShowOptionsPanel()
    {
        ActivatePanel(_optionsPanel);
    }

    public void ShowQuitPanel()
    {
        ActivatePanel(_quitPanel);
    }

    private void ActivatePanel(GameObject panel)
    {
        _activePanel = panel;
        _activePanel.SetActive(true);
    }

    public void CloseActivePanel()
    {
        _activePanel.SetActive(false);
    }
}
