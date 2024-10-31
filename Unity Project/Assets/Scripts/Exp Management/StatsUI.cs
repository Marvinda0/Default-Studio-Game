using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StatsUI : MonoBehaviour
{

    public GameObject[] statsSlots;
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
            UpdateAllStats();
            StatsPanel.SetActive(false);
            menuActivated = false;
        }

        else if(!menuActivated && Input.GetKeyDown(KeyCode.U)){
            
            Time.timeScale = 0; //jch6 stops game when Inventory is opened
            UpdateAllStats();
            StatsPanel.SetActive(true);//jch6 Opens inventory menu
            menuActivated = true;
        }

    }

    public void UpdateDamage(){
        statsSlots[0].GetComponentInChildren<TMP_Text>().text = "Damage: " + StatsManager.Instance.damage;
    }

    public void UpdateSpeed(){
        statsSlots[0].GetComponentInChildren<TMP_Text>().text = "Speed: " + StatsManager.Instance.speed;
    }

    public void UpdateAllStats(){
        UpdateDamage();
        UpdateSpeed();
    }
}
