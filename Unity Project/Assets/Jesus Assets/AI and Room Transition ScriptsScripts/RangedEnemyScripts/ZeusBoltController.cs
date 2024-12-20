using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZeusBoltController : MonoBehaviour
{
    public GameObject projectilePrefab; // Zeus Bolt prefab
    public float targetingRadius = 10f; // Radius for finding enemies
    public float fireCooldown = 3f;    // Time between shots
    private float nextFireTime = 0f;   // Tracks when the next shot is allowed

    private Transform currentTarget;  // The currently targeted enemy

    public void EquipZeusBolt()
    {
        Debug.Log("Zeus Bolt equipped!");
    }

    public void UnequipZeusBolt()
    {
        Debug.Log("Zeus Bolt unequipped!");
        currentTarget = null; // Clear current target
    }

    void Update()
    {
        if (Time.time >= nextFireTime)
        {
            FireAtClosestEnemy();
        }
    }

    void FireAtClosestEnemy()
    {
        // Find the closest enemy
        if (currentTarget == null)
        {
            currentTarget = FindClosestEnemy();
        }

        if (currentTarget == null) return; // No target, don't shoot

        // Instantiate the projectile at the player's position
        GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);

        // Calculate direction to the target
        Vector2 directionToTarget = (currentTarget.position - transform.position).normalized;

        // Set projectile direction and damage
        ProjectileScript projectileScript = projectile.GetComponent<ProjectileScript>();
        if (projectileScript != null)
        {
            projectileScript.SetDirection(directionToTarget);
            projectileScript.SetDamage(StatsManager.Instance.damage);
        }

        // Update the cooldown
        nextFireTime = Time.time + fireCooldown;

        // Reset the target if it dies
        if (currentTarget.GetComponent<HealthSystem>()?.currentHealth <= 0)
        {
            currentTarget = null;
        }
    }

    Transform FindClosestEnemy()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, targetingRadius);
        float closestDistance = float.MaxValue;
        Transform closestEnemy = null;

        foreach (var hit in hits)
        {
            if (hit.CompareTag("Enemy"))
            {
                float distance = Vector2.Distance(transform.position, hit.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestEnemy = hit.transform;
                }
            }
        }

        return closestEnemy;
    }
}
