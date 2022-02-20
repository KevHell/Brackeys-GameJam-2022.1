using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class WorldChangeManager : MonoBehaviour
{
    [Header("World References")]
    [SerializeField] private GameObject _realityContainer;
    [SerializeField] private GameObject _irrealityContainer;

    public WorldType ActiveWorldType { get; private set; }

    private WorldChangeEvent OnWorldChanged = new WorldChangeEvent();
    
    public void ChangeToIrreality()
    {
        _irrealityContainer.SetActive(true);
        ActiveWorldType = WorldType.GoodWorld_Irreality;
    }

    public void ChangeToReality()
    {
        _irrealityContainer.SetActive(false);
        ActiveWorldType = WorldType.BadWorld_Reality;
    }

    #region Event Management
    public void RegisterToWorldChange(UnityAction<WorldType> action)
    {
        OnWorldChanged.AddListener(action);
    }
    public void UnregisterFromWorldChange(UnityAction<WorldType> action)
    {
        OnWorldChanged.RemoveListener(action);
    }
    #endregion
    
}

public class WorldChangeEvent : UnityEvent<WorldType>
{
    
}
public enum WorldType
{
    BadWorld_Reality,
    GoodWorld_Irreality
}
