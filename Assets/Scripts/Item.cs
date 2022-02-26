using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField] private bool _destroySelf;
    public Sprite UISprite;
    public ItemType Type;
    public bool IsSeed;

    public void Activate()
    {
        PlayerController.Instance.ItemBag.TryToStoreItem(this, out bool success);
        if (success && _destroySelf)
        {
            Destroy(gameObject);
        }
    }
}

public enum ItemType
{
    RedFlower,
    RedFlowerSeed,
    BlueFlower,
    BlueFlowerSeed,
    YellowFlower,
    YellowFlowerSeed
}
