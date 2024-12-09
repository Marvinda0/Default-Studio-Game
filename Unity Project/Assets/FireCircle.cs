using UnityEngine;

public class FireCircle : MonoBehaviour
{
    public float damage = 10f;  
    public float damageInterval = 1f; 
    private float nextDamageTime;
    public float lifetime = 10f;

    private void Start()
    {
        Destroy(gameObject, lifetime); 
    }
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player") && Time.time >= nextDamageTime)
        {
            HealthSystem playerHealth = other.GetComponent<HealthSystem>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
                nextDamageTime = Time.time + damageInterval;
                Debug.Log("Player damaged by fire circle!");
            }
        }
    }
}
