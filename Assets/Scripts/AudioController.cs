using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioController : MonoBehaviour
{
    [Header("Source References")]
    [SerializeField] private AudioSource _backgroundMusicSource;
    [SerializeField] private List<AudioSource> _effectSources;

    [SerializeField] private AudioMixer _masterMixer;

    private void Start()
    {
        _masterMixer = _backgroundMusicSource.outputAudioMixerGroup.audioMixer;
    }
    
    public void PlaySoundEffect(AudioClip clip, bool force = false)
    {
        bool soundPlayed = false;
        
        foreach (var effectSource in _effectSources)
        {
            if (!effectSource.isPlaying)
            {
                effectSource.clip = clip;
                effectSource.Play();
                soundPlayed = true;
                break;
            }
        }

        if (!soundPlayed && force)
        {
            _effectSources[0].clip = clip;
            _effectSources[0].Play();
        }
    }

    #region Toggle Mute

    
    public void ToggleBackgroundMusic()
    {
        _backgroundMusicSource.mute = !_backgroundMusicSource.mute;
    }
    public void ToggleSoundEffects()
    {
        foreach (var effectSource in _effectSources)
        {
            effectSource.mute = !effectSource.mute;
        }
    }

    #endregion

    #region Adjust Volume

    public void AdjustMasterVolume(float value)
    {
        _masterMixer.SetFloat("masterVolume", value);
    }

    public void AdjustMusicVolume(float value)
    {
        _masterMixer.SetFloat("musicVolume", value);
    }
    
    public void AdjustEffectVolume(float value)
    {
        _masterMixer.SetFloat("effectVolume", value);
    }
    

    #endregion

    #region Return Volume Values

    public void GetMasterVolume(out float value)
    {
        _masterMixer.GetFloat("masterVolume", out value);
    }
    
    public void GetMusicVolume(out float value)
    {
        _masterMixer.GetFloat("musicVolume", out value);
    }
    
    public void GetEffectVolume(out float value)
    {
        _masterMixer.GetFloat("effectVolume", out value);
    }

    #endregion
}
