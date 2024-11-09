using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class playerController : MonoBehaviour
{
    public float moveSpeed = 1f;
    public float collissionOffset = 0.05f;
    public ContactFilter2D movementFilter;
    public swordAttack swordAttack;

    Vector2 movementInput;

    Rigidbody2D rb;

    Animator animator;

    List<RaycastHit2D> castCollisions = new List<RaycastHit2D>();
    // Start is called before the first frame update
    void Start()
    {
        rb=GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame


    private void FixedUpdate()
    {
        if (movementInput != Vector2.zero)
        {
            bool success = TryMove(movementInput);
            if (!success && movementInput.x > 0)
            {
                success = TryMove(new Vector2(movementInput.x, 0));
            }
            if (!success && movementInput.y > 0)
            {
                success = TryMove(new Vector2(0,movementInput.y));
            }
            animator.SetBool("is-moving", success);
        } else
        {
            animator.SetBool("is-moving", false);
        }
    }
    private bool TryMove(Vector2 direction)
    {
        // Cast to detect any obstacles in the direction of movement
        int count = rb.Cast(direction, movementFilter, castCollisions, moveSpeed * Time.fixedDeltaTime + collissionOffset);

        // Filter out collisions with enemies
        for (int i = castCollisions.Count - 1; i >= 0; i--)
        {
            if (castCollisions[i].collider.CompareTag("Enemy"))
            {
                castCollisions.RemoveAt(i);
            }
        }

        // If there are no remaining obstacles, move
        if (castCollisions.Count == 0)
        {
            rb.MovePosition(rb.position + direction * moveSpeed * Time.fixedDeltaTime);
            return true;
        }
        else
        {
            return false;
        }
    }

    void OnMove(InputValue movementValue) {
        movementInput = movementValue.Get<Vector2>();
    }

    void OnFire()
    {
        animator.SetTrigger("swordAttack");
    }

}
