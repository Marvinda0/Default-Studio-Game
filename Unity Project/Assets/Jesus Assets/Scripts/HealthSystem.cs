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
        StatsManager.Instance.ResetStats();
        PersistentObject.ResetPersistentObject();

        WaveSpawnnerScript waveSpawner = FindObjectOfType<WaveSpawnnerScript>();
        if (waveSpawner != null)
        {
            waveSpawner.ResetWaves();
        }

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void UpdateUI()
    {
        healthSlider.maxValue = maxHealth;
        healthSlider.value = currentHealth;
    }
}
