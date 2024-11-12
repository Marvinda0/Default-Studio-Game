using UnityEngine;
using UnityEngine.SceneManagement;

public class HealthSystem : MonoBehaviour
{
    public float maxHealth = 100f;
    private float currentHealth;
    public bool isEnemy = false;

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
        float previousHealth = currentHealth;
        currentHealth -= amount;

        if (CompareTag("Player"))
        {
            float damageTaken = previousHealth - currentHealth;
            Debug.Log($"Player took {damageTaken} damage, remaining HP: {currentHealth}");
        }

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

            Destroy(gameObject); // Destroy the enemy object
        }
        else if (CompareTag("Player"))
        {
            // Restart the game if the player dies
            RestartGame();
        }
    }

    private void RestartGame()
    {
        // Reload the current scene to restart the game
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
