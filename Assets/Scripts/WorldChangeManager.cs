using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class WorldChangeManager : MonoBehaviour
{
    [Header("World References")]
    [SerializeField] private GameObject _realityContainer;
    [SerializeField] private GameObject _irrealityContainer;
    [SerializeField] private List<GameObject> _irrealityObjects;

    public WorldType ActiveWorldType { get; private set; }

    private WorldChangeEvent OnWorldChanged = new WorldChangeEvent();
    
    public void ChangeToIrreality()
    {
        //_irrealityContainer.SetActive(true);
        foreach (var irrealityObject in _irrealityObjects)
        {
            irrealityObject.gameObject.SetActive(true);
        }
        
        ActiveWorldType = WorldType.GoodWorld_Irreality;
        GameManager.Instance.AudioController.ActivateGoodMusic();
    }

    public void ChangeToReality()
    {
        //_irrealityContainer.SetActive(false);
        foreach (var irrealityObject in _irrealityObjects)
        {
            irrealityObject.gameObject.SetActive(false);
        }
        
        ActiveWorldType = WorldType.BadWorld_Reality;
        GameManager.Instance.AudioController.ActivateBadMusic();
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
