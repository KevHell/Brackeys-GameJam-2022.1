using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBag : MonoBehaviour
{
    public List<Item> Items = new List<Item>();
    [SerializeField] private int _itemSlots = 5;
    public bool SpaceInBag = true;

    private Dictionary<ItemType, int> _itemsCounts = new Dictionary<ItemType, int>();

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            RemoveItemOfType(ItemType.RedFlower);
        }
    }

    public void TryToStoreItem(Item item, out bool success)
    {
        if (Items.Count < _itemSlots)
        {
            Items.Add(item);

            if (!_itemsCounts.ContainsKey(item.Type)) _itemsCounts.Add(item.Type, 0);
            _itemsCounts[item.Type] += 1;
            
            GameManager.Instance.MainGameUIController.UpdateItemsInBag(Items);
            success = true;

            if (Items.Count == _itemSlots)
            {
                SpaceInBag = false;
            }
            
            Debug.Log("Success");
        }
        else
        {
            GameManager.Instance.MainGameUIController.ShowNoBagSpaceLeft();
            success = false;
            Debug.Log("Fail");
        }
    }

    
    public void RemoveItemOfType(ItemType itemType)
    {
        foreach (var loopItem in Items)
        {
            if (loopItem.Type != itemType) continue;
            Items.Remove(loopItem);
            _itemsCounts[itemType] -= 1;
            break;
        }
        
        SpaceInBag = true;
        
        GameManager.Instance.MainGameUIController.UpdateItemsInBag(Items);
    }
}
