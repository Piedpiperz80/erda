using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public float sprintSpeed = 0.9f;
    public float sprintCost = 10f; // The stamina cost of sprinting
    public Character character;

    protected Rigidbody2D rb;

    // Make speed a property that gets its value from the Speed skill of the character
    public float speed
    {
        get { return character.Speed/100; }
    }

    // Make speed a property that gets its value from the Speed skill of the character
    public float sprint
    {
        get { return character.Sprint / 100; }
    }

    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    protected virtual void Update()
    {
        // Regenerate stamina
        if (rb.velocity.magnitude < 0.01f) // If the character is standing still
        {
            character.CurrentStamina = Mathf.Min(character.CurrentStamina + Time.deltaTime * 0.5f, character.Stamina); // Regenerate stamina faster
        }
        else
        {
            character.CurrentStamina = Mathf.Min(character.CurrentStamina + Time.deltaTime * 0.1f, character.Stamina); // Normal stamina regeneration
        }

        // Limit position
        Vector3 position = transform.position;
        float screenWidth = Camera.main.aspect * Camera.main.orthographicSize;
        float screenHeight = Camera.main.orthographicSize;
        position.x = Mathf.Clamp(position.x, -screenWidth, screenWidth);
        position.y = Mathf.Clamp(position.y, -screenHeight, screenHeight);
        transform.position = position;
    }

    protected void Move(Vector2 direction)
    {
        rb.AddForce(direction * speed);
    }

    protected void sprinting(Vector2 direction)
    {
        if (character.CurrentStamina > sprintCost)
        {
            rb.AddForce(direction * (speed+sprint));
            character.CurrentStamina -= sprintCost * Time.deltaTime; // Decrease stamina
        }
    }
}
