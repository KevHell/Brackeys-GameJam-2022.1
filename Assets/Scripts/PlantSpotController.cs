using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantSpotController : MonoBehaviour
{
    [SerializeField] private Interactable _interactable;
    private List<Item> _plantingOptions = new List<Item>();
    [SerializeField] private SpriteRenderer _seedlingRenderer;
    [SerializeField] private List<SeedType> _seedTypes;
    private SeedType _seedType;

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
            // Show no seeds message
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
        _seedlingRenderer.gameObject.SetActive(true);
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
