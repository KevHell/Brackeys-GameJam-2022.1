using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
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
    [SerializeField] private TMP_Text _slotCount;
    private PlantSpotController _currentPlantSpotController;
    private int _plantingOptionIndex;
    private List<Item> _plantingOptions;
    
    [Header("Crafting References")]
    [SerializeField] private GameObject _craftingPanel;
    [SerializeField] private Color _disabledColor;
    [SerializeField] private List<Image> _craftingImages;
    [SerializeField] private List<Button> _decreaseButtons;
    [SerializeField] private List<Button> _increaseButtons;
    [SerializeField] private List<TMP_Text> _craftCountTexts;
    private Dictionary<ItemType, int> _craftingCounts = new Dictionary<ItemType, int>();
    private Dictionary<ItemType, int> _craftingOptions = new Dictionary<ItemType, int>();

    [Header("Health References")]
    [SerializeField] private List<GameObject> _healthContainer = new List<GameObject>();
    [SerializeField] private float _decreaseFlickerRate = 0.5f;
    private int _lastHealth;

    [Header("TextBox References")]
    [SerializeField] private GameObject _textBoxObject;
    [SerializeField] private TMP_Text _textBoxText;
    [SerializeField] private GameObject _okBox;
    private char[] _textChars;
    [SerializeField] private float _typewriteDelay = 0.2f;
    private int _charIndex;
    private string _desiredText;
    public UnityEvent OnBoxClosed = new UnityEvent();

    [Header("Shooting References")]
    [SerializeField] private List<GameObject> _bulletContainer;
    [SerializeField] private Color _disabledBulletColor;
    
    [Header("GameOver References")]
    [SerializeField] private GameObject _gameOver;
    [SerializeField] private GameObject _winPanel;

    
    private void Start()
    {
        GameManager.Instance.InputController.SwitchInputMode(InputMode.MainGame);
        
        _activePanel = _hud;
        _lastHealth = PlayerController.Instance.HealthController.CurrentHealth;
    }

    #region Menu Handling

    public void ShowHud()
    {
        ActivatePanel(_hud);
        //GameManager.Instance.InputController.SwitchInputMode(InputMode.MainGame);
    }

    public void ShowGameOverPanel()
    {
        ActivatePanel(_gameOver);
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
        GameManager.Instance.InputController.SwitchInputMode(InputMode.Menu);
        
        _plantingOptions = plantingOptions;
        _currentPlantSpotController = plantSpotController;
        ActivatePanel(_plantingPanel);

        _plantingOptionIndex = 0;
        _seedDisplayImage.sprite = _plantingOptions[_plantingOptionIndex].UISprite;
        _slotCount.text = (_plantingOptionIndex + 1).ToString();
    }

    public void NextSeedOption()
    {
        _plantingOptionIndex++;
        if (_plantingOptionIndex == _plantingOptions.Count) _plantingOptionIndex = 0;
        
        _seedDisplayImage.sprite = _plantingOptions[_plantingOptionIndex].UISprite;
        _slotCount.text = (_plantingOptionIndex + 1).ToString();
    }
    public void PreviousSeedOption()
    {
        _plantingOptionIndex--;
        if (_plantingOptionIndex < 0) _plantingOptionIndex = _plantingOptions.Count - 1;
        
        _seedDisplayImage.sprite = _plantingOptions[_plantingOptionIndex].UISprite;
        _slotCount.text = (_plantingOptionIndex + 1).ToString();
    }

    public void Plant()
    {
        _currentPlantSpotController.PlantSeed(_plantingOptions[_plantingOptionIndex]);
        GameManager.Instance.InputController.SwitchInputMode(InputMode.MainGame);
    }

    #endregion

    #region Crafting Handle

    public void ShowCraftingPanel()
    {
        ActivatePanel(_craftingPanel);
        GameManager.Instance.InputController.SwitchInputMode(InputMode.Menu);
    }

    public void UpdatePanel(Dictionary<ItemType, int> craftingOptions)
    {
        _craftingOptions = craftingOptions;
        
        foreach (var itemType in _craftingOptions.Keys)
        {
            int index = itemType switch
            {
                ItemType.RedFlowerSeed => 0,
                ItemType.BlueFlowerSeed => 1,
                ItemType.YellowFlowerSeed => 2,
                _ => 0
            };
                
            if (_craftingOptions[itemType] == 0)
            {
                _craftingImages[index].color = _disabledColor;
                _decreaseButtons[index].interactable = false;
                _increaseButtons[index].interactable = false;
            }
            else
            {
                _craftingImages[index].color = Color.white;
                _decreaseButtons[index].interactable = false;
                _increaseButtons[index].interactable = true;
            }
        }
    }

    public void UpdateCraftCountByType(ItemType itemType)
    {
        switch (itemType)
        {
            case ItemType.RedFlowerSeed:
                _craftCountTexts[0].text = _craftingCounts[itemType] + "x";
                break;
            case ItemType.BlueFlowerSeed:
                _craftCountTexts[1].text = _craftingCounts[itemType] + "x";
                break;
            case ItemType.YellowFlowerSeed:
                _craftCountTexts[2].text = _craftingCounts[itemType] + "x";
                break;
        }
    }

    private void IncreaseCraftingCountOfType(ItemType itemType)
    {
        if (_craftingCounts.ContainsKey(itemType)) _craftingCounts[itemType]++;
        else _craftingCounts.Add(itemType, 1);
        
        UpdateCraftCountByType(itemType);

        Debug.Log(_craftingCounts[itemType]);
        
        int index = itemType switch
        {
            ItemType.RedFlowerSeed => 0,
            ItemType.BlueFlowerSeed => 1,
            ItemType.YellowFlowerSeed => 2,
            _ => 0
        };
        
        _decreaseButtons[index].interactable = true;
        
        if (_craftingCounts[itemType] == _craftingOptions[itemType])
        {
            _craftingImages[index].color = _disabledColor;
            _decreaseButtons[index].interactable = true;
            _increaseButtons[index].interactable = false;
        }
    }

    private void DecreaseCraftingCountOfType(ItemType itemType)
    {
        if (_craftingCounts.ContainsKey(itemType)) _craftingCounts[itemType]--;
        
        
        UpdateCraftCountByType(itemType);
        
        int index = itemType switch
        {
            ItemType.RedFlowerSeed => 0,
            ItemType.BlueFlowerSeed => 1,
            ItemType.YellowFlowerSeed => 2,
            _ => 0
        };
        
        if (_craftingCounts[itemType] == 0)
        {
            _decreaseButtons[index].interactable = false;
        }
        
        _craftingImages[index].color = Color.white;
        _increaseButtons[index].interactable = true;
    }

    public void IncreaseCraftRed()
    {
        IncreaseCraftingCountOfType(ItemType.RedFlowerSeed);
    }

    public void DecreaseCraftRed()
    {
        DecreaseCraftingCountOfType(ItemType.RedFlowerSeed);
    }
    
    public void IncreaseCraftBlue()
    {
        IncreaseCraftingCountOfType(ItemType.BlueFlowerSeed);
    }
    public void DecreaseCraftBlue()
    {
        DecreaseCraftingCountOfType(ItemType.BlueFlowerSeed);
    }
    
    public void IncreaseCraftYellow()
    {
        IncreaseCraftingCountOfType(ItemType.YellowFlowerSeed);
    }
    public void DecreaseCraftYellow()
    {
        DecreaseCraftingCountOfType(ItemType.YellowFlowerSeed);
    }
    
    public void ResetCrafting()
    {
        _craftingCounts[ItemType.RedFlowerSeed] = 0;
        UpdateCraftCountByType(ItemType.RedFlowerSeed);
        
        _craftingCounts[ItemType.BlueFlowerSeed] = 0;
        UpdateCraftCountByType(ItemType.BlueFlowerSeed);
        
        _craftingCounts[ItemType.YellowFlowerSeed] = 0;
        UpdateCraftCountByType(ItemType.YellowFlowerSeed);
    }

    public void CraftItems()
    {
        foreach (var itemType in _craftingCounts.Keys)
        {
            for (int i = 0; i < _craftingCounts[itemType]; i++)
            {
                GameManager.Instance.CraftingController.CraftItemOfType(itemType);
            }
        }
        
        GameManager.Instance.InputController.SwitchInputMode(InputMode.MainGame);
        GameManager.Instance.CraftingController.StopCrafting();
        ResetCrafting();
    }

    #endregion
    
    #region Health Handling

    public void UpdateHealth(int currentHealth)
    {
        if (_lastHealth > currentHealth)
        {
            _lastHealth = currentHealth;
            StartCoroutine(nameof(FlickerHealthContainer));
            return;
        }
        
        _lastHealth = currentHealth;
        
        foreach (var healthContainer in _healthContainer)
        {
            healthContainer.SetActive(false);
        }

        for (int i = 0; i < currentHealth; i++)
        {
            _healthContainer[i].SetActive(true);
        }
    }
    
    private IEnumerator FlickerHealthContainer()
    {
        int index = _lastHealth;
        _healthContainer[index].SetActive(false);
        yield return new WaitForSeconds(_decreaseFlickerRate);
        
        _healthContainer[index].SetActive(true);
        yield return new WaitForSeconds(_decreaseFlickerRate);
        
        _healthContainer[index].SetActive(false);
        yield return new WaitForSeconds(_decreaseFlickerRate);
        
        _healthContainer[index].SetActive(true);
        yield return new WaitForSeconds(_decreaseFlickerRate);
        
        _healthContainer[index].SetActive(false);
        yield return new WaitForSeconds(_decreaseFlickerRate);
    }
    
    #endregion

    #region TextBox Handling

    public void DisplayTextInTextBox(string text)
    {
        GameManager.Instance.PauseGame();
        GameManager.Instance.InputController.SwitchInputMode(InputMode.TextBox);
        
        _desiredText = text;
        _textBoxText.text = "";
        _okBox.SetActive(false);
        _textBoxObject.SetActive(true);
        _textChars = _desiredText.ToCharArray();
        _charIndex = 0;

        StartCoroutine(nameof(TypewriteText));
    }
    
    private IEnumerator TypewriteText()
    {
        _textBoxText.text += _textChars[_charIndex].ToString();
        _charIndex++;
        
        yield return new WaitForSecondsRealtime(_typewriteDelay);
        
        if (_charIndex < _textChars.Length)
        {
            StartCoroutine(nameof(TypewriteText));
        }
        else
        {
            yield return new WaitForSecondsRealtime(.5f);
            _okBox.SetActive(true);
        }
    }

    public void CloseTextBox()
    {
        _textBoxObject.SetActive(false);
        GameManager.Instance.InputController.SwitchInputMode(InputMode.MainGame);
        GameManager.Instance.ResumeGame();
    }


    #endregion

    #region ShootHandling

    public void UpdateBullets(int currentStack)
    {
        foreach (var bullet in _bulletContainer)
        {
            bullet.SetActive(false);
        }

        for (int i = 0; i < currentStack; i++)
        {
            _bulletContainer[i].SetActive(true);
        }
    }

    public void SetDisableBulletColor()
    {
        foreach (var bullet in _bulletContainer)
        {
            bullet.GetComponent<Image>().color = _disabledBulletColor;
        }
    }

    public void SetDefaultBulletColor()
    {
        foreach (var bullet in _bulletContainer)
        {
            bullet.GetComponent<Image>().color = Color.white;
        }
    }

    #endregion

    public void ShowWinPanel()
    {
        ActivatePanel(_winPanel);
    }
}
