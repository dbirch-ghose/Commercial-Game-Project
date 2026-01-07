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
    public BrotherBehaviour brother;

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
        animator.SetFloat("MoveX", brother.lastMoveDir.x);
        animator.SetFloat("MoveY", brother.lastMoveDir.y);
        animator.SetTrigger("Attack");


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

        isAttacking = false;
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint != null)
            Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
