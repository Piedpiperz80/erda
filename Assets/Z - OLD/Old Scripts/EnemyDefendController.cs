using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDefendController : MonoBehaviour
{
    public Transform AttackCircle; // The player's attack circle transform
    public float speed = 0.3f; // The force applied to the enemy
    public float boostSpeed = 0.9f; // The force applied to the enemy when boosting
    public float boostDistance = 2f; // The distance at which the enemy starts to boost
    public float boostCost = 10f; // The stamina cost of boosting
    public float wallAvoidanceDistance = 1.0f; // The distance at which the enemy starts to avoid the walls
    public Enemy enemy;

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        enemy.CurrentStamina = enemy.Stamina; // Set current stamina to max stamina
    }

    void Update()
    {
        // Calculate the direction away from the defend circle
        Vector2 direction = (transform.position - AttackCircle.position).normalized;

        // Calculate the distance to the player
        float distance = Vector2.Distance(transform.position, AttackCircle.position);

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

        // Avoid walls
        if (Mathf.Abs(position.x) > screenWidth - wallAvoidanceDistance)
        {
            float wallAvoidanceForce = (screenWidth - Mathf.Abs(position.x)) / wallAvoidanceDistance;
            rb.AddForce(new Vector2(-Mathf.Sign(position.x) * wallAvoidanceForce, 0));
        }
        if (Mathf.Abs(position.y) > screenHeight - wallAvoidanceDistance)
        {
            float wallAvoidanceForce = (screenHeight - Mathf.Abs(position.y)) / wallAvoidanceDistance;
            rb.AddForce(new Vector2(0, -Mathf.Sign(position.y) * wallAvoidanceForce));
        }
    }
}
