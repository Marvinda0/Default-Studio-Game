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

    public Animator animator;
    public Text waveName;

    private Wave currentWave;
    private int curretnWaveNumber;
    private float nextSpawnTime;

    bool canSpawnEnemies = true;

    void Start()
    {
        
    }
    private void Update()
    {
        currentWave = Waves[curretnWaveNumber];
        SpawnWave();
        GameObject[] totalEnemies = GameObject.FindGameObjectsWithTag("Enemy");
        if (totalEnemies.Length == 0 && !canSpawnEnemies && curretnWaveNumber+1 != Waves.Length)
        {
            waveName.text = Waves[curretnWaveNumber + 1].waveName;
            animator.SetTrigger("WaveComplete");
            spawnNextWave();
        }
    }

    void spawnNextWave()
    {
        curretnWaveNumber++;
        canSpawnEnemies = true;
    }

    void SpawnWave()
    {
        if (canSpawnEnemies && nextSpawnTime < Time.time)
        {
            GameObject RandomEnemy = currentWave.typesOfEnemies[Random.Range(0, currentWave.typesOfEnemies.Length)];
            Transform randomSpawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
            Instantiate(RandomEnemy, randomSpawnPoint.position, Quaternion.identity);
            currentWave.NumberOfEnemies--;
            nextSpawnTime = Time.time + currentWave.spawnInterval;
            if (currentWave.NumberOfEnemies == 0)
            {
                canSpawnEnemies=false;
            }
        }
        
    }
}
