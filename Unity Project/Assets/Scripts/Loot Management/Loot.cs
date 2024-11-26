using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Loot : ScriptableObject
{
    
    public Sprite lootSprite;
    public string lootName;
    public LootType lootType;
    [TextArea]public string lootDescription;
    public int dropChance;

    public int attackDamage;
    public int movementSpeed;
    public int healthIncrease;
    public int healthRegen;

    public void EquipItem(){
        //Update Stats
        StatsManager.Instance.damage += attackDamage;
        StatsManager.Instance.speed += movementSpeed;
        StatsManager.Instance.maxHealth += healthIncrease;
        
        StatsManager.Instance.currentHealth = Mathf.Min(StatsManager.Instance.currentHealth, StatsManager.Instance.maxHealth);
        Debug.Log($"Equipped {lootName}: Damage +{attackDamage}, Speed +{movementSpeed}, Health +{healthIncrease}");

        
    }

    public void UnEquipItem(){

        StatsManager.Instance.damage -= attackDamage;
        StatsManager.Instance.speed -= movementSpeed;
        StatsManager.Instance.maxHealth -= healthIncrease;

        
        if (StatsManager.Instance.currentHealth > StatsManager.Instance.maxHealth)
        {
            StatsManager.Instance.currentHealth = StatsManager.Instance.maxHealth;
        }

        Debug.Log($"Unequipped {lootName}: Damage -{attackDamage}, Speed -{movementSpeed}, Health -{healthIncrease}");

    }
}