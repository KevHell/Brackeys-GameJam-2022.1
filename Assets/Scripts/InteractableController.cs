using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableController : MonoBehaviour
{
    public Interactable CurrentInteractable;

    public void TryToInteract()
    {
        if (!CurrentInteractable || !CurrentInteractable.CanInteract) return;

        CurrentInteractable.Interact();
        CurrentInteractable = null;
    }

    public void SetCurrentInteractable(Interactable interactable)
    {
        CurrentInteractable = interactable;
    }

    public void ClearCurrentInteractable()
    {
        CurrentInteractable = null;
    }
}
