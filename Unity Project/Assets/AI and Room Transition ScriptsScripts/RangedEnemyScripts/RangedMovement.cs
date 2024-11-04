using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class RangedEnemyController : MonoBehaviour
{
    public GameObject player;               // Reference to the player
    public GameObject projectilePrefab;     // Reference to the projectile prefab
    public float attackRange = 5f;          // Distance within which the enemy will stop and shoot
    public float followRange = 10f;         // Distance for following the player
    public float fireRate = 1f;             // Delay between shots

    private AIPath aiPath;                  // AIPath component for movement
    private float nextFireTime;             // Time for the next shot

    void Start()
    {
        aiPath = GetComponent<AIPath>();
    }

    void Update()
    {
        FollowOrAttack();
    }

    void FollowOrAttack()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, player.transform.position);

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
            aiPath.canMove = true; // Follow player
        }
        else
        {
            aiPath.canMove = false; // Stop following if out of range
        }
    }

    void FireProjectile()
    {
        // Instantiate the projectile at the enemy's position
        GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);

        // Calculate direction to the player
        Vector2 directionToPlayer = (player.transform.position - transform.position).normalized;

        // Set projectile direction
        projectile.GetComponent<Projectile>().SetDirection(directionToPlayer);
    }
}
