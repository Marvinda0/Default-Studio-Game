using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatsManager : MonoBehaviour

//jch6 add (StatsManager.Instance.speed,damage, maxHealth, currentHealth) into corresponding Player scripts dealing with damage, speed, health, 
{
    public static StatsManager Instance;
    
    // Singleton pattern jch6
    [Header("Combat Stats")]
    public int damage;
    //public float weaponRange;

    [Header("Movement Stats")]
    public int speed;

    [Header("Health Stats")]
    public int maxHealth;
    public int currentHealth;


    private void Awake(){
        if(Instance == null){
            Instance = this;

        } else {
            Destroy(gameObject);
        }
    }
}
