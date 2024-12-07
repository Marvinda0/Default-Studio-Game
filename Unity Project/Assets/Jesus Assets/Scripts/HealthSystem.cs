using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HealthSystem : MonoBehaviour
{
    public float maxHealth;
    private float currentHealth;
    public bool isEnemy = false;
    public delegate void MonsterDeath(int exp); //jch6 
    public static event MonsterDeath OnMonsterDeath;//jch6

    public int expReward;

    public Slider healthSlider;

    void Start()
    {
        //maxHealth = StatsManager.Instance.maxHealth;
        //currentHealth = StatsManager.Instance.currentHealth;
        if (CompareTag("Player")){
            maxHealth = StatsManager.Instance.maxHealth;
            currentHealth = StatsManager.Instance.currentHealth;
        }

        currentHealth = maxHealth;
        //UpdateUI();

    }

    void Update()
    {
        if (CompareTag("Player"))
        {
            currentHealth = StatsManager.Instance.currentHealth;
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
        if (isEnemy)
        {
            if (Input.GetKeyDown(KeyCode.K))
            {
                Die();
            }
        }
    }

    //public void SyncWithStatsManager(){
    //maxHealth = StatsManager.Instance.maxHealth;
    //currentHealth = StatsManager.Instance.currentHealth;
    //UpdateUI();
    //}

    // Call this function to reduce health
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

            //if(ExpManager.Instance.currentExp >= ExpManager.Instance.expToLevelUp){
                //ExpManager.Instance.LevelUp();
            //}
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
            GetComponent<LootBag>().InstantiateLoot(transform.position);
            OnMonsterDeath(expReward);
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

    public void UpdateUI(){//jch6 updates the UI for experience gain and when leveling up
        healthSlider.maxValue = maxHealth;
        healthSlider.value = currentHealth;
        
    }
}
