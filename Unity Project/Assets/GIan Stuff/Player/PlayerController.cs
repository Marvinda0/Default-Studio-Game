using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private ZeusBoltController ZeusBoltController;

    private void OnEnable()
    {
        EquippedSlot.OnZeusThunderboltEquipped += HandleZeusBoltState;
    }

    private void OnDisable()
    {
        EquippedSlot.OnZeusThunderboltEquipped -= HandleZeusBoltState;
    }

    public Collider2D swordCollider;

    public float moveDrag = 15f;
    public float stopDrag = 25f;
    public bool canAttack = true;
    public string attackAnimName = "swordAttack";

    public float dashSpeed = 2500f;
    public float dashDuration = 0.2f;
    public float dashCooldown = 1f;
    public string dashAnimName = "dash";

    private bool isDashing = false;
    private bool canDash = true;
    private bool isDead = false;

    Rigidbody2D rb;
    Animator animator;
    SpriteRenderer spriteRenderer;

    Vector2 moveInput = Vector2.zero;
    bool canMove = true;

    void Start()
    {
        ZeusBoltController = GetComponent<ZeusBoltController>();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (swordCollider == null)
        {
            Debug.LogWarning("Sword Collider not assigned in the Inspector.");
        }
        else
        {
            swordCollider.enabled = false;
        }
    }

    private void HandleZeusBoltState(bool isEquipped)
    {
        if (ZeusBoltController == null) return;

        if (isEquipped)
        {
            ZeusBoltController.EquipZeusBolt();
        }
        else
        {
            ZeusBoltController.UnequipZeusBolt();
        }
    }

    void FixedUpdate()
    {
        if (isDead) return;

        if (!isDashing && canMove && moveInput != Vector2.zero)
        {
            rb.AddForce(moveInput * StatsManager.Instance.speed * Time.fixedDeltaTime, ForceMode2D.Force);

            if (moveInput.x > 0)
            {
                spriteRenderer.flipX = false;
            }
            else if (moveInput.x < 0)
            {
                spriteRenderer.flipX = true;
            }
        }
    }

    public void Die()
    {
        isDead = true;
        canMove = false;
        rb.velocity = Vector2.zero;
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
        if (canDash && moveInput != Vector2.zero)
        {
            StartCoroutine(Dash());
        }
    }

    private IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;
        canMove = false;

        if (!string.IsNullOrEmpty(dashAnimName))
        {
            animator.SetTrigger(dashAnimName);
        }

        Vector2 dashDirection = moveInput.normalized;
        rb.velocity = dashDirection * dashSpeed * Time.fixedDeltaTime;

        yield return new WaitForSeconds(dashDuration);

        rb.velocity = Vector2.zero;
        isDashing = false;
        canMove = true;

        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }

    public void EnableSwordCollider()
    {
        if (swordCollider != null)
        {
            swordCollider.enabled = true;
        }
    }

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
