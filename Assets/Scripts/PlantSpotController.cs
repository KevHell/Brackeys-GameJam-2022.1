using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class PlantSpotController : MonoBehaviour
{
    [SerializeField] private Interactable _interactable;
    private List<Item> _plantingOptions = new List<Item>();
    [SerializeField] private SpriteRenderer _seedlingRenderer;
    [SerializeField] private SpriteRenderer _badRenderer;
    [SerializeField] private List<SeedType> _seedTypes;
    private SeedType _seedType;
    private bool _planted;
    [FormerlySerializedAs("_fullGrown")] public bool FullGrown;
    private float _timer;
    private int _spriteCounter;

    public UnityEvent OnPlanted = new UnityEvent();

    public void StartPlanting()
    {
        GetPlantingOptions();

        if (_plantingOptions.Count > 0)
        {
            GameManager.Instance.PauseGame();
            GameManager.Instance.MainGameUIController.ShowPlantingMenu(_plantingOptions, this);
        }
        else
        {
            GameManager.Instance.MainGameUIController.DisplayTextInTextBox("Seems like you don't have any seeds to plant...");
        }
    }

    private void Update()
    {
        if (_planted && !FullGrown)
        {
            _timer += Time.deltaTime;
            if (_timer >= _seedType.GrowthRateInSeconds)
            {
                _spriteCounter++;
                if (_spriteCounter < _seedType.GrowSprites.Count)
                {
                    _seedlingRenderer.sprite = _seedType.GrowSprites[_spriteCounter];
                    _badRenderer.sprite = _seedType.GrowSprites[_spriteCounter];
                    Debug.Log("Call");
                }
                else
                {
                    FullGrown = true;
                    GameManager.Instance.CheckIfAllSpotsPlanted();
                }

                _timer = 0;
            }
        }
    }

    public void PlantSeed(Item item)
    {
        _interactable.Deactivate();
        
        PlayerController.Instance.ItemBag.RemoveItemOfType(item.Type);
        
        GameManager.Instance.ResumeGame();
        GameManager.Instance.MainGameUIController.ShowHud();

        GetSeedTypeByItem(item);
        
        _seedlingRenderer.sprite = _seedType.GrowSprites[0];
        _badRenderer.sprite = _seedType.GrowSprites[0];
        _seedlingRenderer.gameObject.SetActive(true);
        _badRenderer.gameObject.SetActive(true);

        _planted = true;
        OnPlanted.Invoke();
    }

    private void GetPlantingOptions()
    {
        
        List<Item> items = PlayerController.Instance.ItemBag.Items;
        if (items.Count == 0) return;
        
        for (int i = items.Count - 1; i >= 0; i--)
        {
            if (items[i].IsSeed)
            {
                _plantingOptions.Add(items[i]);
            }
        }
    }

    private void GetSeedTypeByItem(Item item)
    {
        foreach (var seed in _seedTypes)
        {
            if (seed.ItemType == item.Type)
            {
                _seedType = seed;
                break;
            }
        }
    }
}
