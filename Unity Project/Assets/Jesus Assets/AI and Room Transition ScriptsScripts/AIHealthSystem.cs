using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIHealthSystem : MonoBehaviour
{
    public int maxHealth = 100; // Maximum health of the enemy
    private int currentHealth;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth; // Set current health to max health at start
    }

    void Update()
    {
        // Check if K key is pressed to test the death functionality
        if (Input.GetKeyDown(KeyCode.K))
        {
            Die(); // Call Die method to destroy the mob
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
        // Find the WaveSpawnnerScript in the scene and inform it of the enemy's death
        WaveSpawnnerScript waveSpawner = FindObjectOfType<WaveSpawnnerScript>();
        if (waveSpawner != null)
        {
            waveSpawner.OnEnemyDefeated(); // Notify the spawner of the enemy's death
        }

        // Add logic for on-death event such as dropping loot, playing death animation, etc.
        Destroy(gameObject); // Destroy the enemy object
    }
}
