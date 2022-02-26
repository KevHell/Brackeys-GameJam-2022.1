using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] [Range(0, 1)] private float _verticalSlowDown; 
    private Vector2 _movementDirection;
    [SerializeField] private Animator _badAnimator;
    [SerializeField] private Animator _goodAnimator;
    [SerializeField] private Animator _badDroneAnimator;
    [SerializeField] private Animator _goodDroneAnimator;
    
    private void Update()
    {
        Vector3 movement = _movementDirection * (_speed * Time.deltaTime);
        transform.position += movement;
    }

    public void SetDirection(float x, float y)
    {
        if (x == 1 && y == 1)
        {
            x = _verticalSlowDown;
            y = _verticalSlowDown;
        }
        else if (x == -1 && y == -1)
        {
            x = -_verticalSlowDown;
            y = -_verticalSlowDown;
        }
        else if (x == 1 && y == -1)
        {
            x = _verticalSlowDown;
            y = -_verticalSlowDown;
        }
        else if (x == -1 && y == 1)
        {
            x = -_verticalSlowDown;
            y = _verticalSlowDown;
        }
        
        _movementDirection = new Vector2(x, y);
        
        float horizontalMovement = Input.GetAxisRaw("Horizontal");
        float verticalMovement = Input.GetAxisRaw("Vertical");
        
        _badAnimator.SetInteger("MovementX", (int)horizontalMovement);
        _badAnimator.SetInteger("MovementY", (int)verticalMovement);
        
        _goodAnimator.SetInteger("MovementX", (int)horizontalMovement);
        _goodAnimator.SetInteger("MovementY", (int)verticalMovement);
        
        _badDroneAnimator.SetInteger("MovementX", (int)horizontalMovement);
        _goodDroneAnimator.SetInteger("MovementX", (int)horizontalMovement);
    }
}
