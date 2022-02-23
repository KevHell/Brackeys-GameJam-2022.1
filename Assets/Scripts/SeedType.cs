using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SeedType", menuName = "ScriptableObjects/SeedType", order = 1)]
public class SeedType : ScriptableObject
{
    public ItemType ItemType;
    public List<Sprite> GrowSprites;
    public float GrowthRateInSeconds;
}
