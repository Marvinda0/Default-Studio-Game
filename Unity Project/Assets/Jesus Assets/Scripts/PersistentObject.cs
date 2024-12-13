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

    public static void ResetPersistentObject()
    {
        if (instance != null)
        {
            Destroy(instance.gameObject); // Destroy the persistent object
            instance = null;
        }
    }
}