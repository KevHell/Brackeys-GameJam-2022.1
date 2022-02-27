using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Serialization;

public class AudioController : MonoBehaviour
{
    [FormerlySerializedAs("_backgroundMusicSource")]
    [Header("Source References")]
    [SerializeField] private AudioSource _badBackgroundMusicSource;
    [SerializeField] private AudioSource _goodBackgroundMusicSource;
    [SerializeField] private List<AudioSource> _effectSources;
    [SerializeField] private AudioSource _textBoxSource;
    [SerializeField] private AudioSource _playerWalkSource;

    [SerializeField] private AudioMixer _masterMixer;
    private bool _musicMuted;

    public bool WalkSoundPlaying;

    private void Start()
    {
        _masterMixer = _badBackgroundMusicSource.outputAudioMixerGroup.audioMixer;
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

    public void ActivateGoodMusic()
    {
        _badBackgroundMusicSource.mute = true;
        _goodBackgroundMusicSource.mute = false;
    }

    public void ActivateBadMusic()
    {
        _badBackgroundMusicSource.mute = false;
        _goodBackgroundMusicSource.mute = true;
    }

    public void ToggleBackgroundMusic()
    {
        _musicMuted = !_musicMuted;

        if (_musicMuted)
        {
            _badBackgroundMusicSource.mute = true;
            _goodBackgroundMusicSource.mute = true;
        }
        else
        {
            if (GameManager.Instance.RealityDistortionModule.Active)
            {
                _badBackgroundMusicSource.mute = true;
                _goodBackgroundMusicSource.mute = false;
            }
            else
            {
                _badBackgroundMusicSource.mute = false;
                _goodBackgroundMusicSource.mute = true;
            }
        }
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

    public void PlayTextBoxSound()
    {
        _textBoxSource.mute = false;
    }
    public void StopTextBoxSound()
    {
        _textBoxSource.mute = true;
    }
    
    public void PlayWalkSound()
    {
        _playerWalkSource.mute = false;
        WalkSoundPlaying = true;
    }
    public void StopWalkSound()
    {
        _playerWalkSource.mute = true;
        WalkSoundPlaying = false;
    }
}
