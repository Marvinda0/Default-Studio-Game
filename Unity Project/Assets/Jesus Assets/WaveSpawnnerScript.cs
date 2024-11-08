using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]

public class Wave
{
    public string waveName;
    public int NumberOfEnemies;
    public GameObject[] typesOfEnemis;
    public float spawnInterval;

}
public class WaveSpawnnerScript : MonoBehaviour
{
    public Wave[] Waves;
    public Transform[] spawnPoint;

    private Wave currentWave;
    private int curretnWaveNumber;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    private void Update()
    {
        currentWave = Waves[curretnWaveNumber];
    }
    void SpawnWave()
    {

    }
}
