using UnityEngine;

public class RoomInitializer : MonoBehaviour
{
    public Transform PlayerSpawnPoint; 

    private void Start()
    {
        if (GameManager.Instance != null && GameManager.Instance.SelectedPlayerPrefab != null)
        {
            Instantiate(GameManager.Instance.SelectedPlayerPrefab, PlayerSpawnPoint.position, Quaternion.identity);
        }
        else
        {
            Debug.LogError("No player prefab selected in GameManager. Did you forget to select a character?");
        }
    }
}
