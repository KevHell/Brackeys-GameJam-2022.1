using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CraftingController : MonoBehaviour
{
    [SerializeField] private List<CraftingItem> _craftingItems;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            CraftItemOfType(ItemType.TestFlowerSeed);
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