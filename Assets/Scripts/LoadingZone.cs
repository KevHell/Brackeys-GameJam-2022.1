using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingZone : MonoBehaviour
{
    [SerializeField] private Animator _badAnimator;
    [SerializeField] private Animator _goodAnimator;
    
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.GetComponent<PlayerController>())
        {
            GameManager.Instance.RealityDistortionModule.StartLoadingOverTime();
            _badAnimator.StartPlayback();
            _goodAnimator.StartPlayback();
        }
    }
}
