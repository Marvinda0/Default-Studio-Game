using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Wave
{
    public string waveName;
    public int NumberOfEnemies;
    public GameObject[] typesOfEnemies;
    public float spawnInterval;
}

public class WaveSpawnnerScript : MonoBehaviour
{
    public Wave[] Waves;
    public Transform[] spawnPoints;
    public Transform playerTransform;

    public Animator animator;
    public Text waveName;

    public bool isRoomComplete = false; // Track if room is complete

    private Wave currentWave;
    private int currentWaveNumber = 0;
    private float nextSpawnTime;
    private int remainingEnemiesInWave; // Counter for enemies in the current wave
    private int activeEnemies; // Counter for active enemies

    bool canSpawnEnemies = true;

    void Start()
    {
        InitializeWave();
    }

    private void Update()
    {
        SpawnWave();
        // Check if all enemies are defeated to spawn the next wave
        if (activeEnemies == 0 && !canSpawnEnemies && currentWaveNumber + 1 < Waves.Length)
        {
            MobStatsManager.Instance.ScaleStats();
            spawnNextWave();
        }
    }

    void InitializeWave()
    {
        if (Waves.Length != 0)
        {
            currentWave = Waves[currentWaveNumber];
            remainingEnemiesInWave = currentWave.NumberOfEnemies; // Set remaining enemies based on wave data
            canSpawnEnemies = true;
        }
    }

    void spawnNextWave()
    {
        currentWaveNumber++;
        InitializeWave();
    }

    void SpawnWave()
    {
        if (canSpawnEnemies && nextSpawnTime < Time.time && remainingEnemiesInWave > 0)
        {
            GameObject randomEnemy = Instantiate(
                currentWave.typesOfEnemies[Random.Range(0, currentWave.typesOfEnemies.Length)],
                spawnPoints[Random.Range(0, spawnPoints.Length)].position,
                Quaternion.identity
            );

            // Set the player's transform as the target
            AIDestinationSetter destinationSetter = randomEnemy.GetComponent<AIDestinationSetter>();
            if (destinationSetter != null)
            {
                destinationSetter.target = playerTransform; // Set the player as the target
            }

            activeEnemies++;
            remainingEnemiesInWave--; // Reduce remaining enemies in the current wave
            nextSpawnTime = Time.time + currentWave.spawnInterval;

            if (remainingEnemiesInWave == 0)
            {
                canSpawnEnemies = false; // Stop spawning when wave is complete
            }
        }
    }

    // Method to decrease active enemies when an enemy is defeated or destroyed
    public void OnEnemyDefeated()
    {
        activeEnemies--;

        // Ensure we check if the room or wave is done each time an enemy dies
        if ((activeEnemies == 0 && remainingEnemiesInWave == 0) && currentWaveNumber + 1 == Waves.Length)
        {
            OpenDoors();
        }
    }

    private void OpenDoors()
    {
        isRoomComplete = true; // Set the room as complete
        Debug.Log("Room is complete. Doors are open!");
    }

    public void ResetWaves()
    {
        // Reset the wave state variables
        currentWaveNumber = 0;
        activeEnemies = 0;
        canSpawnEnemies = true;

        // Reset the state for the first wave
        InitializeWave();

        // Reset the "room complete" status
        isRoomComplete = false;

        Debug.Log("WaveSpawner has been reset.");
    }
}
