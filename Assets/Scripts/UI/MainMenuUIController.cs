using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuUIController : MonoBehaviour
{
    [Header("Panel References")]
    [SerializeField] private GameObject _mainPanel;
    [SerializeField] private GameObject _optionsPanel;
    [SerializeField] private GameObject _creditsPanel;
    [SerializeField] private GameObject _quitPanel;
    private GameObject _activePanel;

    private void Awake()
    {
        
    }

    private void Start()
    {
        GameManager.Instance.InputController.SwitchInputMode(InputMode.Menu);
        _activePanel = _mainPanel;
    }

    public void ShowMainPanel()
    {
        ActivatePanel(_mainPanel);
    }
    
    public void ShowOptionsPanel()
    {
        ActivatePanel(_optionsPanel);
    }

    public void ShowCreditsPanel()
    {
        ActivatePanel(_creditsPanel);
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
