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
    [SerializeField]private GameObject droppedItemPrefab;

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
    private Loot currentLoot;


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
            lootDescriptionImage.sprite = emptySprite;
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
                UseLoot();
            }
        }else{
            inventoryManager.DeselectAllSlots();
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
        } else {
            Debug.LogWarning("No item not found!");
        }
    }

    public void UseLoot(){
        Loot currentLoot = FindLootByName(lootName);
        if(currentLoot != null && lootType == LootType.consumable && StatsManager.Instance.currentHealth != StatsManager.Instance.maxHealth){
            currentLoot.UseItem();
            ClearSlot();
        } else {
            Debug.LogWarning("No item not found!");
        }
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

    public void OnRightClick(){
        //jch6 option to delete or drop loot item from inventory
        if(thisItemSelected && isFull){
            DropItem();
        }
    }

    public void DropItem(){
        if (droppedItemPrefab == null)
        {
            Debug.LogError("droppedItemPrefab is not assigned in the Inspector!");
            return;
        }

        // Spawn the dropped item in the world near the player
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            Vector3 dropPosition = player.transform.position + new Vector3 (Random.Range(0.5f, 0.5f), Random.Range(-0.5f, 0.5f), 0);
            GameObject droppedItem = Instantiate(droppedItemPrefab, dropPosition, Quaternion.identity);

            // Set the sprite on the dropped item
            SpriteRenderer spriteRenderer = droppedItem.GetComponent<SpriteRenderer>();
            if (spriteRenderer != null){
                spriteRenderer.sprite = lootSprite;
            }

            // Assign LootPickup data so it can be re-picked up
            LootPickup lootPickup = droppedItem.GetComponent<LootPickup>();
            if (lootPickup != null)
            {
                Loot currentLoot = FindLootByName(lootName);
                if (currentLoot != null){
                    lootPickup.SetLoot(currentLoot);
                }
            }

            // Optional: Add some force to make it 'pop' out
            Rigidbody2D rb = droppedItem.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                float dropForce = 5f;
                Vector2 dropDirection = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
                rb.AddForce(dropDirection.normalized * dropForce, ForceMode2D.Impulse);
            }

            // Clear the slot
            ClearSlot();
            Debug.Log($"Dropped {lootName} in the world.");
        }
        else
        {
            Debug.LogWarning("Player not found - cannot drop the item!");
        }

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