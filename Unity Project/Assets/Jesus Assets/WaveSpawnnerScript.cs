using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
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

    private Wave currentWave;
    private int curretnWaveNumber;
    private float nextSpawnTime;

    bool canSpawnEnemies = true;
    void Start()
    {
        
    }

    // Update is called once per frame
    private void Update()
    {
        currentWave = Waves[curretnWaveNumber];
        SpawnWave();
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
