using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] [Range(0, 1)] private float _verticalSlowDown; 
    private Vector2 _movementDirection;
    
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
    }
}
