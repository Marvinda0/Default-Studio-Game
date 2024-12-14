using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; 

public class NewBehaviourScript : MonoBehaviour
{
    public void GoToSelectionScreen()
    {
        SceneManager.LoadScene("Selection Menu");
    }

    public void QuitGame()
    {
        Debug.Log("QuitGame");
        Application.Quit();
    }
}
