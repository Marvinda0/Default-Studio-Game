using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    public float maxHealth = 100f;
    private float currentHealth;
    public bool isEnemy = false;  // Set this to true for enemies, false for player

    void Start()
    {
        currentHealth = maxHealth;
    }

    void Update()
    {
        // If this is an enemy and "K" is pressed, destroy this enemy
        if (isEnemy && Input.GetKeyDown(KeyCode.K))
        {
            Die();
        }
    }

    // Call this function to reduce health
    public void TakeDamage(float amount)
    {
        currentHealth -= amount;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        if (isEnemy)
        {
            // Notify the WaveSpawnnerScript that an enemy has died
            WaveSpawnnerScript waveSpawner = FindObjectOfType<WaveSpawnnerScript>();
            if (waveSpawner != null)
            {
                waveSpawner.OnEnemyDefeated();
            }
        }

        Destroy(gameObject); // Destroy this object (either player or enemy)
    }
}
