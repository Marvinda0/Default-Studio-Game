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

    public void EnableSwordCollider()
    {
        swordCollider.enabled = true;
        Debug.Log("Sword Collider enabled");
    }

    public void DisableSwordCollider()
    {
        swordCollider.enabled = false;
        Debug.Log("Sword Collider disabled");
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        HealthSystem enemyHealth = collision.GetComponent<HealthSystem>();
        if (enemyHealth != null && enemyHealth.isEnemy)
        {
            enemyHealth.TakeDamage(swordDamage);

            Rigidbody2D enemyRb = collision.GetComponent<Rigidbody2D>();
            if (enemyRb != null)
            {
                Vector2 knockbackDirection = (collision.transform.position - transform.position).normalized;
                enemyRb.AddForce(knockbackDirection * knockbackForce, ForceMode2D.Impulse);
            }
            Debug.Log("SwordHitbox triggered on " + collision.name);
        }
    }

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

