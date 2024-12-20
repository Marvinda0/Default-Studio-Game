using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZeusBoltController : MonoBehaviour
{
    public GameObject projectilePrefab; // Reference to the Zeus projectile prefab
    public float targetingRadius = 10f; // Radius for finding enemies
    public float fireCooldown = 3f;    // Time between shots

    private bool isEquipped = false;    // Tracks if Zeus Bolt is equipped
    private float nextFireTime = 0f;   // Time for the next allowed shot
    private Transform currentTarget;   // Current enemy target

    void OnEnable()
    {
        EquippedSlot.OnZeusThunderboltEquipped += HandleEquipState;
    }

    void OnDisable()
    {
        EquippedSlot.OnZeusThunderboltEquipped -= HandleEquipState;
    }

    private void HandleEquipState(bool equipped)
    {
        isEquipped = equipped;
        if (!isEquipped)
        {
            currentTarget = null; // Clear target when unequipped
        }
    }

    void Update()
    {
        if (!isEquipped) return;

        if (Time.time >= nextFireTime)
        {
            FireAtClosestEnemy();
        }
    }

    private void FireAtClosestEnemy()
    {
        currentTarget = FindClosestEnemy();
        if (currentTarget == null) return;

        // Spawn projectile at player's position
        Vector3 spawnPosition = transform.position;
        GameObject projectile = Instantiate(projectilePrefab, spawnPosition, Quaternion.identity);

        // Initialize the projectile with the target
        ProjectileZeus projectileScript = projectile.GetComponent<ProjectileZeus>();
        if (projectileScript != null)
        {
            projectileScript.Initialize(currentTarget, StatsManager.Instance.damage);
        }

        nextFireTime = Time.time + fireCooldown; // Update cooldown
    }

    private Transform FindClosestEnemy()
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
