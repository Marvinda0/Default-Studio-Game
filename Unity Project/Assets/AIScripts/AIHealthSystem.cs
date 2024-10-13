using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIHealthSystem : MonoBehaviour
{
    public int maxHealth = 100; // Maximum health of the enemy //Placeholder
    private int currentHealth;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth; // Set current health to max health at start
    }

    // Method to take damage
    public void TakeDamage(int damage)
    {
        currentHealth -= damage; // Reduce current health by damage amount
        if (currentHealth <= 0)
        {
            Die(); // Call die method if health is zero or below
        }
    }

    // Method to handle death
    private void Die()
    {
        // Add logic for on-death event such as dropping loot, playing death animation, etc.
        Destroy(gameObject); // Destroy the enemy object
    }
}