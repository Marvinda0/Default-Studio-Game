using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public static MenuManager Instance;
    private GameObject currentMenu;

    private void Awake(){
        if(Instance == null){
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else{
            Destroy(gameObject);
        }
    }

    public void OpenMenu(GameObject newMenu){
        if(currentMenu != null && currentMenu != newMenu){
            currentMenu.SetActive(false);
        }

        if(currentMenu == newMenu){//jch6 toggle the menu off if already active
            currentMenu.SetActive(false); 
            currentMenu = null;
            Time.timeScale = 1;
        }
        else {
            currentMenu = newMenu; //jch6 activate new menu
            currentMenu.SetActive(true);
            Time.timeScale = 0;
        }
    }

    public void CloseCurrentMenu(){
        if(currentMenu != null){
            currentMenu.SetActive(false);
            currentMenu = null;
            Time.timeScale = 1;
        }
    }

    public void ResetMenuState(){
        if(currentMenu != null){
            currentMenu.SetActive(false);
            currentMenu = null;
        }
        Time.timeScale = 1;
    }
}
