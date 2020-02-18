using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : ArenaParticipant
{
    float movedownX = 0.0f;
    float movedownY = 0.0f;
    
    float sensitivityX = 1;
    float sensitivityY = 1;
    

    public override void Start()
    {
        base.Start();
    }
    public override void SetDirection()
    {
        TouchScreenMovement();
        MouseMovement();
    }

    void TouchScreenMovement()
    {
        if (Input.touchCount >= 1)
        {
            Vector2 touchDeltaPosition = Input.GetTouch(0).deltaPosition;
            curDir = new Vector3(touchDeltaPosition.x * -1, 0, touchDeltaPosition.y * -1);
        }
    }
    void MouseMovement()
    {
        if (Input.GetMouseButton(0))
        {
            movedownX += Input.GetAxis("Mouse X") * sensitivityX;
            movedownY += Input.GetAxis("Mouse Y") * sensitivityY;
            if (Input.GetAxis("Mouse Y") != 0 || Input.GetAxis("Mouse X") != 0)
            {
                curDir = (new Vector3(movedownX, 0, movedownY) * -1);
            }
            movedownX = 0.0f;
            movedownY = 0.0f;
        }
        else { movedownX = 0.0f; movedownY = 0.0f; }
    }
}
