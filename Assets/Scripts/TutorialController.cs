using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialController : MonoBehaviour
{
    [SerializeField] private string _greetText;
    [SerializeField] private string _rdmText;
    [SerializeField] private string _collectExplainText;

    [SerializeField] private Item _tutorialItem;

    private void GreetPlayer()
    {
        GameManager.Instance.MainGameUIController.DisplayTextInTextBox(_greetText);
        GameManager.Instance.MainGameUIController.OnBoxClosed.AddListener(ExplainRDM);
    }
    
    private void ExplainRDM()
    {
        GameManager.Instance.MainGameUIController.OnBoxClosed.RemoveListener(ExplainRDM);
        
    }

    private void ExplainCollect()
    {
        
        GameManager.Instance.MainGameUIController.DisplayTextInTextBox(_collectExplainText);
        //_tutorialItem.OnCollect.AddListener();
    }

    
}
