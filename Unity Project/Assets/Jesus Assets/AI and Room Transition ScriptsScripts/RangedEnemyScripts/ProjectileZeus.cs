using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileZeus : MonoBehaviour
{
    public float speed = 10f;      // Projectile speed
    public float damage = 10f;     // Damage dealt by the projectile
    public float lifespan = 3f;    // How long the projectile lasts

    private Transform target;      // Target for the projectile
    private Vector2 direction;     // Direction to travel

    void Start()
    {
        // Destroy after a set lifespan
        Destroy(gameObject, lifespan);
    }

    void Update()
    {
        if (target == null)
        {
            Destroy(gameObject); // Destroy if no target
            return;
        }

        // Calculate and move toward the target
        direction = (target.position - transform.position).normalized;
        transform.Translate(direction * speed * Time.deltaTime);
    }

    public void Initialize(Transform newTarget, float newDamage)
    {
        target = newTarget;   // Set the target to the specified enemy
        damage = newDamage;   // Set the projectile's damage
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) return; // Ignore collisions with the player

        if (other.CompareTag("Enemy"))
        {
            // Apply damage to the enemy
            HealthSystem enemyHealth = other.GetComponent<HealthSystem>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(damage);
            }

            // Destroy the projectile on impact
            Destroy(gameObject);
        }
    }
}
