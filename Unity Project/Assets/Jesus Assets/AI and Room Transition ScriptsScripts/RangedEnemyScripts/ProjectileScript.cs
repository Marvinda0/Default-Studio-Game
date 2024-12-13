using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 10f;             // Speed of the projectile
    public float lifespan = 3f;           // Time in seconds before the projectile is destroyed
    private float damage = 10;              // Damage inflicted on the player, can be set by enemy

    private Vector2 direction;            // Direction to travel

    void Start()
    {
        // Destroy the projectile after a set lifespan to prevent it from persisting indefinitely
        Destroy(gameObject, lifespan);
    }

    void Update()
    {
        // Move the projectile in the specified direction at the given speed
        transform.Translate(direction * speed * Time.deltaTime);
    }

    // Method to set the direction for the projectile
    public void SetDirection(Vector2 newDirection)
    {
        direction = newDirection.normalized;
    }

    // Method to set the damage of the projectile, which can be called by the enemy script
    public void SetDamage(float newDamage)
    {
        damage = newDamage;
    }

    // Detect when the projectile enters a trigger collider
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Check if the collided object has a HealthSystem and apply damage
            HealthSystem playerHealth = other.GetComponent<HealthSystem>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
            }

            // Destroy the projectile upon hitting the player
            Destroy(gameObject);
        }
    }
}
