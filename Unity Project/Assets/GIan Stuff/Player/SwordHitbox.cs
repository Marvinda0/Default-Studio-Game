using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordHitbox : MonoBehaviour
{
    public float swordDamage = 1f;
    public float knockbackForce = 15f;
    public Collider2D swordCollider;
    public Vector3 faceRight = new Vector3(1, -0.9f, 0);
    public Vector3 faceLeft = new Vector3(-1, -0.9f, 0);

    void Start()
    {
        if (swordCollider == null)
        {
            Debug.LogWarning("Sword Collider not set");
        }
        else
        {
            swordCollider.enabled = false; // Disable collider initially
        }
    }

    // This method should be called from the animation events to enable/disable collider
    public void EnableSwordCollider()
    {
        swordCollider.enabled = true;
    }

    public void DisableSwordCollider()
    {
        swordCollider.enabled = false;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the sword hitbox collided with an enemy
        IDamageable enemyHealth = collision.GetComponent<IDamageable>();
        if (enemyHealth != null && collision.CompareTag("Enemy"))
        {
            // Apply damage to the enemy
            enemyHealth.OnHit(swordDamage);

            // Apply knockback if the enemy has a Rigidbody2D
            Rigidbody2D enemyRb = collision.GetComponent<Rigidbody2D>();
            if (enemyRb != null)
            {
                Vector2 knockbackDirection = (collision.transform.position - transform.position).normalized;
                enemyRb.AddForce(knockbackDirection * knockbackForce, ForceMode2D.Impulse);
            }
            Debug.Log("SwordHitbox triggered on " + collision.name);
        }
    }

    // Method to adjust sword hitbox position based on facing direction
    void IsFacingRight(bool isFacingRight)
    {
        if (isFacingRight)
        {
            gameObject.transform.localPosition = faceRight;
        }
        else
        {
            gameObject.transform.localPosition = faceLeft;
        }
    }
}