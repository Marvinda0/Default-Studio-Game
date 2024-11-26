using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class EquippedSlot : MonoBehaviour, IPointerClickHandler
{
   //Slot appearance
   [SerializeField]
   private Image slotImage;
   
   [SerializeField]
   private TMP_Text slotName;
//
   //Slot Data
   [SerializeField]
   private LootType lootType = new LootType();

   private Sprite lootSprite;
   private string lootName;
   private string lootDescription;

   private InventoryManager inventoryManager;

   private void Start(){
        inventoryManager = GameObject.Find("InventoryCanvas").GetComponent<InventoryManager>();
   }
//
   public bool slotInUse;

   

   [SerializeField]public GameObject selectedShader;

   [SerializeField] public bool thisItemSelected;

   [SerializeField] private Sprite emptySprite;

   public void OnPointerClick(PointerEventData eventData){
    if(eventData.button == PointerEventData.InputButton.Left){
        OnLeftClick();
    }

    if(eventData.button == PointerEventData.InputButton.Right){
        OnRightClick();
    }
   }

   void OnLeftClick(){
        if(thisItemSelected && slotInUse){
            UnEquipGear();
        } else {
            inventoryManager.DeselectAllSlots();
            selectedShader.SetActive(true);
            thisItemSelected = true;
        }
   }

   void OnRightClick(){
        UnEquipGear();
   }


   public void EquipGear(string name, Sprite sprite, string description, LootType type){
        if(slotInUse){
            UnEquipGear();
        }
        
        //jch6 Updating image
        lootSprite = sprite;
        slotImage.sprite = lootSprite;
        slotImage.enabled = true;
        slotName.enabled = false;
        //lootImage.enabled = true;

        //jch6 Update Data
        lootType = type;
        lootName = name;
        lootDescription = description;
        slotInUse = true;

        Loot loot = FindLootByName(name); // Retrieve Loot object by name
        if (loot != null)
        {
            loot.EquipItem(); // Apply loot stats
        }
        
        
   }

   public void UnEquipGear(){
        if (!slotInUse) return;

        // Remove Buff
        Loot loot = FindLootByName(lootName); // Retrieve Loot object by name
        if (loot != null)
        {
            loot.UnEquipItem(); // Revert loot stats
        }
        
        
        inventoryManager.DeselectAllSlots();

        inventoryManager.AddItem(lootName, lootSprite, lootDescription, lootType);
        
        //Update image
        lootSprite = emptySprite;
        slotImage.sprite = lootSprite;
        slotName.enabled = true;
        slotInUse = false;
   }

    private Loot FindLootByName(string name)
    {
        EquipmentSOLibrary library = FindObjectOfType<EquipmentSOLibrary>();
        foreach (var loot in library.lootLibrary)
        {
            if (loot.lootName == name) return loot;
        }
        return null;
    }
  

   
   public bool IsSlotInUse(){
    Debug.Log(slotInUse + "is in use");
    return slotInUse;
   }




}
