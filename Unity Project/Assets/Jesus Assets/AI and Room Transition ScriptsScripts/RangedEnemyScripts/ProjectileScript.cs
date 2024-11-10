using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 10f;             // Speed of the projectile
    public float lifespan = 3f;           // Time in seconds before the projectile is destroyed
    private int damage = 10;              // Damage inflicted on the player, can be set by enemy

    private Vector2 direction;            // Direction to travel

    void Start()
    {
        Destroy(gameObject, lifespan); // Destroy the projectile after its lifespan
    }

    void Update()
    {
        transform.Translate(direction * speed * Time.deltaTime);
    }

    // Sets the direction of the projectile
    public void SetDirection(Vector2 newDirection)
    {
        direction = newDirection.normalized;
    }

    // Sets the damage of the projectile, used by the enemy script
    public void SetDamage(int newDamage)
    {
        damage = newDamage;
    }

    // Handle collision with player
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Access player’s health system and apply damage
            HealthSystem playerHealth = other.GetComponent<HealthSystem>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
            }

            Destroy(gameObject); // Destroy projectile on hit
        }
    }
}
