using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Interactable : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Animator _tooltipAnimator;
    public GameObject InteractionIndicator;
    public Image InteractionIndicatorFill;

    [Header("Settings")]
    public bool CanInteract = true;
    public float InteractDuration = 0;
    
    [Header("Interaction Reactions")]
    public UnityEvent OnInteraction = new UnityEvent();

    private bool _tooltipActive;

    private void Update()
    {
        if (_tooltipActive && !GameManager.Instance.RealityDistortionModule.Active)
        {
            PlayerController.Instance.InteractableController.ClearCurrentInteractable();
            _tooltipAnimator.SetBool("Active", false);
            _tooltipActive = false;
        }
    }

    public void Interact()
    {
        OnInteraction.Invoke();
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (!GameManager.Instance.RealityDistortionModule.Active || !CanInteract || PlayerController.Instance.InteractableController.CurrentInteractable) return;
        ActivateTooltip();
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (!GameManager.Instance.RealityDistortionModule.Active || _tooltipActive || !CanInteract || PlayerController.Instance.InteractableController.CurrentInteractable) return;
        ActivateTooltip();
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!GameManager.Instance.RealityDistortionModule.Active || !CanInteract) return;
        
        PlayerController.Instance.InteractableController.ClearCurrentInteractable();
        _tooltipAnimator.SetBool("Active", false);
        _tooltipActive = false;
    }

    private void ActivateTooltip()
    {
        PlayerController.Instance.InteractableController.SetCurrentInteractable(this);
        _tooltipAnimator.SetBool("Active", true);
        _tooltipActive = true;
    }

    public void Deactivate()
    {
        CanInteract = false;
        _tooltipAnimator.SetBool("Active", false);
        _tooltipActive = false;
    }
}
