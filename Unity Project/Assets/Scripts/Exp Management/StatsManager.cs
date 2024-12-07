using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatsManager : MonoBehaviour

//jch6 add (StatsManager.Instance.speed,damage, maxHealth, currentHealth) into corresponding Player scripts dealing with damage, speed, health, 
{
    public static StatsManager Instance;
    
    // Singleton pattern jch6
    [Header("Combat Stats")]
    public float damage;
    public float knockbackForce;

    [Header("Movement Stats")]
    public float speed;

    [Header("Health Stats")]
    public float maxHealth;
    public float currentHealth;

    // Default values for resetting stats
    private float defaultDamage = 10f;
    private float defaultKnockbackForce = 15f;
    private float defaultSpeed = 600f;
    private float defaultMaxHealth = 100f;

    private void Awake(){
        if(Instance == null){
            Instance = this;

        } else {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        ResetStats(); // Set stats to their default values at the start
    }

    // Function to reset stats to their default values
    public void ResetStats()
    {
        damage = defaultDamage;
        knockbackForce = defaultKnockbackForce;
        speed = defaultSpeed;
        maxHealth = defaultMaxHealth;
        currentHealth = maxHealth;

        Debug.Log("Stats reset to default values.");
    }
}
