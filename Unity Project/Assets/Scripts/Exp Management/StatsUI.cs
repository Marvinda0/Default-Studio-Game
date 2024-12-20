using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StatsUI : MonoBehaviour
{
    public static StatsUI Instance;

    public GameObject[] statsSlots;

    private int allocatedDamagePoints = 0;
    private int allocatedSpeedPoints = 0;
    private int allocatedHealthPoints = 0;
    private void Start(){
        UpdateAllStats();
        if(StatsPanel == null){
            Debug.LogError("Statspanel not assigned");
        }
    }
    
    public GameObject StatsPanel;
    public GameObject LevelUpPanel;
    private bool menuActivated;
    

    private void Awake() {
        if (Instance == null) {
            Instance = this;
        }
    }
    // Update is called once per frame
    void Update()
    {
       if (Input.GetKeyDown(KeyCode.U)){
            Debug.Log("U key pressed. Attempting to open stats");
            MenuManager.Instance.OpenMenu(StatsPanel);
        } 

    }

    public void ShowLevelUpPanel(){
        LevelUpPanel.SetActive(true);
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
        Debug.Log("Button clicked!");
        if (ExpManager.Instance.statPoint > 0){
            allocatedDamagePoints++;
            StatsManager.Instance.damage += 10;//Increases the actual damage
            ExpManager.Instance.statPoint -= 1;
            UpdateDamage();
            ExpManager.Instance.UpdateUI();

            if(ExpManager.Instance.statPoint <= 0 && LevelUpPanel.activeSelf){
                LevelUpPanel.SetActive(false);
            }
        }
    }

    public void IncreaseSpeed(){
        Debug.Log("Button clicked!");
        if (ExpManager.Instance.statPoint > 0){
            allocatedSpeedPoints++;
            StatsManager.Instance.speed += 80;//jch6 Increase the actual speed
            ExpManager.Instance.statPoint-=1;
            UpdateSpeed();
            ExpManager.Instance.UpdateUI();

            if(ExpManager.Instance.statPoint <= 0 && LevelUpPanel.activeSelf){
                LevelUpPanel.SetActive(false);
            }
        }
    }

    public void IncreaseHealth(){
        Debug.Log("Button clicked!");
        if (ExpManager.Instance.statPoint > 0){
            allocatedHealthPoints++;
            StatsManager.Instance.maxHealth += 20;//jch6 Increase actual health
            //StatsManager.Instance.currentHealth = StatsManager.Instance.maxHealth;//jch6 when increasing stat health player regains new full health
            ExpManager.Instance.statPoint-=1;
            UpdateHealth();
            ExpManager.Instance.UpdateUI();

            
            Debug.Log("Max Health: " + StatsManager.Instance.maxHealth);
            Debug.Log("Current Health: " + StatsManager.Instance.currentHealth);

            if(ExpManager.Instance.statPoint <= 0 && LevelUpPanel.activeSelf){
                LevelUpPanel.SetActive(false);
            }
        }
    }
}
