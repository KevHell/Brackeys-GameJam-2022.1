using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainGameUIController : MonoBehaviour
{
    [Header("Panel References")]
    [SerializeField] private GameObject _hud;
    [SerializeField] private GameObject _pausePanel;
    [SerializeField] private GameObject _optionsPanel;
    [SerializeField] private GameObject _quitPanel;
    private GameObject _activePanel;

    [Header("Slider References")]
    [SerializeField] private Slider _masterVolumeSlider;
    [SerializeField] private Slider _musicVolumeSlider;
    [SerializeField] private Slider _effectVolumeSlider;

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
        UpdateVolumeSliders();
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

    private void UpdateVolumeSliders()
    {
        float master = 0;
        float music = 0;
        float effect = 0;

        GameManager.Instance.AudioController.GetMasterVolume(out master);
        _masterVolumeSlider.value = master;
        
        GameManager.Instance.AudioController.GetMusicVolume(out music);
        _masterVolumeSlider.value = music;
        
        GameManager.Instance.AudioController.GetEffectVolume(out effect);
        _masterVolumeSlider.value = effect;
    }
}
