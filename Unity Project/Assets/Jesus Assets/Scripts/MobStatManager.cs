using UnityEngine;

public class MobStatsManager : MonoBehaviour
{
    public static MobStatsManager Instance;

    private float initialHealthMultiplier;
    private float initialDamageMultiplier;

    [Header("Global Stats Scaling")]
    public float healthIncrement = 1.1f; // Flat health increment per scaling call
    public float damageIncrement = 0.1333f; // Flat damage increment per scaling call

    [Header("Current Global Stats")]
    public float globalHealthMultiplier = 1f; // Starting health multiplier
    public float globalDamageMultiplier = 1f; // Starting damage multiplier

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Persist across scenes

            // Store initial multipliers
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
        // Apply flat increments to global multipliers
        globalHealthMultiplier += healthIncrement;
        globalDamageMultiplier += damageIncrement;

        Debug.Log($"Stats Scaled: Health x{globalHealthMultiplier}, Damage x{globalDamageMultiplier}");
    }

    public void ResetStats()
    {
        // Reset multipliers to their initial values
        globalHealthMultiplier = initialHealthMultiplier;
        globalDamageMultiplier = initialDamageMultiplier;

        Debug.Log("Mob stats reset to initial values.");
    }
}