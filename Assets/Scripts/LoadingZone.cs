using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingZone : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.GetComponent<PlayerController>())
        {
            GameManager.Instance.RealityDistortionModule.StartLoadingOverTime();
        }
    }
}
