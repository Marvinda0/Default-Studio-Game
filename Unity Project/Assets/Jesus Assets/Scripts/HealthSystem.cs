using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HealthSystem : MonoBehaviour
{
    public float maxHealth;
    public float currentHealth;
    public bool isBoss = false;
    public bool isEnemy = false;

    public delegate void MonsterDeath(int exp);
    public static event MonsterDeath OnMonsterDeath;

    public int expReward;
    public Slider healthSlider;
    public GameObject damageTextPrefab; 
    public Transform textSpawnPoint; 


    void Start()
    {
        if (CompareTag("Player"))
        {
            maxHealth = StatsManager.Instance.maxHealth;
            currentHealth = StatsManager.Instance.currentHealth;
        }
        else
        {
            currentHealth = maxHealth; // Ensure enemies start at max health
        }
        UpdateUI();
    }

    void Update()
    {
        if (CompareTag("Player"))
        {
            float previousMaxHealth = maxHealth; // Store previous max health
            maxHealth = StatsManager.Instance.maxHealth; // Update to new max health
            currentHealth = StatsManager.Instance.currentHealth;


            UpdateUI();
        }

        // Test functionality to kill an enemy by pressing "K"
        if (isEnemy && Input.GetKeyDown(KeyCode.K))
        {
            Die();
        }
    }

    public void TakeDamage(float amount)
    {
        float previousHealth = currentHealth;
        currentHealth -= amount;
        Debug.Log($"Damage received: {amount}, Current Health: {currentHealth}, Max Health: {maxHealth}");
        if (CompareTag("Player"))
        {
            float damageTaken = previousHealth - currentHealth;
            StatsManager.Instance.currentHealth = currentHealth;
            Debug.Log($"Player took {damageTaken} damage, remaining HP: {currentHealth}");
            UpdateUI();
        }
        if (damageTextPrefab != null)
        {
            GameObject damageText = Instantiate(damageTextPrefab, textSpawnPoint.position, Quaternion.identity);
            damageText.GetComponent<DamageText>().SetText(amount.ToString());
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        if (CompareTag("Player"))
        {
            Animator animator = GetComponent<Animator>();
            if (animator != null)
            {
                animator.SetTrigger("Death"); // Trigger the death animation
            }

            PlayerController playerController = GetComponent<PlayerController>();
            if (playerController != null)
            {
                playerController.Die(); // Disable movement
            }

            // Start the coroutine to restart the game after death animation
            StartCoroutine(RestartAfterDeath());
        }
        else if (isEnemy)
        {
            // Notify the WaveSpawnerScript that an enemy has died
            WaveSpawnnerScript waveSpawner = FindObjectOfType<WaveSpawnnerScript>();
            if (waveSpawner != null)
            {
                waveSpawner.OnEnemyDefeated();
            }
            GetComponent<LootBag>().InstantiateLoot(transform.position);
            OnMonsterDeath(expReward);
            Destroy(gameObject); // Destroy the enemy object
        }
    }

    private IEnumerator RestartAfterDeath()
    {
        yield return new WaitForSeconds(2f); // Wait for the animation to play out
        RestartGame();
    }

    private void RestartGame()
    {
        // Reset stats and persistent objects
        PersistentObject.ResetPersistentObject();
        StatsManager.Instance.ResetStats();

        // Reset wave spawner if necessary
        WaveSpawnnerScript waveSpawner = FindObjectOfType<WaveSpawnnerScript>();
        if (waveSpawner != null)
        {
            waveSpawner.ResetWaves();
        }

        // Load the first room
        SceneManager.LoadScene("Room 1.0");
    }

    public void UpdateUI()
    {
        if (!isEnemy)
        {
            healthSlider.maxValue = maxHealth;
            healthSlider.value = currentHealth;
        }
    }
}
