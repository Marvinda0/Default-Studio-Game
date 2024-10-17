using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ExpManager : MonoBehaviour
{
    public int level;
    public int statPoint;
    public int currentExp;
    public int expToLevelUp = 10; //jch6 the amount of experience needed to level up
    public float expGrowthMultiplier = 1.2f;//jch6 increase amount of experience needed to level up after experience meets the full amount.
    public Slider expSlider; //jch6 UI slider for exp gain. 
    public TMP_Text currentLevelText;
    //public TMP_Text statPointText; this is for stat point ui update


   private void Start(){
    UpdateUI();
   }
   // private void Update(){
        //if(Input.GetKeyDown(KeyCode.Return)){ //jch6 This is just to test if exp gain is working
          //  GainExperience(2);//jch6 Change for enemy xp
       // }
   // }
    public void GainExperience(int amount){ //jch6 player gain x amount experience becomes a level up
        currentExp += amount;
        if(currentExp >= expToLevelUp){ 
            LevelUp();
        }
        UpdateUI();
    }
    
    private void LevelUp(){ //jch6 For when a player levels up. Can add a graphic or soundeffect for levelup
        statPoint += 2; //jch6 gain 2 statpoints after every level up
        level++;
        currentExp -= expToLevelUp;
        expToLevelUp = Mathf.RoundToInt(expToLevelUp * expGrowthMultiplier);
    }

    private void OnEnable (){
        AIHealthSystem.OnMonsterDeath += GainExperience;
    }

    private void OnDisable (){
        AIHealthSystem.OnMonsterDeath -= GainExperience;
    }

    public void UpdateUI(){//jch6 updates the UI for experience gain and when leveling up
        expSlider.maxValue = expToLevelUp;
        expSlider.value = currentExp;
        currentLevelText.text = "Level: " + level;
    }
}
