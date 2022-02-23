using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour
{
    void Update()
    {
        #region Movement

        float horizontalMovement = Input.GetAxisRaw("Horizontal");
        float verticalMovement = Input.GetAxisRaw("Vertical");
        PlayerController.Instance.PlayerMovement.SetDirection(horizontalMovement, verticalMovement);
        
        
        #endregion

        #region Drone

        if (Input.GetMouseButtonUp(0)) return; // Shoot
        if (Input.GetMouseButtonUp(2)) return; // Toggle RDM

        #endregion

        #region DEBUG

        if (Input.GetKeyDown(KeyCode.L)) return; // Load

        #endregion

        if (Input.GetKeyDown(KeyCode.E))
        {
            PlayerController.Instance.InteractableController.TryToInteract();
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
}
