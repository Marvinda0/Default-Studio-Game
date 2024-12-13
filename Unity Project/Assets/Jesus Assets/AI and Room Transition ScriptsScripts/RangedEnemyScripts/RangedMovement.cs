using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class RangedEnemyController : MonoBehaviour
{
    public GameObject projectilePrefab;     // Reference to the projectile prefab
    public float attackRange = 5f;          // Distance within which the enemy will stop and shoot
    public float followRange = 10f;         // Distance for following the player
    public float fireRate = 1f;             // Delay between shots
    public float projectileDamage = 10;       // Damage dealt by the projectile

    private AIPath aiPath;                  // AIPath component for movement
    private Transform playerTransform;      // Reference to the player's transform
    private float nextFireTime;             // Time for the next shot
    private AIDestinationSetter destinationSetter; // Reference to the destination setter

    void Start()
    {
        aiPath = GetComponent<AIPath>();
        destinationSetter = GetComponent<AIDestinationSetter>();

        // Find the player by tag and set as target
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
            destinationSetter.target = playerTransform; // Set the player as the target
        }
        else
        {
            Debug.LogWarning("Player not found. Make sure the player has the 'Player' tag.");
        }
    }

    void Update()
    {
        FollowOrAttack();
    }

    void FollowOrAttack()
    {
        if (playerTransform == null) return;

        float distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);

        if (distanceToPlayer <= attackRange)
        {
            aiPath.canMove = false; // Stop moving to attack
            if (Time.time >= nextFireTime)
            {
                FireProjectile();
                nextFireTime = Time.time + fireRate; // Set next shot time
            }
        }
        else if (distanceToPlayer <= followRange)
        {
            aiPath.canMove = true; // Follow player within follow range
        }
        else
        {
            aiPath.canMove = false; // Stop following if out of range
        }
    }

    void FireProjectile()
    {
        if (projectilePrefab == null) return;

        // Instantiate the projectile at the enemy's position
        GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);

        // Calculate direction to the player
        Vector2 directionToPlayer = (playerTransform.position - transform.position).normalized;

        // Set projectile direction and damage
        Projectile projectileScript = projectile.GetComponent<Projectile>();
        if (projectileScript != null)
        {
            float scaledDamage = projectileDamage * MobStatsManager.Instance.globalDamageMultiplier;
            projectileScript.SetDirection(directionToPlayer);
            projectileScript.SetDamage(scaledDamage);  // Set the desired damage amount for the projectile
        }
    }
}
