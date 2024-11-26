using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StatsUI : MonoBehaviour
{

    public GameObject[] statsSlots;

    private int allocatedDamagePoints = 0;
    private int allocatedSpeedPoints = 0;
    private int allocatedHealthPoints = 0;
    private void Start(){
        UpdateAllStats();
    }
    
    public GameObject StatsPanel;
    private bool menuActivated;
    

    // Update is called once per frame
    void Update()
    {
        if(menuActivated && Input.GetKeyDown(KeyCode.U)){
            
            Time.timeScale = 1; //jch6 unpauses game when Inventory is selected
            //UpdateAllStats();
            StatsPanel.SetActive(false);
            menuActivated = false;
        }

        else if(!menuActivated && Input.GetKeyDown(KeyCode.U))
        {
            
            Time.timeScale = 0; //jch6 stops game when Inventory is opened
            UpdateAllStats();
            StatsPanel.SetActive(true);//jch6 Opens inventory menu
            menuActivated = true;
        }

    }

    public void UpdateDamage(){
        //statsSlots[0].GetComponentInChildren<TMP_Text>().text = "Damage: " + StatsManager.Instance.damage;
        statsSlots[0].GetComponentInChildren<TMP_Text>().text = "Damage: " + allocatedDamagePoints;
    }

    public void UpdateSpeed(){
        //statsSlots[1].GetComponentInChildren<TMP_Text>().text = "Speed: " + StatsManager.Instance.speed;
        statsSlots[1].GetComponentInChildren<TMP_Text>().text = "Speed: " + allocatedSpeedPoints;
    }

    public void UpdateHealth(){
        //statsSlots[2].GetComponentInChildren<TMP_Text>().text = "Health: " + StatsManager.Instance.maxHealth;
        statsSlots[2].GetComponentInChildren<TMP_Text>().text = "Health: " + allocatedHealthPoints;
    }

    public void UpdateAllStats(){
        UpdateDamage();
        UpdateSpeed();
        UpdateHealth();
    }

    public void IncreaseDamage(){
        if (ExpManager.Instance.statPoint > 0){
            allocatedDamagePoints++;
            StatsManager.Instance.damage += 10;//Increases the actual damage
            ExpManager.Instance.statPoint -= 1;
            UpdateDamage();
            ExpManager.Instance.UpdateUI();
        }
    }

    public void IncreaseSpeed(){
        if (ExpManager.Instance.statPoint > 0){
            allocatedSpeedPoints++;
            StatsManager.Instance.speed += 700;
            ExpManager.Instance.statPoint-=1;
            UpdateSpeed();
            ExpManager.Instance.UpdateUI();
        }
    }

    public void IncreaseHealth(){
        if (ExpManager.Instance.statPoint > 0){
            allocatedHealthPoints++;
            StatsManager.Instance.maxHealth += 100;
            StatsManager.Instance.currentHealth = StatsManager.Instance.maxHealth;//jch6 when increasing stat health player regains new full health
            ExpManager.Instance.statPoint-=1;
            UpdateHealth();
            ExpManager.Instance.UpdateUI();

            
            Debug.Log("Max Health: " + StatsManager.Instance.maxHealth);
            Debug.Log("Current Health: " + StatsManager.Instance.currentHealth);
        }
    }
}
