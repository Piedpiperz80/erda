using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDefendAI : Movement
{
    public Rigidbody2D playerRigidbody; // The player's Rigidbody2D component
    public float minimumDistance = 3f; // The minimum distance for the enemy to start moving
    public float wallAvoidanceDistance = 1.0f; // The distance at which the enemy starts to avoid the walls
    // might consider using the magnitude for reaction time? but could be exploited by creeping
    public Camera attackingCamera;

    private Vector2 randomDirection; // The random direction the enemy will move in

    protected override void Start()
    {
        base.Start(); // Call the base class's Start method
        // Set the initial position of the enemy randomly along the x-axis
        Vector3 startPosition = transform.position;
        startPosition.x = Random.Range(-4f, 4f); // Adjust the range as needed
        transform.position = startPosition;
    }

    protected override void Update()
    {
        base.Update();

        // Calculate the direction towards the player
        Vector2 directionToPlayer = (playerRigidbody.transform.position - transform.position).normalized;

        // Calculate the distance to the player
        float distanceToPlayer = Vector2.Distance(transform.position, playerRigidbody.transform.position);

        // Check if the player is moving and the enemy is within the minimum distance
        if (playerRigidbody.velocity.magnitude > 0f && distanceToPlayer < minimumDistance)
        {
            // Generate a random direction
            randomDirection = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;

            // If the random direction is towards the player, flip it to point away
            if (Vector2.Dot(randomDirection, directionToPlayer) > 0)
            {
                randomDirection = -randomDirection;
            }

            // Check if there is an obstacle ahead
            Vector3 position = transform.position;
            float screenWidth = attackingCamera.aspect * attackingCamera.orthographicSize;
            float screenHeight = attackingCamera.orthographicSize;
            Vector2 directionAwayFromWall = Vector2.zero;
            if (Mathf.Abs(position.x) > screenWidth - wallAvoidanceDistance)
            {
                directionAwayFromWall.x = -Mathf.Sign(position.x);
            }
            else if (Mathf.Abs(position.y) > screenHeight - wallAvoidanceDistance)
            {
                directionAwayFromWall.y = -Mathf.Sign(position.y);
            }

            // If the enemy is close to a wall, adjust the move away direction to move away from the wall
            if (directionAwayFromWall != Vector2.zero)
            {
                randomDirection += directionAwayFromWall.normalized;
            }

            // Move the enemy in the random direction
            Move(randomDirection.normalized);
        }
    }
}
