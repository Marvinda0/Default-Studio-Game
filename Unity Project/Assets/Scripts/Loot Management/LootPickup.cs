using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootPickup : MonoBehaviour //Attach the LootPickup Script to the loot prefab jch6
{
    private bool isPlayerInRange = false;
    public Loot loot;


    void PickUpLoot(){

        //jch6 Add to give loot item here(add to Inventory)
        
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
