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

    //public float moveSpeed = 1250f;
    public float moveDrag = 15f;
    public float stopDrag = 25f;
    public bool canAttack = true;
    public string attackAnimName = "swordAttack";

    public Collider2D swordCollider; // Sword collider to be assigned in Inspector

    // Dash variables
    public float dashSpeed = 2500f; // Speed during dash
    public float dashDuration = 0.2f; // Time the dash lasts
    public float dashCooldown = 1f; // Time between dashes
    public string dashAnimName = "dash"; // Optional dash animation trigger

    private bool isDashing = false;
    private bool canDash = true;
    private bool isDead = false;

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
        if (isDead) return;

        if (!isDashing && canMove && moveInput != Vector2.zero)
        {
            //rb.AddForce(moveInput * moveSpeed * Time.fixedDeltaTime, ForceMode2D.Force);
            rb.AddForce(moveInput * StatsManager.Instance.speed * Time.fixedDeltaTime, ForceMode2D.Force);

            if (moveInput.x > 0) // Moving right
            {
                spriteRenderer.flipX = false;
                animator.SetBool("isFacingUp", false);
                animator.SetBool("isFacingDown", false);
                gameObject.BroadcastMessage("IsFacingRight", true);
            }
            else if (moveInput.x < 0) // Moving left
            {
                spriteRenderer.flipX = true;
                animator.SetBool("isFacingUp", false);
                animator.SetBool("isFacingDown", false);
                gameObject.BroadcastMessage("IsFacingRight", false);
            }
            else if (moveInput.y > 0) // Moving up
            {
                spriteRenderer.flipX = false; // No horizontal flip for vertical movement
                animator.SetBool("isFacingUp", true);
                animator.SetBool("isFacingDown", false);
                gameObject.BroadcastMessage("IsFacingUp", true);
            }
            else if (moveInput.y < 0) // Moving down
            {
                spriteRenderer.flipX = false; // No horizontal flip for vertical movement
                animator.SetBool("isFacingUp", false);
                animator.SetBool("isFacingDown", true);
                gameObject.BroadcastMessage("IsFacingUp", false);
            }
            else
            {
                animator.SetBool("isFacingUp", false);
                animator.SetBool("isFacingDown", false);
            }

            IsMoving = true;
        }
        else if (!isDashing)
        {
            IsMoving = false;
        }
    }

    public void Die()
    {
        isDead = true; // Set isDead to true when the player dies
        canMove = false; // Optionally lock other movement-related features
        rb.velocity = Vector2.zero; // Stop all movement
        animator.SetBool("isMoving", false);
    }

    void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    void OnFire()
    {
        if (canAttack && !isDashing)
        {
            animator.SetTrigger(attackAnimName);
        }
    }

    void OnDash()
    {
        if (canDash && moveInput != Vector2.zero) // Only dash if moving
        {
            StartCoroutine(Dash());
        }
    }

    private IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;
        canMove = false;

        // Optional: Trigger dash animation
        if (!string.IsNullOrEmpty(dashAnimName))
        {
            animator.SetTrigger(dashAnimName);
        }

        Vector2 dashDirection = moveInput.normalized;
        rb.velocity = dashDirection * dashSpeed * Time.fixedDeltaTime;

        yield return new WaitForSeconds(dashDuration);

        rb.velocity = Vector2.zero; // Stop dash
        isDashing = false;
        canMove = true;

        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
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