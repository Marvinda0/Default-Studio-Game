using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HealthSystem : MonoBehaviour
{
    public float maxHealth;
    private float currentHealth;
    public bool isBoss = false;
    public bool isEnemy = false;

    public delegate void MonsterDeath(int exp);
    public static event MonsterDeath OnMonsterDeath;

    public int expReward;
    public Slider healthSlider;

    void Start()
    {
        if (CompareTag("Player"))
        {
            maxHealth = StatsManager.Instance.maxHealth;
            currentHealth = StatsManager.Instance.currentHealth;
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

        if (CompareTag("Player"))
        {
            float damageTaken = previousHealth - currentHealth;
            StatsManager.Instance.currentHealth = currentHealth;
            Debug.Log($"Player took {damageTaken} damage, remaining HP: {currentHealth}");
            UpdateUI();
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        if (isEnemy && !isBoss)
        {
            WaveSpawnnerScript waveSpawner = FindObjectOfType<WaveSpawnnerScript>();
            if (waveSpawner != null)
            {
                waveSpawner.OnEnemyDefeated();
            }
            GetComponent<LootBag>().InstantiateLoot(transform.position);
            OnMonsterDeath?.Invoke(expReward);
            Destroy(gameObject);
        }

        if (isBoss)
        {
            Destroy(gameObject);
            //End game
        }
        else if (CompareTag("Player"))
        {
            RestartGame();
        }
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
