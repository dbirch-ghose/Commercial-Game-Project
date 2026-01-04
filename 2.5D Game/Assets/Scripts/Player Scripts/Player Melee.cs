using System.Collections;
using UnityEngine;
using Fusion;

public class PlayerMelee : NetworkBehaviour
{
    public Transform attackPoint;
    public float attackRange = 1f;
    public LayerMask enemyLayers;
    public float attackDuration = 0.6f;
    public int damage = 2;

    private bool isAttacking;

    public Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        // Only the player with input authority handles input
        if (!Object.HasInputAuthority)
            return;

        if (Input.GetKeyDown(KeyCode.Space) && !isAttacking)
        {
            StartCoroutine(Attack());
        }
    }

    private IEnumerator Attack()
    {
        isAttacking = true;

        //if (animator.GetBool("isWalkingSide"))
        //{
            animator.SetBool("isHitting", true);
        //}
        //else if (animator.GetBool("isWalkingDown"))
        //{
        //    animator.SetBool("isHittingDown", true);
        //}
        //else if (animator.GetBool("isWalkingUp"))
        //{
        //    animator.SetBool("isHittingUp", true);

        //}




        // Client-side hit detection
        Collider[] hitEnemies = Physics.OverlapSphere(
                attackPoint.position,
                attackRange,
                enemyLayers
            );

        foreach (Collider enemy in hitEnemies)
        {
            EnemyTakeDamage etd = enemy.GetComponent<EnemyTakeDamage>();
            if (etd != null)
            {
                // Call RPC directly on the enemy
                etd.RPC_TakeDamage(damage);
            }
        }

        yield return new WaitForSeconds(attackDuration);

        animator.SetBool("isHitting", false);
        isAttacking = false;
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint != null)
            Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
