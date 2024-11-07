using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIChase : MonoBehaviour
{
    public GameObject player;           // Reference to the player GameObject
    public float speed;                  // Speed of the AI enemy
    public float collissionOffset = 0.01f; // Offset to prevent tunneling
    public ContactFilter2D movementFilter; // Contact filter for checking collisions

    private float distance;              // Distance to the player
    private Rigidbody2D rb;              // Rigidbody2D component of the AI
    private List<RaycastHit2D> castCollisions = new List<RaycastHit2D>(); // Store raycast hits

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>(); // Get the Rigidbody2D component
        movementFilter.SetLayerMask(~LayerMask.GetMask("Player"));
    }

    // Update is called once per frame
    void Update()
    {
        distance = Vector2.Distance(transform.position, player.transform.position);
        if (distance > 0.01f) { }// if too close stop so platyer can escape, Placeholder
        
            ChasePlayer();
        
    }

    void ChasePlayer()
    {
        Vector2 direction = (player.transform.position - transform.position).normalized; // Calculate direction to the player
        // Check if the path to the player is clear
        if (CanMove(direction))
        {
            // Move the AI towards the player
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
        }
    }

    private bool CanMove(Vector2 direction)
    {
        // Cast a ray in the desired direction to check for collisions
        int count = rb.Cast(direction, movementFilter, castCollisions, speed * Time.fixedDeltaTime + collissionOffset);

        // Check if the cast hits anything other than the player
        for (int i = 0; i < count; i++)
        {
            // If the hit collider is not the player's collider, then return false
            if (castCollisions[i].collider.gameObject != player)
            {
                return false;
            }
        }

        return true; // Return true if no collisions with obstacles other than the player detected
    }
}
