using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIMeleeDamage : MonoBehaviour
{
    public int damage = 10; // Placeholder damage value
    private bool isTouchingPlayer;
    public float damageCooldown = 1f; // 1 second cooldown between damage ticks
    private float lastDamageTime;

    void OnTriggerEnter2D(Collider2D other) // If inside 2D collider of mob, damage the player
    {
        if (other.gameObject.CompareTag("Player"))
        {
            isTouchingPlayer = true;
            // Start cooldown timer when first touching the player
            lastDamageTime = Time.time;
            //DamagePlayer(other.gameObject);
        }
    }

    void OnTriggerStay2D(Collider2D other) // Continuously check if touching player during overlap
    {
        if (isTouchingPlayer && other.gameObject.CompareTag("Player"))
        {
            // Check if cooldown time has passed before applying damage
            if (Time.time >= lastDamageTime + damageCooldown)
            {
                //DamagePlayer(other.gameObject);
                lastDamageTime = Time.time; // Reset cooldown timer after dealing damage
            }
        }
    }

    void OnTriggerExit2D(Collider2D other) // If player gets outside the 2D collider of mob, stop damaging
    {
        if (other.gameObject.CompareTag("Player"))
        {
            isTouchingPlayer = false;
        }
    }

    /*void DamagePlayer(GameObject player)
    {
        if (isTouchingPlayer)
        {
            player.GetComponent<PlayerHealth>().TakeDamage(damage);
        }
    }*/
}
    