using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{

    public GameObject InventoryMenu;
    private bool menuActivated;

    public LootSlot[] lootSlot;

    //public Loot loot;




    // Start is called before the first frame update
    void Start()
    {
        if(InventoryMenu == null){
            Debug.LogError("InventoryMenu is not assigned");
        }

    }

    // Update is called once per frame
    void Update()
    {
        if(menuActivated && Input.GetKeyDown(KeyCode.I)){
            
            Time.timeScale = 1; //jch6 unpauses game when Inventory is selected
            
            InventoryMenu.SetActive(false);
            menuActivated = false;
        }

        else if(!menuActivated && Input.GetKeyDown(KeyCode.I)){
            
            Time.timeScale = 0; //jch6 stops game when Inventory is opened

            InventoryMenu.SetActive(true);//jch6 Opens inventory menu
            menuActivated = true;
        }

    }

    public void AddItem(Loot loot){
    
        for(int i = 0; i < lootSlot.Length; i++){
            if(lootSlot[i].isFull == false){
                lootSlot[i].AddItem(loot);
                return;
            }
        }
        Debug.Log(loot + "loot has been added to inventory");
    }
}
