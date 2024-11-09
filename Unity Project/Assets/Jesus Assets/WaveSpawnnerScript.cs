using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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

    private Wave currentWave;
    private int currentWaveNumber;
    private float nextSpawnTime;

    private int activeEnemies; // Counter for active enemies
    bool canSpawnEnemies = true;

    void Start()
    {
        currentWave = Waves[currentWaveNumber];
    }

    private void Update()
    {
        SpawnWave();

        // Check if all enemies are defeated and it's time to spawn the next wave
        if (activeEnemies == 0 && !canSpawnEnemies && currentWaveNumber + 1 < Waves.Length)
        {
            spawnNextWave();
        }
        else if (activeEnemies == 0 && currentWaveNumber + 1 == Waves.Length)
        {
            // Room is done, final wave has been completed
        }
    }

    void spawnNextWave()
    {
        currentWaveNumber++;
        currentWave = Waves[currentWaveNumber];
        canSpawnEnemies = true;
    }

    void SpawnWave()
    {
        if (canSpawnEnemies && nextSpawnTime < Time.time)
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
            currentWave.NumberOfEnemies--;
            nextSpawnTime = Time.time + currentWave.spawnInterval;

            if (currentWave.NumberOfEnemies == 0)
            {
                canSpawnEnemies = false;
            }
        }
    }

    // Method to decrease active enemies when an enemy is defeated or destroyed
    public void OnEnemyDefeated()
    {
        activeEnemies--;
    }
}

