using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeAI : MonoBehaviour
{
    // Slime movement and damage properties
    public float damage = 1;
    public float knockbackForce = 20f;
    public float moveSpeed = 500f;

    // Health properties
    public int maxHealth = 100;
    private int currentHealth;

    // Detection and other components
    public List<GameObject> detectedObjs = new List<GameObject>(); // Simulate detection logic
    private Rigidbody2D rb;
    private DamageableCharacter damagableCharacter;

    void Start()
    {
        // Initialize components and health
        rb = GetComponent<Rigidbody2D>();
        damagableCharacter = GetComponent<DamageableCharacter>();
        currentHealth = maxHealth;

        // Debug checks
        if (rb == null) Debug.LogError("Rigidbody2D is not assigned.");
        if (damagableCharacter == null) Debug.LogError("DamageableCharacter is not assigned.");
    }

    void FixedUpdate()
    {
        // Move towards the detected object if conditions are met
        if (damagableCharacter.Targetable && detectedObjs.Count > 0)
        {
            Vector2 direction = (detectedObjs[0].transform.position - transform.position).normalized;
            rb.AddForce(direction * moveSpeed * Time.fixedDeltaTime);
        }
    }

    // Method to take damage
    public void TakeDamage(int damage)
    {
        currentHealth -= damage; // Reduce current health by damage amount
        if (currentHealth <= 0)
        {
            Die(); // Call Die method if health is zero or below
        }
    }

    // Method to handle death
    private void Die()
    {
        // Notify the wave spawner of the enemy's death if applicable
        WaveSpawnnerScript waveSpawner = FindObjectOfType<WaveSpawnnerScript>();
        if (waveSpawner != null)
        {
            waveSpawner.OnEnemyDefeated(); // Notify the spawner
        }

        // Add any additional logic for on-death event, such as animations or loot drops

        Destroy(gameObject); // Destroy the Slime object
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Example detection logic
        if (!detectedObjs.Contains(other.gameObject))
        {
            detectedObjs.Add(other.gameObject);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (detectedObjs.Contains(other.gameObject))
        {
            detectedObjs.Remove(other.gameObject);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Collider2D collider = collision.collider;
        IDamageable damageable = collider.GetComponent<IDamageable>();

        if (damageable != null)
        {
            Vector2 direction = (collider.transform.position - transform.position).normalized;
            Vector2 knockback = direction * knockbackForce;
            damageable.OnHit(damage, knockback);
        }
    }

    void Update()
    {
        // Check if K key is pressed to test the death functionality
        if (Input.GetKeyDown(KeyCode.K))
        {
            Die(); // Call Die method to test
        }
    }
}

