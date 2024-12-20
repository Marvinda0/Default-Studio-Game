using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ExpManager : MonoBehaviour
{
    public static ExpManager Instance;

    public GameObject levelUpTextPrefab;

    private GameObject levelUpPromptInstance;

    public int level;
    public int statPoint;
    public float currentExp;
    public float expToLevelUp = 6.52f; //jch6 the amount of experience needed to level up
    public float expGrowthMultiplier = 1.79f;//jch6 increase amount of experience needed to level up after experience meets the full amount.
    public Slider expSlider; //jch6 UI slider for exp gain. 
    public TMP_Text currentLevelText;
    public TMP_Text statPointText;

    private void Awake(){
        if(Instance == null){
            Instance = this;

        } else {
            Destroy(gameObject);
        }
    }

   private void Start(){
    UpdateUI();
   }
    private void Update(){
        if(Input.GetKeyDown(KeyCode.Return)){ //jch6 This is just to test if exp gain is working
           GainExperience(2);//jch6 Change for enemy xp
        }
    }
    public void GainExperience(int amount){ //jch6 player gain x amount experience becomes a level up
        currentExp += amount;
        if(currentExp >= expToLevelUp){ 
            LevelUp();
        }
        UpdateUI();
    }

    public void LevelUp()
    {
        if (levelUpTextPrefab != null)
        {
            Canvas uiCanvas = GameObject.Find("UIPopUpCanvas").GetComponent<Canvas>();
            levelUpPromptInstance = Instantiate(levelUpTextPrefab, uiCanvas.transform);

            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                Vector3 screenPos = Camera.main.WorldToScreenPoint(player.transform.position + new Vector3(0, 0.2f, 0));
                levelUpPromptInstance.transform.position = screenPos;
                levelUpPromptInstance.SetActive(true);
            }
        }
        statPoint += 4;
        level++;
        currentExp = Mathf.Min(currentExp - expToLevelUp, expToLevelUp - 1); // Cap carryover
        expToLevelUp = Mathf.Round(expToLevelUp * expGrowthMultiplier); // Preserve precision
        StatsManager.Instance.currentHealth = StatsManager.Instance.maxHealth;
        UpdateUI();
        StatsUI.Instance.ShowLevelUpPanel();
        Debug.Log($"LEVEL UP! New Level: {level}, Next Level EXP: {expToLevelUp}");
    }


    private void OnEnable (){
        HealthSystem.OnMonsterDeath += GainExperience;
    }

    private void OnDisable (){
        HealthSystem.OnMonsterDeath -= GainExperience;
    }

    public void UpdateUI(){//jch6 updates the UI for experience gain and when leveling up
        expSlider.maxValue = expToLevelUp;
        expSlider.value = currentExp;
        currentLevelText.text = "Level: " + level;
        statPointText.text = "Stat Points: " + statPoint;
    }
}
