using UnityEngine;

public class MobStatsManager : MonoBehaviour
{
    public static MobStatsManager Instance;

    private float initialHealthMultiplier;
    private float initialDamageMultiplier;

    [Header("Global Stats Scaling")]
    public float healthIncrement = 10f;
    public float damageIncrement = 5f;

    [Header("Current Global Stats")]
    public float globalHealthMultiplier = 1f;
    public float globalDamageMultiplier = 1f;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Persist across scenes
            initialHealthMultiplier = globalHealthMultiplier;
            initialDamageMultiplier = globalDamageMultiplier;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ScaleStats()
    {
        globalHealthMultiplier += healthIncrement / 100f; 
        globalDamageMultiplier += damageIncrement / 100f; 
        Debug.Log($"Stats Scaled: Health x{globalHealthMultiplier}, Damage x{globalDamageMultiplier}");
    }
    public void ResetStats()
    {
        globalHealthMultiplier = initialHealthMultiplier;
        globalDamageMultiplier = initialDamageMultiplier;
        Debug.Log("Mob stats reset to initial values.");
    }
}
