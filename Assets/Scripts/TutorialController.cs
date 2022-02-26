using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialController : MonoBehaviour
{
    [SerializeField] private string _greetText;
    [SerializeField] private string _rdmText;
    [SerializeField] private string _collectExplainText;
    [SerializeField] private string _craftText;
    [SerializeField] private string _plantingText;
    [SerializeField] private string _shootText;
    [SerializeField] private string _endText;
    [SerializeField] private string _loadText;

    [SerializeField] private Item _tutorialItem;
    [SerializeField] private PlantSpotController _tutorialPlantSpot;

    private void Start()
    {
        if (GameManager.Instance.Tutorial)
        {
            StartCoroutine(nameof(GreetAfterSeconds));
            GameManager.Instance.RealityDistortionModule.OnAlert.AddListener(ExplainRDMLoad);
        }
    }

    private IEnumerator GreetAfterSeconds()
    {
        yield return new WaitForSeconds(1);
        GreetPlayer();
    }

    private void GreetPlayer()
    {
        if (!GameManager.Instance.Tutorial) return;
        
        GameManager.Instance.MainGameUIController.DisplayTextInTextBox(_greetText);
        GameManager.Instance.MainGameUIController.OnBoxClosed.AddListener(ExplainRDM);
    }
    
    private void ExplainRDM()
    {
        GameManager.Instance.MainGameUIController.OnBoxClosed.RemoveListener(ExplainRDM);
        if (!GameManager.Instance.Tutorial) return;
        
        GameManager.Instance.MainGameUIController.DisplayTextInTextBox(_rdmText);
        GameManager.Instance.RealityDistortionModule.OnActivation.AddListener(ExplainCollect);
    }

    private void ExplainCollect()
    {
        GameManager.Instance.RealityDistortionModule.OnActivation.RemoveListener(ExplainCollect);
        if (!GameManager.Instance.Tutorial) return;
        
        GameManager.Instance.MainGameUIController.DisplayTextInTextBox(_collectExplainText);
        _tutorialItem.OnCollect.AddListener(ExplainCraft);
    }

    private void ExplainCraft()
    {
        _tutorialItem.OnCollect.RemoveListener(ExplainCraft);
        if (!GameManager.Instance.Tutorial) return;
        
        GameManager.Instance.MainGameUIController.DisplayTextInTextBox(_craftText);
        GameManager.Instance.CraftingController.OnCrafted.AddListener(ExplainPlanting);
    }

    private void ExplainPlanting()
    {
        GameManager.Instance.CraftingController.OnCrafted.RemoveListener(ExplainPlanting);
        if (!GameManager.Instance.Tutorial) return;
        
        GameManager.Instance.InputController.SwitchInputMode(InputMode.TextBox);
        
        GameManager.Instance.MainGameUIController.DisplayTextInTextBox(_plantingText);
        _tutorialPlantSpot.OnPlanted.AddListener(ExplainShooting);
    }

    private void ExplainShooting()
    {
        _tutorialPlantSpot.OnPlanted.RemoveListener(ExplainShooting);
        if (!GameManager.Instance.Tutorial) return;
        
        GameManager.Instance.InputController.SwitchInputMode(InputMode.TextBox);
        
        GameManager.Instance.MainGameUIController.DisplayTextInTextBox(_shootText);
        PlayerController.Instance.StunGun.OnShot.AddListener(EndTutorial);
    }

    private void EndTutorial()
    {
        PlayerController.Instance.StunGun.OnShot.RemoveListener(EndTutorial);
        GameManager.Instance.MainGameUIController.DisplayTextInTextBox(_endText);
    }

    private void ExplainRDMLoad()
    {
        GameManager.Instance.RealityDistortionModule.OnAlert.RemoveListener(ExplainRDMLoad);
        GameManager.Instance.MainGameUIController.DisplayTextInTextBox(_loadText);
    }
}
