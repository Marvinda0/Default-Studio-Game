using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HealthSystem : MonoBehaviour
{
    public float maxHealth;
    private float currentHealth;
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
        else
        {
            currentHealth = maxHealth;
        }
    }

    void Update()
    {
        if (CompareTag("Player"))
        {
            float previousMaxHealth = maxHealth;
            maxHealth = StatsManager.Instance.maxHealth;

            // Scale current health proportionally when maxHealth changes
            if (maxHealth != previousMaxHealth)
            {
                float healthPercentage = currentHealth / previousMaxHealth;
                currentHealth = maxHealth * healthPercentage;
                UpdateUI();
            }
        }
        if (isEnemy && Input.GetKeyDown(KeyCode.K))
        {
            Die();
        }
    }

    // Reduce health
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
        if (isEnemy)
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
        else if (CompareTag("Player"))
        {
            RestartGame();
        }
    }

    private void RestartGame()
    {
        // Reset StatsManager
        StatsManager.Instance.ResetStats();

        // Reset Persistent Objects
        PersistentObject.ResetPersistentObject();

        // Reset WaveSpawner if necessary
        WaveSpawnnerScript waveSpawner = FindObjectOfType<WaveSpawnnerScript>();
        if (waveSpawner != null)
        {
            waveSpawner.ResetWaves();
        }

        // Reload the scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void UpdateUI()
    {
        healthSlider.maxValue = maxHealth;
        healthSlider.value = currentHealth;
    }
}
