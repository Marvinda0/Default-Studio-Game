using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootBag : MonoBehaviour
{
    public GameObject droppedItemPrefab;
    public List <Loot> lootList = new List<Loot>();
    
    Loot GetDroppedItem(){
        int randomNumber = Random.Range(1, 101);
        List <Loot> possibleItems = new List<Loot>();

        foreach(Loot item in lootList){
            
            if(randomNumber <= item.dropChance){
                possibleItems.Add(item);
            }
        }
        
        if(possibleItems.Count > 0){
            return possibleItems[Random.Range(0, possibleItems.Count)];
        }

        Debug.Log("No loot");
        return null;
    }

    public void InstantiateLoot (Vector3 spawnPosition){
        Loot droppedItem = GetDroppedItem();
        if(droppedItem != null){
            GameObject lootGameObject = Instantiate(droppedItemPrefab, spawnPosition, Quaternion.identity);
            lootGameObject.GetComponent <SpriteRenderer>().sprite = droppedItem.lootSprite;

            SpriteRenderer spriteRenderer = lootGameObject.GetComponent<SpriteRenderer>();
            if (spriteRenderer != null)
            {
                spriteRenderer.sprite = droppedItem.lootSprite;
            }
            else
            {
                Debug.LogWarning("Dropped item prefab is missing a SpriteRenderer component.");
            }

            // Set the Loot reference on LootPickup for inventory handling
            LootPickup lootPickup = lootGameObject.GetComponent<LootPickup>();
            if (lootPickup != null)
            {
                lootPickup.SetLoot(droppedItem);
            }
            else
            {
                Debug.LogWarning("Dropped item prefab is missing a LootPickup component.");
            }




            float dropForce = 35f;
            Vector2 dropDirection = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
            lootGameObject.GetComponent<Rigidbody2D>().AddForce(dropDirection * dropForce, ForceMode2D.Impulse);
        }
    }
}
