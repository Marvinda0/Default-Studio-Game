using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance; 

    public GameObject SelectedPlayerPrefab; 

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Persist across scenes
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
