using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileZeus : MonoBehaviour
{
    public float speed = 10f;      // Speed of the projectile
    public float damage = 10f;     // Damage dealt by the projectile
    public float lifespan = 3f;    // Time before the projectile is destroyed

    private Transform target;      // The target to hit

    public void Initialize(Transform newTarget, float newDamage)
    {
        target = newTarget;
        damage = newDamage;
        Destroy(gameObject, lifespan); // Destroy after lifespan
    }

    void Update()
    {
        if (target == null)
        {
            Destroy(gameObject); // Destroy if target is lost
            return;
        }

        // Move toward the target
        Vector2 direction = (target.position - transform.position).normalized;
        transform.Translate(direction * speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            HealthSystem enemyHealth = other.GetComponent<HealthSystem>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(damage);
            }
            Destroy(gameObject); // Destroy on impact
        }
    }
}
