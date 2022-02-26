using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class CraftingController : MonoBehaviour
{
    [SerializeField] private List<CraftingItem> _craftingItems;
    private Dictionary<ItemType, int> _ingredientCounts = new Dictionary<ItemType, int>();
    private Dictionary<ItemType, int> _craftingOptions = new Dictionary<ItemType, int>();

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            CraftItemOfType(ItemType.RedFlowerSeed);
        }
    }

    private CraftingItem GetCraftingItemByOutputType(ItemType outputType)
    {
        foreach (var item in _craftingItems)
        {
            if (item.OutputItem == outputType)
            {
                return item;
            }
        }

        return _craftingItems[0];
    }

    public void CraftItemOfType(ItemType itemType)
    {
        CraftingItem itemToCraft = GetCraftingItemByOutputType(itemType);
        
        // Remove ingredients from bag
        for (int i = 0; i < itemToCraft.RequiredAmount; i++)
        {
            PlayerController.Instance.ItemBag.RemoveItemOfType(itemToCraft.RequiredItem);
        }

        // Instantiate Seedling
        GameObject craftedObject = Instantiate(itemToCraft.Prefab, transform.position, Quaternion.identity,
            PlayerController.Instance.ItemBag.transform);
        Item craftedItem = craftedObject.GetComponent<Item>();
        
        // add it to bag
        PlayerController.Instance.ItemBag.TryToStoreItem(craftedItem, out bool success);
    }

    public void StartCrafting()
    {
        GameManager.Instance.PauseGame();

        for (int i = 0; i < _ingredientCounts.Count; i++)
        {
            ItemType type = _ingredientCounts.Keys.ElementAt(i);
            _ingredientCounts[type] = 0;
        }
        
        for (int i = 0; i < _craftingOptions.Count; i++)
        {
            ItemType type = _craftingOptions.Keys.ElementAt(i);
            _craftingOptions[type] = 0;
        }
        
        CalculateIngredients();
        CalculateCraftingOptions();
        
        GameManager.Instance.MainGameUIController.UpdatePanel(_craftingOptions);
        GameManager.Instance.MainGameUIController.ShowCraftingPanel();
    }

    public void StopCrafting()
    {
        GameManager.Instance.ResumeGame();
        GameManager.Instance.MainGameUIController.ShowHud();
        GameManager.Instance.MainGameUIController.ResetCrafting();
    }

    private void CalculateIngredients()
    {
        List<Item> _bagItems = PlayerController.Instance.ItemBag.Items;

        foreach (var item in _bagItems)
        {
            if (item.IsSeed) continue;
            
            if (_ingredientCounts.ContainsKey(item.Type)) _ingredientCounts[item.Type]++;
            else _ingredientCounts.Add(item.Type, 1);
        }
    }

    private void CalculateCraftingOptions()
    {
        for (int i = 0; i < _craftingItems.Count; i++)
        {
            CraftingItem craftingItem = _craftingItems[i];
            int requiredAmount = craftingItem.RequiredAmount;
            ItemType requiredType = craftingItem.RequiredItem;
            ItemType outputType = craftingItem.OutputItem;

            int availableAmount = 0;
            if (_ingredientCounts.ContainsKey(requiredType)) availableAmount = _ingredientCounts[requiredType];
            else
            {
                if (_craftingOptions.ContainsKey(outputType)) _craftingOptions[outputType] = 0;
                else _craftingOptions.Add(outputType, 0);
                continue;
            }

            if (availableAmount == 0)
            {
                if (_craftingOptions.ContainsKey(outputType)) _craftingOptions[outputType] = 0;
                else _craftingOptions.Add(outputType, 0);
                continue;
            }

            int possibleCrafts = Mathf.FloorToInt(availableAmount / requiredAmount);

            if (possibleCrafts == 0)
            {
                if (_craftingOptions.ContainsKey(outputType)) _craftingOptions[outputType] = 0;
                else _craftingOptions.Add(outputType, 0);
                continue;
            }
            
            if (_craftingOptions.ContainsKey(outputType)) _craftingOptions[outputType] = possibleCrafts;
            else _craftingOptions.Add(outputType, possibleCrafts);
        }
    }
}

[System.Serializable]
public struct CraftingItem
{
    public string Name;
    [Header("Input")]
    public ItemType RequiredItem;
    public int RequiredAmount;
    [Header("Output")]
    public ItemType OutputItem;
    public int OutputAmount;
    public GameObject Prefab;
}