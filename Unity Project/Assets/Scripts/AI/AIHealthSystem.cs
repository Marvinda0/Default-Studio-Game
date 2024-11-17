using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIHealthSystem : MonoBehaviour
{
    public int maxHealth = 100; // Maximum health of the enemy
    private int currentHealth;

    public delegate void MonsterDeath(int exp); //jch6 defines what info is passed along
    public static event MonsterDeath OnMonsterDeath;// jch6 notifies expmanager when the monster dies
    public int expReward = 2; //jch6 This is experience reward

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

        GetComponent<LootBag>().InstantiateLoot(transform.position); //jch6 Reference to lootbag on enemy to instantiate the loot on monster death
        OnMonsterDeath(expReward); //jch6 calls the event for Experience when monster dies
        Destroy(gameObject); // Destroy the enemy object
    }
}
