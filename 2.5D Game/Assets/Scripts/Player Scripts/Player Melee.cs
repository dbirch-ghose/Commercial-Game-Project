using System.Collections;
using UnityEngine;
using Fusion;

public class PlayerMelee : NetworkBehaviour
{
    public Transform attackPoint;
    public float attackRange = 0.5f;
    public LayerMask enemyLayers;
    public float attackDuration = 0.2f;

    private bool isAttacking = false;

    private Collider meleeCollider;

    private void Start()
    {
        // Assumes the attack collider is attached to the same GameObject
        meleeCollider = GetComponent<Collider>();
        if (meleeCollider != null)
            meleeCollider.enabled = false;
    }

    private void Update()
    {
        // Only the authoritative player handles input for network-safe attacks
        if (!Object.HasInputAuthority) return;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(Attack());
        }
    }

    private IEnumerator Attack()
    {
        isAttacking = true;

        if (meleeCollider != null)
            meleeCollider.enabled = true;

        // Detect enemies immediately
        Collider[] hitEnemies = Physics.OverlapSphere(attackPoint.position, attackRange, enemyLayers);

        foreach (Collider enemy in hitEnemies)
        {
            // Apply damage only if the enemy has a Networked health component
            EnemyTakeDamage enemyTakeDamage = enemy.GetComponent<EnemyTakeDamage>();
            NetworkObject enemyNetObj = enemy.GetComponent<NetworkObject>();

            if (enemyTakeDamage != null && enemyNetObj != null && enemyNetObj.HasStateAuthority)
            {
                enemyTakeDamage.TakeDamage(2); // or whatever damage value
            }
        }

        yield return new WaitForSeconds(attackDuration);

        if (meleeCollider != null)
            meleeCollider.enabled = false;

        isAttacking = false;
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint != null)
            Gizmos.DrawSphere(attackPoint.position, attackRange);
    }
}
