using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class LootSlot : MonoBehaviour, IPointerClickHandler
{
    //public Loot loot;
    //Loot Data
    private string lootName;
    private Sprite lootSprite;

    private LootType lootType;

    private string lootDescription;
    //public GameObject droppedItemPrefab;

    //Equipped slot
    [SerializeField]private EquippedSlot itemSlot1, itemSlot2;
    

    //public int quantity; 
    public bool isFull;

    public Sprite emptySprite;

    //Loot Slot
    [SerializeField]
    private Image lootImage;
    
    public GameObject selectedShader;
    public bool thisItemSelected;

    private InventoryManager inventoryManager;


    //Loot Descrip
    [SerializeField]private Image lootDescriptionImage;
    [SerializeField]private TMP_Text LootDescriptionNameText;
    [SerializeField]private TMP_Text LootDescriptionText;


    public void AddItem(string name, Sprite sprite, string description, LootType type){
        //lootImage = GetComponent<Image>();
        //if (loot == null)
       // {
          //  Debug.LogWarning("Attempted to add a null loot item to the slot.");
           // return;
       // }

        lootType = type;
        lootName = name;
        //loot.lootName = lootName;
        lootDescription = description;
        //this.lootName = lootName;
        lootSprite = sprite;
        //this.lootSprite = lootSprite;
        isFull = true;

        lootImage.sprite = lootSprite;
        lootImage.enabled = true;
    }
    
    public void ClearSlot()
    {
        //lootName = string.Empty; // Clear the loot name
        //lootSprite = null; // Clear the loot sprite
        isFull = false; // Mark the slot as empty
        
        lootImage.sprite = null; // Clear the image 
        lootImage.enabled = false; // Hide the image

        lootName = string.Empty;
        lootType = LootType.none;
        lootDescription = string.Empty;//these work need to delete bottom code
        lootSprite = emptySprite;//

        //selectedShader.SetActive(false);
        if(selectedShader != null){
            selectedShader.SetActive(false);
        }
        thisItemSelected = false;
        
        LootDescriptionNameText.text = string.Empty;
        LootDescriptionText.text = string.Empty;
        if(lootImage.sprite == null){
            lootDescriptionImage.sprite = null;
        }
        //lootDescriptionImage.sprite = null;


    }

    public void OnPointerClick(PointerEventData eventData){
        if(eventData.button == PointerEventData.InputButton.Left){
            OnLeftClick();
        }
        if(eventData.button == PointerEventData.InputButton.Right){
            OnRightClick();
        }
    }

    public void OnLeftClick(){
        if(thisItemSelected){
            if(lootType == LootType.equippable){
                EquipGear();
            } else if(lootType == LootType.consumable){
                //UseItem(); jch6 cre
            }
        }else{
            inventoryManager.DeselectAllSlots();
            //selectedShader.SetActive(true);
            if (selectedShader != null) {
                selectedShader.SetActive(true);
            }
            thisItemSelected = true;

            LootDescriptionNameText.text = lootName;
            LootDescriptionText.text = lootDescription;
            lootDescriptionImage.sprite = lootSprite;
        }
        if(lootDescriptionImage.sprite == null){
            lootDescriptionImage.sprite = emptySprite;
        }
    }
    private void EquipGear() {
        if (lootType == LootType.equippable) {
            bool equipped = false;

            if (!itemSlot1.IsSlotInUse()) {
                Debug.Log("Equipping loot item to Slot 1");
                itemSlot1.EquipGear(lootName, lootSprite, lootDescription, lootType);
                //ClearSlot();
                equipped = true;
            } else if (!itemSlot2.IsSlotInUse()) {
                Debug.Log("Equipping loot item to Slot 2");
                itemSlot2.EquipGear(lootName, lootSprite, lootDescription, lootType);
                //ClearSlot();
                equipped = true;
            } else {
                Debug.LogWarning("No available equipped slots!");
            }
            //ClearSlot();
            if(equipped){
                ClearSlot();
            }
        }
    }

    public void OnRightClick(){

    }
    
    // Start is called before the first frame update
    private void Start()
    {
        inventoryManager = GameObject.Find("InventoryCanvas").GetComponent<InventoryManager>();
        //ClearSlot();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}