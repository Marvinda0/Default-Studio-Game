using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CerberusController : MonoBehaviour
{
    [Header("Health & Phases")]
    public float phase2Threshold = 70f; // Health percentage for Phase 2
    public float phase3Threshold = 35f; // Health percentage for Phase 3
    private int currentPhase = 1;

    [Header("Projectiles")]
    public GameObject projectilePrefab;     // Projectile prefab
    public float fireRate = 1f;             // Delay between shots
    private float nextFireTime;
    public int projectileDamage = 10;       // Damage dealt by projectiles
    public float projectileSpreadAngle = 45f; // Spread angle for projectiles

    [Header("Charging")]
    public float chargeSpeed = 8f;         // Speed when charging
    public float chargeCooldown = 5f;     // Cooldown between charges
    private float nextChargeTime;
    private bool isCharging;
    public Transform roomBounds; // Reference to the room bounds (BoxCollider2D)
    private Bounds bounds; // Stores the bounds of the room
    public float chargeDuration = 1f;

    [Header("Fire Circles")]
    public GameObject fireCirclePrefab;    // Fire circle prefab
    public Transform[] fireCircleSpawnPoints;
    public float fireCircleInterval = 15f; // Interval for spawning fire circles
    private float nextFireCircleTime;

    [Header("Minion Summoning")]
    public GameObject minionPrefab;
    public int minionsPerSummon = 3;
    public Transform[] minionSpawnPoints;
    public float summonCooldown = 10f;
    private float nextSummonTime;
    public int maxActiveMinions = 5;
    private List<GameObject> activeMinions = new List<GameObject>();

    [Header("Visual Feedback")]
    public SpriteRenderer spriteRenderer; // Reference to Cerberus's sprite renderer
    public Color chargeColor = Color.red; // Color when preparing to charge
    private Color originalColor; // Store the original color

    private Transform playerTransform;
    private HealthSystem healthSystem; // Reference to the HealthSystem component

    void Start()
    {
        // Get the HealthSystem component
        healthSystem = GetComponent<HealthSystem>();
        if (healthSystem == null)
        {
            Debug.LogError("HealthSystem component not found on Cerberus!");
            return;
        }

        // Find the player by tag
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
        }
        else
        {
            Debug.LogWarning("Player not found. Make sure the player has the 'Player' tag.");
        }
        if (spriteRenderer != null)
        {
            originalColor = spriteRenderer.color;
        }
        if (roomBounds != null)
        {
            BoxCollider2D roomCollider = roomBounds.GetComponent<BoxCollider2D>();
            if (roomCollider != null)
            {
                bounds = roomCollider.bounds;
            }
            else
            {
                Debug.LogError("RoomBounds object is missing a BoxCollider2D!");
            }
        }
    }

    void Update()
    {
        if (healthSystem == null || playerTransform == null) return;

        HandlePhases();
        HandleAttacks();
    }

    void HandlePhases()
    {
        float healthPercentage = (healthSystem.currentHealth / healthSystem.maxHealth) * 100;

        if (healthPercentage <= phase3Threshold && currentPhase != 3)
        {
            chargeCooldown = 2;
            currentPhase = 3;
            Debug.Log("Phase 3 Activated");
        }
        else if (healthPercentage <= phase2Threshold && currentPhase != 2)
        {
            currentPhase = 2;
            Debug.Log("Phase 2 Activated");
        }
    }

    void HandleAttacks()
    {
        if (currentPhase == 1)
        {
            HandleFireCircles();
            HandleProjectiles();
        }
        else if (currentPhase == 2)
        {
            HandleCharge();
            HandleFireCircles();
            SummonMinions();
        }
        else if (currentPhase == 3)
        {
            HandleProjectiles();
            HandleCharge();
            HandleFireCircles();
        }
    }

    void HandleProjectiles()
    {
        if (Time.time >= nextFireTime)
        {
            FireSpreadProjectiles();
            nextFireTime = Time.time + fireRate;
        }
    }

    void FireSpreadProjectiles()
    {
        if (projectilePrefab == null) return;

        float angleStep = projectileSpreadAngle / 2;
        for (float angle = -angleStep; angle <= angleStep; angle += angleStep)
        {
            Vector2 direction = Quaternion.Euler(0, 0, angle) * (playerTransform.position - transform.position).normalized;
            GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
            Projectile projectileScript = projectile.GetComponent<Projectile>();

            if (projectileScript != null)
            {
                projectileScript.SetDirection(direction);
                projectileScript.SetDamage(projectileDamage);
            }
        }
    }

    void HandleCharge()
    {
        if (!isCharging && Time.time >= nextChargeTime)
        {
            StartCoroutine(PrepareAndCharge());
        }
    }

    public IEnumerator ChargeAtPlayer(Transform playerTransform)
    {
        if (isCharging || playerTransform == null) yield break; // Prevent multiple charges

        isCharging = true; // Indicate that the boss is currently charging

        Vector2 chargeDirection = (playerTransform.position - transform.position).normalized; // Direction towards the player
        float elapsedTime = 0f;

        while (elapsedTime < chargeDuration)
        {
            Vector2 newPosition = (Vector2)transform.position + chargeDirection * chargeSpeed * Time.deltaTime;

            // Clamp the position within the room bounds
            newPosition.x = Mathf.Clamp(newPosition.x, bounds.min.x, bounds.max.x);
            newPosition.y = Mathf.Clamp(newPosition.y, bounds.min.y, bounds.max.y);

            transform.position = newPosition;

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Stop charging movement after charge duration
        isCharging = false;

        // Add a slight pause before the boss can move or attack again
        yield return new WaitForSeconds(0.5f);

        // Set the cooldown for the next charge
        nextChargeTime = Time.time + chargeCooldown;
    }

    private IEnumerator PrepareAndCharge()
    {
        if (spriteRenderer != null)
        {
            // Change color to red
            spriteRenderer.color = chargeColor;
        }

        // Wait for 1 second before charging
        yield return new WaitForSeconds(1f);

        if (spriteRenderer != null)
        {
            // Revert to the original color
            spriteRenderer.color = originalColor;
        }

        // Perform the charge
        yield return ChargeAtPlayer(playerTransform);
    }


    void HandleFireCircles()
    {
        if (Time.time >= nextFireCircleTime && fireCirclePrefab != null)
        {
            // Number of fire circles to spawn
            int fireCircleCount = 1; 

            for (int i = 0; i < fireCircleCount; i++)
            {
                // Generate a random position around Cerberus within a defined radius
                Vector2 randomPosition = (Vector2)playerTransform.position + Random.insideUnitCircle * 1f;   

                // Instantiate the fire circle prefab at the random position
                Instantiate(fireCirclePrefab, randomPosition, Quaternion.identity);
            }

            nextFireCircleTime = Time.time + fireCircleInterval; // Set next spawn time
        }
    }
    void SummonMinions()
    {
        // Check if it's time to summon and if there are fewer active minions than the max
        if (Time.time >= nextSummonTime && activeMinions.Count < maxActiveMinions)
        {
            // Summon minions at the available summon points
            foreach (Transform summonPoint in minionSpawnPoints)
            {
                if (activeMinions.Count >= maxActiveMinions) break; // Ensure we don't exceed the limit

                GameObject minion = Instantiate(minionPrefab, summonPoint.position, Quaternion.identity);
                activeMinions.Add(minion);
            }

            // Update the next summon time
            nextSummonTime = Time.time + summonCooldown;
        }

        // Clean up inactive minions from the list
        activeMinions.RemoveAll(minion => minion == null);
    }
}
