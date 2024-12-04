using UnityEngine;

public class PersistentObject : MonoBehaviour
{
    private static PersistentObject instance;

    void Awake()
    {
        // Check if an instance already exists
        if (instance != null && instance != this)
        {
            Destroy(gameObject); // Destroy duplicate object
            return;
        }

        // Set the instance and make it persistent
        instance = this;
        DontDestroyOnLoad(gameObject);
    }
}