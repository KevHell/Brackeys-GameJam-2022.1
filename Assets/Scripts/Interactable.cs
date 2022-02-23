using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Animator _tooltipAnimator;
    
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
        Debug.Log("Interact");
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (!GameManager.Instance.RealityDistortionModule.Active) return;
        ActivateTooltip();
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (!GameManager.Instance.RealityDistortionModule.Active || _tooltipActive) return;
        ActivateTooltip();
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!GameManager.Instance.RealityDistortionModule.Active) return;
        
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
}
