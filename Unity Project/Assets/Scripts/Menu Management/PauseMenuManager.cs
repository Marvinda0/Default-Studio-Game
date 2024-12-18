using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenuManager : MonoBehaviour
{
    public static PauseMenuManager Instance;
    public GameObject PauseMenuPanel;
    public GameObject ControlsPanel;
    public GameObject QuestionPanel;
    public GameObject QuestionQuitPanel;

    private bool isPaused = false;
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        /*else
        {
            Destroy(gameObject);
            return;
        }*/
    }

    
    // Start is called before the first frame update
    void Start()
    {
        InitializeMenu();
        Time.timeScale = 1; // Ensure game starts unpaused
        isPaused = false;
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded; // Reinitialize on scene load
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        InitializeMenu(); // Reinitialize menu references after scene changes
    }

    private void InitializeMenu()
    {
        PauseMenuPanel = transform.Find("PauseMenuPanel")?.gameObject;
        ControlsPanel = transform.Find("ControlsPanel")?.gameObject;
        QuestionPanel = transform.Find("QuestionPanel")?.gameObject;
        QuestionQuitPanel = transform.Find("QuestionQuitPanel")?.gameObject;
        
        if (PauseMenuPanel == null || ControlsPanel == null || QuestionPanel == null || QuestionQuitPanel == null)
        {
            Debug.LogError("One or more panels are not assigned in the Inspector. Please check!");
            return;
        }

        // Deactivate all panels at the start
        PauseMenuPanel.SetActive(false);
        ControlsPanel.SetActive(false);
        QuestionPanel.SetActive(false);
        QuestionQuitPanel.SetActive(false);

        Debug.Log("PauseMenuManager initialized. All panels set to inactive.");
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.P)){
            Debug.Log("P key pressed. Attempting to open pause menu");
            MenuManager.Instance.OpenMenu(PauseMenuPanel);
            if(ControlsPanel.activeSelf || QuestionPanel.activeSelf || QuestionQuitPanel.activeSelf){
                ControlsPanel.SetActive(false);
                QuestionPanel.SetActive(false);
                QuestionQuitPanel.SetActive(false);
            }
        }
    }

    public void ShowControlsPanel(){
        bool isActive = ControlsPanel.activeSelf;
        ControlsPanel.SetActive(!isActive); // Toggle the panel
        Debug.Log("Controls Panel Toggled");

        if(QuestionPanel.activeSelf){
            QuestionPanel.SetActive(false);
        }
        if(QuestionQuitPanel.activeSelf){
            QuestionQuitPanel.SetActive(false);
        }

    }

    public void ShowQuestionPanel(){
        Debug.Log("Main Menu button clicked!");
        QuestionPanel.SetActive(true);
        if(ControlsPanel.activeSelf){
            ControlsPanel.SetActive(false);
        }
        if(QuestionQuitPanel.activeSelf){
            QuestionQuitPanel.SetActive(false);
        }

    }

    public void CloseQuestionPanel(){
        Debug.Log("Main Menu button clicked! Panel closed");
        QuestionPanel.SetActive(false);
    }

    public void ShowQuestionQuitPanel(){
        Debug.Log("Quit Game button clicked!");
        QuestionQuitPanel.SetActive(true);
        if(ControlsPanel.activeSelf){
            ControlsPanel.SetActive(false);
        }
        if(QuestionPanel.activeSelf){
            QuestionPanel.SetActive(false);
        }
    }

    public void CloseQuestionQuitPanel(){
        Debug.Log("Main Menu button clicked!");
        QuestionQuitPanel.SetActive(false);
    }

    public void QuitGame(){
        Debug.Log("Quit Game!");
        Application.Quit();
    }

    public void ToMainMenu(){
        Debug.Log("Returning to the main menu!");
        if (MenuManager.Instance != null)
        {
            MenuManager.Instance.ResetMenuState();
        }

        //MenuManager.Instance.ResetMenuState();
        PersistentObject.ResetPersistentObject();
        StatsManager.Instance.ResetStats();
        MobStatsManager.Instance.ResetStats();
        Destroy(gameObject);


        WaveSpawnnerScript waveSpawner = FindObjectOfType<WaveSpawnnerScript>();
        if (waveSpawner != null)
        {
            waveSpawner.ResetWaves();
        }

        Time.timeScale = 1;
        SceneManager.LoadScene("StartMenu");
    }

}
