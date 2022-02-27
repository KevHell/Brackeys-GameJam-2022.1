using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonSound : MonoBehaviour
{
    [SerializeField] private AudioClip _clip;

    public void PlayButtonClick()
    {
        GameManager.Instance.AudioController.PlaySoundEffect(_clip);
    }
}
