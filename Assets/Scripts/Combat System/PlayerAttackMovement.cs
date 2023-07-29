using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackMovement : Movement
{
    protected override void Start()
    {
        base.Start(); // Call the base class's Start method
    }

    protected override void Update()
    {
        base.Update();

        float moveHorizontal = Input.GetKey(KeyCode.Quote) ? 1 : Input.GetKey(KeyCode.L) ? -1 : 0;
        float moveVertical = Input.GetKey(KeyCode.P) ? 1 : Input.GetKey(KeyCode.Semicolon) ? -1 : 0;

        Vector2 movement = new Vector2(moveHorizontal, moveVertical);

        // If the player is holding sprint key and has enough stamina, sprint
        if (Input.GetKey(KeyCode.RightShift) && character.CurrentStamina > sprintCost)
        {
            sprinting(movement);
        }
        else
        {
            Move(movement);
        }
    }
}
