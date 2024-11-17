using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Loot : ScriptableObject
{
    
    public Sprite lootSprite;
    public string lootName;
    [TextArea]public string lootDescription;
    public int dropChance;

    //public Loot(string lootName, int dropChance, lootSprite){
        //this.lootName = lootName;
        //this.dropChance = dropChance;
        //this.lootSprite = lootSprite;
    //}
}