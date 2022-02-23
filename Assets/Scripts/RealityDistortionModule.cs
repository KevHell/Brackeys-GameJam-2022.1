using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class RealityDistortionModule : MonoBehaviour
{
    public bool Active { get; private set; }
    private bool _loading;

    [Header("Energy Settings")]
    [SerializeField] private float _maxEnergyInSeconds;
    private float _energySecondsLeft;
    private float _currentEnergyRatio;
    // Energy Alert
    [SerializeField] [Range(0, 1)] private float _alertValue1;
    [SerializeField] [Range(0, 1)] private float _alertValue2;
    [SerializeField] private Gradient _alertGradient;
    [SerializeField] private float _flickerDurationAlert1 = 1;
    [SerializeField] private float _flickerDurationAlert2 = 3;
    private float _flickerTimer;
    private bool _flickering1;
    private bool _flickering2;
    


    [Header("Loading Settings")]
    [SerializeField] private float _loadingTime;
    private float _loadingAmount;
    private float _loadingRatio;

    private void Start()
    {
        _energySecondsLeft = _maxEnergyInSeconds;
        _currentEnergyRatio = _energySecondsLeft / _maxEnergyInSeconds;
        GameManager.Instance.MainGameUIController.
            UpdateRDMEnergy(_currentEnergyRatio, _alertGradient.Evaluate(_currentEnergyRatio));
    }

    private void Update()
    {
        // DEBUG ////////////////////////////////
        if (Input.GetKeyDown(KeyCode.L) && !GameManager.Instance.GamePaused)
        {
            StartLoadingOverTime();
        }
        /////////////////////////////////////////
        
        
        if (Active)
        {
            _energySecondsLeft -= Time.deltaTime;
            if (_energySecondsLeft <= 0)
            {
                _energySecondsLeft = 0;
                Deactivate();
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
        if (Active) Deactivate();
        else Activate();
    }
    private void Activate()
    {
        if (_loading) return;
        
        Active = true;
        GameManager.Instance.WorldChangeManager.ChangeToIrreality();
    }
    private void Deactivate()
    {
        Active = false;
        StopCoroutine(nameof(FlickerWorldsAfterRandomSeconds));
        StopCoroutine(nameof(FlickerWorldsQuicklyAfterRandomSeconds));
        GameManager.Instance.WorldChangeManager.ChangeToReality();
    }

    #endregion
    
    #region Energy Alert

    private void HandleEnergyAlert()
    {
        if (_currentEnergyRatio <= _alertValue1 && !_flickering1)
            {
                _flickering1 = true;
                StartCoroutine(nameof(FlickerWorldsAfterRandomSeconds));
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
    }

    public void LoadInstantly(float amount)
    {
        _energySecondsLeft += amount;
        if (_energySecondsLeft > _maxEnergyInSeconds) _energySecondsLeft = _maxEnergyInSeconds;
        
        _flickering1 = false;
        _flickering2 = false;
        _flickerTimer = 0;
    }

    #endregion
    
    
}
