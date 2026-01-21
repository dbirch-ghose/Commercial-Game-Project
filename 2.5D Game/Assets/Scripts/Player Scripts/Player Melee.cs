using System.Collections;
using UnityEngine;
using Fusion;

public class PlayerMelee : NetworkBehaviour
{
    public Transform attackPoint;
    public float attackRange = 1f;
    public LayerMask enemyLayers;
    public float attackDuration = 0.6f;
    public float attackHitDelay = 0.3f;
    public int damage = 2;

    public bool isAttacking;
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
            if (!Object.HasInputAuthority)
                return;
            StartCoroutine(Attack());
        }
    }

    private IEnumerator Attack()
    {

        if (!Object.HasInputAuthority)
            yield return null;

        isAttacking = true;

        RPC_playAttackAnim();

        if (Mathf.Abs(brother.LastMoveDir.x) > 0.01f)
        {
            brother.sr.flipX = brother.LastMoveDir.x > 0; // adjust depending on default sprite
        }

        yield return new WaitForSeconds(attackHitDelay); // hit frame delay before applying damage

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

        yield return new WaitForSeconds(attackDuration - attackHitDelay);

        isAttacking = false;
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint != null)
            Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }


    [Rpc(RpcSources.All, RpcTargets.All)]
    public void RPC_playAttackAnim()
    {
        animator.SetFloat("MoveX", brother.LastMoveDir.x);
        animator.SetFloat("MoveY", brother.LastMoveDir.y);
        animator.SetTrigger("Attack");
    }
}
