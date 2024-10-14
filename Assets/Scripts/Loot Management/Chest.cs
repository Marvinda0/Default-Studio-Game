using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{

    private bool isPlayerInRange = false;
  



    void OpenChest(){
        GetComponent<LootBag>().InstantiateLoot(transform.position); //jch6 Reference to lootbag on enemy to instantiate the loot on monster death

        //Add Open Chest animation here

        // 
        gameObject.SetActive(false); //jch6 Chest is disabled after opening

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

        } //jch6  When player enters interaction range
    }

    
    void Start(){
        
    }
    
    void Update()
    {
        if(isPlayerInRange && Input.GetKeyDown(KeyCode.F)){
            OpenChest();
        }
    }


}
