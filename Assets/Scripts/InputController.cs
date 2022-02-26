using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour
{
    private bool _waitingForInteraction;
    private float _interactionTimer;
    private Interactable _interactable;

    private InputMode _inputMode;

    void Update()
    {
        switch (_inputMode)
        {
            case InputMode.MainGame:

                #region Movement

                float horizontalMovement = Input.GetAxisRaw("Horizontal");
                float verticalMovement = Input.GetAxisRaw("Vertical");
                PlayerController.Instance.PlayerMovement.SetDirection(horizontalMovement, verticalMovement);

                #endregion

                #region Drone

                if (Input.GetMouseButtonUp(0)) PlayerController.Instance.StunGun.Shoot(); // Shoot
                if (Input.GetMouseButtonUp(1) && !GameManager.Instance.GamePaused)
                    GameManager.Instance.RealityDistortionModule.ToggleModule(); // Toggle RDM

                #endregion

                #region DEBUG

                if (Input.GetKeyDown(KeyCode.L)) return; // Load

                if (Input.GetKeyDown(KeyCode.Space)) PlayerController.Instance.HealthController.DecreaseHealth(1);

                if (Input.GetKeyDown(KeyCode.T))
                    GameManager.Instance.MainGameUIController.DisplayTextInTextBox(
                        "Seems like everything works just perfectly!");

                #endregion

                #region Interaction

                if (Input.GetKeyDown(KeyCode.E))
                {
                    _interactable = PlayerController.Instance.InteractableController.CurrentInteractable;
                    if (!_interactable) return;

                    if (_interactable.GetComponent<Item>())
                    {
                        if (!PlayerController.Instance.ItemBag.SpaceInBag)
                        {
                            GameManager.Instance.MainGameUIController.DisplayTextInTextBox("I'm afraid there's no more space in your bag...");
                            return;
                        }
                    }


                    _interactionTimer = 0;

                    if (Mathf.Approximately(_interactable.InteractDuration, 0))
                    {
                        PlayerController.Instance.InteractableController.TryToInteract();
                    }
                    else
                    {
                        _waitingForInteraction = true;
                        _interactable.InteractionIndicator.SetActive(true);
                    }
                }
                else if (Input.GetKeyUp(KeyCode.E))
                {
                    _waitingForInteraction = false;

                    if (_interactable)
                    {
                        _interactable.InteractionIndicator.SetActive(false);
                    }
                }

                if (_waitingForInteraction)
                {
                    _interactionTimer += Time.deltaTime;
                    _interactable.InteractionIndicatorFill.fillAmount =
                        _interactionTimer / _interactable.InteractDuration;

                    if (_interactionTimer >= _interactable.InteractDuration)
                    {
                        PlayerController.Instance.InteractableController.TryToInteract();

                        if (_interactable)
                        {
                            _interactable.InteractionIndicator.SetActive(false);
                        }
                    }
                }

                #endregion


                break;
            
            
            case InputMode.TextBox:

                if (Input.GetKeyDown(KeyCode.E)) GameManager.Instance.MainGameUIController.CloseTextBox();
                
                break;
        }


        if (Input.GetKeyDown(KeyCode.Escape))
        {
            /*
            if (CustomSceneManager.MainMenuIsActive())
            {
                _mainMenuUIController.CloseActivePanel();
                _mainMenuUIController.ShowQuitPanel();
            }
            else
            {
                PauseGame();
                MainGameUIController.ShowPausePanel();
            }
            */
        }
    }

    public void SwitchInputMode(InputMode newMode)
    {
        _inputMode = newMode;
    }
}

public enum InputMode
{
    MainGame,
    Menu,
    TextBox
}