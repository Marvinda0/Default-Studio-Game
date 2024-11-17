using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{

    
    [SerializeField]private GameObject uiPrompt;
    private bool isPlayerInRange = false;
  



    void OpenChest(){
        GetComponent<LootBag>().InstantiateLoot(transform.position); //jch6 Reference to lootbag on enemy to instantiate the loot on monster death

        //Add Open Chest animation here

        // 
        gameObject.SetActive(false); //jch6 Chest is disabled after opening
        if(uiPrompt != null){
                uiPrompt.SetActive(false);
            }
    }
    
    private void OnTriggerEnter2D(Collider2D other){
        if(other.CompareTag("Player")){
            isPlayerInRange = true;
            //Add UI prompt to press "F"
            if(uiPrompt != null){
                uiPrompt.SetActive(true);
            }
            //

        } //jch6  When player enters interaction range
    }

    private void OnTriggerExit2D(Collider2D other){
        if(other.CompareTag("Player")){
            isPlayerInRange = false;
            //Hide UI prompt
            if(uiPrompt != null){
                uiPrompt.SetActive(false);
            }
            //

        } //jch6  When player enters interaction range
    }

    
    void Start(){
        if(uiPrompt != null){
            uiPrompt.SetActive(false); //jch6 added to ensure prompt is disabled
        }
    }
    
    void Update()
    {
        if(isPlayerInRange && Input.GetKeyDown(KeyCode.F)){
            OpenChest();
        }

        if (uiPrompt != null && isPlayerInRange)
        {
            uiPrompt.transform.position = Camera.main.WorldToScreenPoint(transform.position + new Vector3(0, 1, 0)); // Offset prompt above chest
        }
    }


}
