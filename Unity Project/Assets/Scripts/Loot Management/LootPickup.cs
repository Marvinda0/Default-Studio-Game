using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootPickup : MonoBehaviour //Attach the LootPickup Script to the loot prefab jch6
{
    [SerializeField]private GameObject pickUpLootPrompt;
    private GameObject pickUpLootPromptInstance;
    private bool isPlayerInRange = false;
    public Sprite lootSprite;
    public string lootName;
    
    public string lootDescription;
    public Loot loot;

    public InventoryManager inventoryManager;

    void Start(){

        if(pickUpLootPrompt != null){
            Canvas uiCanvas = GameObject.Find("UIPopUpCanvas").GetComponent<Canvas>();
            
            pickUpLootPromptInstance = Instantiate(pickUpLootPrompt, uiCanvas.transform);
            pickUpLootPromptInstance.SetActive(false); //jch6 added to ensure prompt is disabled
        }
        inventoryManager = GameObject.Find("InventoryCanvas").GetComponent<InventoryManager>();//jch6
    }

    public void SetLoot(Loot newLoot){//jch6 set the loot item dynamically

        loot = newLoot;

    }
    
    void PickUpLoot(){

        bool wasItemAdded = inventoryManager.AddItem(loot.lootName, loot.lootSprite, loot.lootDescription, loot.lootType);
        if(wasItemAdded){
            Destroy(gameObject); //jch6 Destroys loot after pick up
            if(pickUpLootPromptInstance != null){
                pickUpLootPromptInstance.SetActive(false);
            }
        } else {
            Debug.Log("Could not pick up item, inventory is currently full!");
        }
        
        /*Destroy(gameObject); //jch6 Destroys loot after pick up
        if(pickUpLootPromptInstance != null){
            pickUpLootPromptInstance.SetActive(false);
        }*/

    }

    private void OnTriggerEnter2D(Collider2D other){
        if(other.CompareTag("Player")){
            isPlayerInRange = true;

            //Add UI prompt to press "F"
            if(pickUpLootPromptInstance != null){
                pickUpLootPromptInstance.SetActive(true);
            }
            //

        } //jch6  When player enters interaction range
    }

    private void OnTriggerExit2D(Collider2D other){
        if(other.CompareTag("Player")){
            isPlayerInRange = false;
            //Hide UI prompt
            if(pickUpLootPromptInstance != null){
                pickUpLootPromptInstance.SetActive(false);
            }
            //

        } //jch6  When player leaves interaction range
    }

    // Update is called once per frame
    void Update()
    {
        if(isPlayerInRange && Input.GetKeyDown(KeyCode.F)){
            PickUpLoot();
        }
        if (pickUpLootPromptInstance != null && isPlayerInRange){
            pickUpLootPromptInstance.transform.position = Camera.main.WorldToScreenPoint(transform.position + new Vector3(0, 0.2f, 0)); // Offset prompt above chest
        }

    }
}
