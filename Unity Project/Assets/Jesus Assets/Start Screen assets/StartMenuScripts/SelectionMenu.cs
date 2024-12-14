using UnityEngine;
using UnityEngine.SceneManagement;

public class SelectionScript : MonoBehaviour
{
    public GameObject PlayerSword;
    public GameObject PlayerSpear; 

    public void SelectSword()
    {
        GameManager.Instance.SelectedPlayerPrefab = PlayerSword;
        PlayGame();
    }

    public void SelectSpear()
    {
        GameManager.Instance.SelectedPlayerPrefab = PlayerSpear; 
        PlayGame();
    }

    public void PlayGame()
    {
        SceneManager.LoadScene("Room 1.0");
    }

    public void QuitGame()
    {
        Debug.Log("QuitGame");
        Application.Quit();
    }
}
