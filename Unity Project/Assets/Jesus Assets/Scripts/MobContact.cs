using UnityEngine;

public class SlimeDamage : MonoBehaviour
{
    public float damageToPlayer = 10f;
    public float damageInterval = 1f; // Time in seconds between damage applications
    private float nextDamageTime = 0f; // Time tracking for next damage

    void OnCollisionStay2D(Collision2D collision)
    {
        // Check if the object in contact is the player and if enough time has passed for the next damage
        if (collision.gameObject.CompareTag("Player") && Time.time >= nextDamageTime)
        {
            HealthSystem playerHealth = collision.gameObject.GetComponent<HealthSystem>();
            if (playerHealth != null)
            {
                float scaledDamage = damageToPlayer * MobStatsManager.Instance.globalDamageMultiplier;
                playerHealth.TakeDamage(scaledDamage);
                nextDamageTime = Time.time + damageInterval; // Set the time for the next allowed damage
            }
        }
    }
}
