using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class LootSlot : MonoBehaviour
{
    
    //Loot Data
    public string lootName;
    public Sprite lootSprite;
    public GameObject droppedItemPrefab;

    //public int quantity; 
    public bool isFull;

    //Loot Slot
    [SerializeField]
    private Image lootImage;


    public void AddItem(Loot loot){
        lootImage = GetComponent<Image>();
        this.lootName = lootName;
        this.lootSprite = lootSprite;
        isFull = true;

        lootImage.sprite = lootSprite;
        lootImage.enabled = true;
    }
    
    public void ClearSlot()
    {
        //lootName = string.Empty; // Clear the loot name
       // lootSprite = null; // Clear the loot sprite
       // isFull = false; // Mark the slot as empty
       // lootImage.sprite = null; // Clear the image
        //lootImage.enabled = false; // Hide the image
    }
    
    // Start is called before the first frame update
    void Start()
    {
        ClearSlot();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
