using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDefendController : MonoBehaviour
{
    public float speed = 0.3f;
    public float sprintSpeed = 0.9f;
    public float sprintCost = 10f; // The stamina cost of sprinting
    public Player player;


    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player.CurrentStamina = player.Stamina; // Set current stamina to max stamina
    }

    void Update()
    {
        float moveHorizontal = Input.GetKey(KeyCode.A) ? -1 : Input.GetKey(KeyCode.D) ? 1 : 0;
        float moveVertical = Input.GetKey(KeyCode.W) ? 1 : Input.GetKey(KeyCode.S) ? -1 : 0;

        Vector2 movement = new Vector2(moveHorizontal, moveVertical);

        // If the player is holding sprint key and has enough stamina, sprint
        if (Input.GetKey(KeyCode.LeftShift) && player.CurrentStamina > sprintCost)
        {
            rb.AddForce(movement * sprintSpeed);
            player.CurrentStamina -= sprintCost * Time.deltaTime; // Decrease stamina
        }
        else
        {
            rb.AddForce(movement * speed);
        }

        // Regenerate stamina
        if (moveHorizontal == 0 && moveVertical == 0) // If the player is standing still
        {
            player.CurrentStamina = Mathf.Min(player.CurrentStamina + Time.deltaTime * 0.5f, player.Stamina); // Regenerate stamina faster
        }
        else
        {
            player.CurrentStamina = Mathf.Min(player.CurrentStamina + Time.deltaTime * 0.1f, player.Stamina); // Normal stamina regeneration
        }

        // Limit position
        Vector3 position = transform.position;
        float screenWidth = Camera.main.aspect * Camera.main.orthographicSize;
        float screenHeight = Camera.main.orthographicSize;
        position.x = Mathf.Clamp(position.x, -screenWidth, screenWidth);
        position.y = Mathf.Clamp(position.y, -screenHeight, screenHeight);
        transform.position = position;
    }
}
