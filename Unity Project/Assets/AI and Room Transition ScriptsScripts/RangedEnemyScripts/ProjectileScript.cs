using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 10f;             // Speed of the projectile
    public float lifespan = 3f;           // Time in seconds before the projectile is destroyed
    public int damage = 10;               // Damage inflicted on the player

    private Vector2 direction;            // Direction to travel

    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, lifespan); // Destroy the projectile after its lifespan
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(direction * speed * Time.deltaTime);
    }

    // Sets the direction of the projectile
    public void SetDirection(Vector2 newDirection)
    {
        direction = newDirection.normalized;
    }

    // Handle collision with player
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Access player’s health script and apply damage if implemented
            // other.GetComponent<PlayerHealth>().TakeDamage(damage);

            Destroy(gameObject); // Destroy projectile on hit
        }
    }
}
