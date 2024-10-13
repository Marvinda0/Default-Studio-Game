using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class swordAttack : MonoBehaviour
{
    public enum AttackDirection
    {
        down, up
    }
    public AttackDirection attackDirection;
    Vector2 downAttackOffset;
    Collider2D swordCollider;

    private void Start()
    {
        swordCollider = GetComponent<Collider2D>();
        downAttackOffset = transform.position;
    }

    public void Attack()
    {
        switch(attackDirection)
        {
            case AttackDirection.down:
                attackDown(); break; 
        }
    }

    public void attackDown() {
        swordCollider.enabled = true;
        transform.position = downAttackOffset;
    }
   
    //public void attackUp;
    //public void attackRight;
    //public void attackLeft;
    public void StopAttack() {
        swordCollider.enabled = false;
    }
}
