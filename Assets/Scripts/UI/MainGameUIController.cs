using System.Collections;
using System.Collections.Generic;
using UnityEditor;
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

    [Header("RDM References")]
    [SerializeField] private Image _rdmEnergyFill;
    
    [Header("Item Bag References")]
    [SerializeField] private Animator _itemBagAnimator;
    [SerializeField] private List<Image> _slotImages;

    [Header("Planting References")]
    [SerializeField] private GameObject _plantingPanel;
    [SerializeField] private Image _seedDisplayImage;
    private PlantSpotController _currentPlantSpotController;
    private int _plantingOptionIndex;
    private List<Item> _plantingOptions;

    private void Start()
    {
        _activePanel = _hud;
    }

    #region Menu Handling

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
        CloseActivePanel();
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

    #endregion

    #region RDM Handling

    public void UpdateRDMEnergy(float value, Color color)
    {
        _rdmEnergyFill.fillAmount = value;
        _rdmEnergyFill.color = color;
    }

    #endregion
    
    #region Bag Handling

    public void ShowNoBagSpaceLeft()
    {
        
    }

    public void ShowItemBag()
    {
        _itemBagAnimator.SetBool("Show", true);
    }

    public void HideItemBag()
    {
        _itemBagAnimator.SetBool("Show", false);
    }

    public void UpdateItemsInBag(List<Item> items)
    {
        foreach (var image in _slotImages)
        {
            image.gameObject.SetActive(false);
        }

        for (int i = 0; i < items.Count; i++)
        {
            _slotImages[i].sprite = items[i].UISprite;
            _slotImages[i].gameObject.SetActive(true);
        }
    }
    public void RemoveItemFromBag(int index)
    {
        _slotImages[index].gameObject.SetActive(false);
    }

    #endregion

    #region PlantingHandling

    public void ShowPlantingMenu(List<Item> plantingOptions, PlantSpotController plantSpotController)
    {
        _plantingOptions = plantingOptions;
        _currentPlantSpotController = plantSpotController;
        ActivatePanel(_plantingPanel);

        _plantingOptionIndex = 0;
        _seedDisplayImage.sprite = _plantingOptions[_plantingOptionIndex].UISprite;
    }

    public void NextSeedOption()
    {
        _plantingOptionIndex++;
        if (_plantingOptionIndex == _plantingOptions.Count) _plantingOptionIndex = 0;
        
        _seedDisplayImage.sprite = _plantingOptions[_plantingOptionIndex].UISprite;
    }
    public void PreviousSeedOption()
    {
        _plantingOptionIndex--;
        if (_plantingOptionIndex < 0) _plantingOptionIndex = _plantingOptions.Count - 1;
        
        _seedDisplayImage.sprite = _plantingOptions[_plantingOptionIndex].UISprite;
    }

    public void Plant()
    {
        _currentPlantSpotController.PlantSeed(_plantingOptions[_plantingOptionIndex]);
    }

    #endregion
}
