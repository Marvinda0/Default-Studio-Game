using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootPickup : MonoBehaviour //Attach the LootPickup Script to the loot prefab jch6
{
    private bool isPlayerInRange = false;
    public Sprite lootSprite;
    public string lootName;

    public Loot loot;

    public InventoryManager inventoryManager;

    void Start(){

        
        inventoryManager = GameObject.Find("InventoryCanvas").GetComponent<InventoryManager>();//jch6
    }

    public void SetLoot(Loot newLoot){//jch6 set the loot item dynamically

        loot = newLoot;

    }
    
    void PickUpLoot(){

        inventoryManager.AddItem(loot.lootName, loot.lootSprite);
        
        Destroy(gameObject); //jch6 Destroys loot after pick up

    }

    private void OnTriggerEnter2D(Collider2D other){
        if(other.CompareTag("Player")){
            isPlayerInRange = true;

            //Add UI prompt to press "F"

            //

        } //jch6  When player enters interaction range
    }

    private void OnTriggerExit2D(Collider2D other){
        if(other.CompareTag("Player")){
            isPlayerInRange = false;
            //Hide UI prompt

            //

        } //jch6  When player leaves interaction range
    }

    // Update is called once per frame
    void Update()
    {
        if(isPlayerInRange && Input.GetKeyDown(KeyCode.F)){
            PickUpLoot();
        }
    }
}
