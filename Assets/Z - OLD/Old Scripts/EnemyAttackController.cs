using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackController : MonoBehaviour
{
    public Transform DefendCircle; // The player's defend circle transform
    public float speed = 0.3f; // The force applied to the enemy
    public float boostSpeed = 0.9f; // The force applied to the enemy when boosting
    public float boostDistance = 2f; // The distance at which the enemy starts to boost
    public float boostCost = 10f; // The stamina cost of boosting
    public Enemy enemy;

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        enemy.CurrentStamina = enemy.Stamina; // Set current stamina to max stamina
    }

    void Update()
    {
        // Calculate the direction towards the player
        Vector2 direction = (DefendCircle.position - transform.position).normalized;

        // Calculate the distance to the defend circle
        float distance = Vector2.Distance(transform.position, DefendCircle.position);

        // If the enemy is close to the player, boost
        if (distance < boostDistance && enemy.CurrentStamina > boostCost)
        {
            rb.AddForce(direction * boostSpeed);
            enemy.CurrentStamina -= boostCost * Time.deltaTime; // Decrease stamina
        }
        else
        {
            rb.AddForce(direction * speed);
        }

        // Regenerate stamina
        if (rb.velocity.magnitude < 0.01f) // If the enemy is standing still
        {
            enemy.CurrentStamina = Mathf.Min(enemy.CurrentStamina + Time.deltaTime * 0.5f, enemy.Stamina); // Regenerate stamina faster
        }
        else
        {
            enemy.CurrentStamina = Mathf.Min(enemy.CurrentStamina + Time.deltaTime * 0.1f, enemy.Stamina); // Normal stamina regeneration
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
