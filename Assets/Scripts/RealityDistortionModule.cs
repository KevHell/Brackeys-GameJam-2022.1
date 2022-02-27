using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public class RealityDistortionModule : MonoBehaviour
{
    public bool Active { get; private set; }
    private bool _loading;

    [Header("References")]
    [SerializeField] private Camera _irrealityCamera;
    [SerializeField] private Transform _rdmTransform;
    [SerializeField] private Transform _rdmParticleTransform;
    
    [Header("Energy Settings")]
    public float _maxEnergyInSeconds;
    private float _energySecondsLeft;
    private float _currentEnergyRatio;
    public float EnergySecondsLeft
    {
        get { return _energySecondsLeft; }
    }
    // Energy Alert
    [SerializeField] [Range(0, 1)] private float _alertValue1;
    [SerializeField] [Range(0, 1)] private float _alertValue2;
    [SerializeField] private Gradient _alertGradient;
    [SerializeField] private float _flickerDurationAlert1 = 1;
    [SerializeField] private float _flickerDurationAlert2 = 3;
    private float _flickerTimer;
    private bool _flickering1;
    private bool _flickering2;

    [Header("Scaling Settings")]
    [SerializeField] private float _scaleUpTime;
    [SerializeField] private AudioSource _badMusic;
    [SerializeField] private AudioSource _goodMusic;
    private float _desiredMusicVolume;
    private float _scaleTimer;
    private float _ratio;
    private Vector3 _desiredRDMScale;
    private Vector3 _desiredParticleScale;
    private float _desiredCameraSize;
    private bool _scaleUp;
    private bool _scaleDown;
    
    [Header("Loading Settings")]
    [SerializeField] private float _loadingTime;
    private float _loadingAmount;
    private float _loadingRatio;

    public UnityEvent OnActivation = new UnityEvent();
    public UnityEvent OnAlert = new UnityEvent();

    [SerializeField] private AudioClip _loadingClip;

    private void Start()
    {
        _energySecondsLeft = _maxEnergyInSeconds;
        _currentEnergyRatio = _energySecondsLeft / _maxEnergyInSeconds;
        GameManager.Instance.MainGameUIController.
            UpdateRDMEnergy(_currentEnergyRatio, _alertGradient.Evaluate(_currentEnergyRatio));

        _desiredRDMScale = _rdmTransform.localScale;
        _desiredCameraSize = _irrealityCamera.orthographicSize;
        _desiredParticleScale = _rdmParticleTransform.localScale;
        _desiredMusicVolume = _badMusic.volume;
        
        _rdmTransform.localScale = Vector3.zero;
        _irrealityCamera.orthographicSize = 0;
        _rdmParticleTransform.localScale = Vector3.zero;
        //_goodMusic.volume = 0;
    }

    private void Update()
    {
        
        if (_scaleUp)
        {
            _scaleTimer += Time.deltaTime;
            _ratio = _scaleTimer / _scaleUpTime;

            _rdmTransform.localScale = Vector3.Lerp(Vector3.zero, _desiredRDMScale, _ratio);
            _irrealityCamera.orthographicSize = Mathf.Lerp(0, _desiredCameraSize, _ratio);
            _rdmParticleTransform.localScale = Vector3.Lerp(Vector3.zero, _desiredParticleScale, _ratio);
            //_goodMusic.volume = Mathf.Lerp(0, _desiredMusicVolume, _ratio);
            //_badMusic.volume = Mathf.Lerp(_desiredMusicVolume, 0, _ratio);

            if (_ratio >= 1)
            {
                _scaleUp = false;
                _scaleTimer = 0;
                OnActivation.Invoke();
            }
        }

        if (_scaleDown)
        {
            _scaleTimer += Time.deltaTime;
            _ratio = _scaleTimer / _scaleUpTime;
            
            _rdmTransform.localScale = Vector3.Lerp(_desiredRDMScale, Vector3.zero, _ratio);
            _irrealityCamera.orthographicSize = Mathf.Lerp(_desiredCameraSize, 0, _ratio);
            _rdmParticleTransform.localScale = Vector3.Lerp(_desiredParticleScale, Vector3.zero, _ratio);
            //_badMusic.volume = Mathf.Lerp(0, _desiredMusicVolume, _ratio);
            //_goodMusic.volume = Mathf.Lerp(_desiredMusicVolume, 0, _ratio);

            if (_ratio >= 1)
            {
                _scaleDown = false;
                _scaleTimer = 0;
                
                Deactivate();
            }
        }
        
        if (Active)
        {
            _energySecondsLeft -= Time.deltaTime;
            if (_energySecondsLeft <= 0)
            {
                _energySecondsLeft = 0;
                _scaleDown = false;
                _scaleTimer = 0;
                Deactivate();
                Active = false;
            }
            
            _currentEnergyRatio = _energySecondsLeft / _maxEnergyInSeconds;
            
            
            HandleEnergyAlert();
            
            
            // Update UI
            GameManager.Instance.MainGameUIController.
                UpdateRDMEnergy(_currentEnergyRatio, _alertGradient.Evaluate(_currentEnergyRatio));
        }

        if (_loading)
        {
            _loadingAmount += Time.deltaTime;
            _loadingRatio = _loadingAmount / _loadingTime;
            
            // Update UI when reached alert 1
            
            
            // Update UI when reached alert 2

            if (_loadingRatio > 1)
            {
                _loadingRatio = 1;
                _loadingAmount = 0;
                _loading = false;
            }
            
            _energySecondsLeft = _loadingRatio * _maxEnergyInSeconds;
            
            // Update UI
            _currentEnergyRatio = _energySecondsLeft / _maxEnergyInSeconds;
            GameManager.Instance.MainGameUIController.
                UpdateRDMEnergy(_currentEnergyRatio, _alertGradient.Evaluate(_currentEnergyRatio));
        }
    }


    #region Activation Management

    public void ToggleModule()
    {
        if (Active)
        {
            _scaleDown = true;
            Active = false;
        }
        else Activate();
    }
    private void Activate()
    {
        if (_loading) return;
        
        Active = true;
        GameManager.Instance.WorldChangeManager.ChangeToIrreality();
        GameManager.Instance.AudioController.ActivateGoodMusic();

        _scaleUp = true;
        //_scaleTimer = 0;
        _scaleDown = false;
    }
    private void Deactivate()
    {
        //Active = false;
        StopCoroutine(nameof(FlickerWorldsAfterRandomSeconds));
        StopCoroutine(nameof(FlickerWorldsQuicklyAfterRandomSeconds));
        GameManager.Instance.WorldChangeManager.ChangeToReality();
        GameManager.Instance.AudioController.ActivateBadMusic();

        _scaleUp = false;
        //_scaleTimer = 0;
        _scaleDown = true;
    }

    #endregion
    
    #region Energy Alert

    private void HandleEnergyAlert()
    {
        if (_currentEnergyRatio <= _alertValue1 && !_flickering1)
            {
                _flickering1 = true;
                StartCoroutine(nameof(FlickerWorldsAfterRandomSeconds));
                OnAlert.Invoke();
            }
            else if (_flickering1)
            {
                _flickerTimer += Time.deltaTime;
                
                if (_flickerTimer >= _flickerDurationAlert1)
                {
                    StopCoroutine(nameof(FlickerWorldsAfterRandomSeconds));
                    GameManager.Instance.WorldChangeManager.ChangeToIrreality();
                    _flickerTimer = 0.0f;
                }
            }
            
            
            if (_currentEnergyRatio <= _alertValue2 && !_flickering2)
            {
                _flickering2 = true;
                StartCoroutine(nameof(FlickerWorldsQuicklyAfterRandomSeconds));
                _flickerTimer = 0;
            }
            else if (_flickering2)
            {
                _flickerTimer += Time.deltaTime;
                
                if (_flickerTimer >= _flickerDurationAlert2)
                {
                    StopCoroutine(nameof(FlickerWorldsQuicklyAfterRandomSeconds));
                    _flickerTimer = 0.0f;
                }
            }
    }
    private IEnumerator FlickerWorldsAfterRandomSeconds()
    {
        GameManager.Instance.WorldChangeManager.ChangeToReality();
        float rdm = Random.Range(0.2f, 0.5f);
        yield return new WaitForSeconds(rdm);
        
        GameManager.Instance.WorldChangeManager.ChangeToIrreality();
        rdm = Random.Range(0.2f, _flickerDurationAlert1 * 0.75f);
        yield return new WaitForSeconds(rdm);

        StartCoroutine(nameof(FlickerWorldsAfterRandomSeconds));
    }
    private IEnumerator FlickerWorldsQuicklyAfterRandomSeconds()
    {
        GameManager.Instance.WorldChangeManager.ChangeToReality();
        float rdm = Random.Range(0.1f, 0.3f);
        yield return new WaitForSeconds(rdm);
        
        GameManager.Instance.WorldChangeManager.ChangeToIrreality();
        rdm = Random.Range(0.1f, 0.5f);
        yield return new WaitForSeconds(rdm);

        StartCoroutine(nameof(FlickerWorldsQuicklyAfterRandomSeconds));
    }
    
    #endregion

    #region Loading

    public void StartLoadingOverTime()
    {
        _loadingAmount = _currentEnergyRatio * _loadingTime;
        _loading = true;
        _flickering1 = false;
        _flickering2 = false;
        _flickerTimer = 0;
        
        StopCoroutine(nameof(FlickerWorldsAfterRandomSeconds));
        StopCoroutine(nameof(FlickerWorldsQuicklyAfterRandomSeconds));
        
        GameManager.Instance.AudioController.PlaySoundEffect(_loadingClip);
    }

    public void LoadInstantly(float amount)
    {
        _energySecondsLeft += amount;
        if (_energySecondsLeft > _maxEnergyInSeconds) _energySecondsLeft = _maxEnergyInSeconds;
        
        _flickering1 = false;
        _flickering2 = false;
        _flickerTimer = 0;
        
        _currentEnergyRatio = _energySecondsLeft / _maxEnergyInSeconds;
        
        GameManager.Instance.MainGameUIController.
            UpdateRDMEnergy(_currentEnergyRatio, _alertGradient.Evaluate(_currentEnergyRatio));
        
        StopCoroutine(nameof(FlickerWorldsAfterRandomSeconds));
        StopCoroutine(nameof(FlickerWorldsQuicklyAfterRandomSeconds));
        
        GameManager.Instance.AudioController.PlaySoundEffect(_loadingClip);
    }

    #endregion
    
    
}
