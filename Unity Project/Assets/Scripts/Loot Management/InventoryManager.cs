using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{

    public GameObject InventoryMenu;
    private bool menuActivated;

    public LootSlot[] lootSlots;

    public EquippedSlot[] equippedSlot;
    

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
        if(Input.GetKeyDown(KeyCode.I)){
            Debug.Log("I key pressed. Attempting to open inventory");
            MenuManager.Instance.OpenMenu(InventoryMenu);
        }
    }


    public bool AddItem(string lootName, Sprite lootSprite, string lootDescription, LootType lootType){
    
        if(lootType == LootType.consumable || lootType == LootType.equippable){
            for(int i = 0; i < lootSlots.Length; i++){
                if(lootSlots[i].isFull == false){
                    lootSlots[i].AddItem(lootName, lootSprite, lootDescription, lootType);
                    Debug.Log(lootName + "loot has been added to inventory");
                    return true;
                }
            }
        }
        
        Debug.LogWarning("Inventory is full. Can't add item into inventory");
        return false;
    }

    public void DeselectAllSlots(){
        for(int i = 0; i < lootSlots.Length; i++){
            lootSlots[i].selectedShader.SetActive(false);
            lootSlots[i].thisItemSelected = false;
        }
        for(int i = 0; i < equippedSlot.Length; i++){
            equippedSlot[i].selectedShader.SetActive(false);
            equippedSlot[i].thisItemSelected = false;
        }   
    }
}

public enum LootType{
    consumable,
    equippable, 
    none,
};
