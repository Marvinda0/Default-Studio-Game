using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    bool IsMoving
    {
        set
        {
            isMoving = value;
            animator.SetBool("isMoving", isMoving);

            if (isMoving)
            {
                rb.drag = moveDrag;
            }
            else
            {
                rb.drag = stopDrag;
            }
        }
    }

    public float moveSpeed = 1250f;
    public float moveDrag = 15f;
    public float stopDrag = 25f;
    public bool canAttack = true;
    public string attackAnimName = "swordAttack";

    public Collider2D swordCollider; // Sword collider to be assigned in Inspector

    Rigidbody2D rb;
    Animator animator;
    SpriteRenderer spriteRenderer;

    Vector2 moveInput = Vector2.zero;
    bool isMoving = false;
    bool canMove = true;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator.SetBool("canAttack", canAttack);

        if (swordCollider == null)
        {
            Debug.LogWarning("Sword Collider not assigned in the Inspector.");
        }
        else
        {
            swordCollider.enabled = false; // Disable collider initially
        }
    }

    void FixedUpdate()
    {
        if (canMove && moveInput != Vector2.zero)
        {
            rb.AddForce(moveInput * moveSpeed * Time.fixedDeltaTime, ForceMode2D.Force);

            if (moveInput.x > 0)
            {
                spriteRenderer.flipX = false;
                gameObject.BroadcastMessage("IsFacingRight", true);
            }
            else if (moveInput.x < 0)
            {
                spriteRenderer.flipX = true;
                gameObject.BroadcastMessage("IsFacingRight", false);
            }

            IsMoving = true;
        }
        else
        {
            IsMoving = false;
        }
    }

    void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    void OnFire()
    {
        if (canAttack)
        {
            animator.SetTrigger(attackAnimName);
        }
    }

    // Called from animation event to enable sword collider at the start of the attack
    public void EnableSwordCollider()
    {
        if (swordCollider != null)
        {
            swordCollider.enabled = true;
        }
    }

    // Called from animation event to disable sword collider at the end of the attack
    public void DisableSwordCollider()
    {
        if (swordCollider != null)
        {
            swordCollider.enabled = false;
        }
    }

    void LockMovement()
    {
        canMove = false;
    }

    void UnlockMovement()
    {
        canMove = true;
    }
}
