using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableController : MonoBehaviour
{
    private Interactable _currentInteractable;

    public void TryToInteract()
    {
        if (!_currentInteractable) return;

        _currentInteractable.Interact();
    }

    public void SetCurrentInteractable(Interactable interactable)
    {
        _currentInteractable = interactable;
    }

    public void ClearCurrentInteractable()
    {
        _currentInteractable = null;
    }
}
