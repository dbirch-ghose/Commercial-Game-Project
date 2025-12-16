using UnityEngine;
using Fusion;

public class SpawnTrigger : NetworkBehaviour
{
    public EnemySpawner enemySpawner;
    private bool triggered;

    private void OnTriggerEnter(Collider other)
    {
        if (triggered) return;
        if (!other.CompareTag("Player")) return;

        triggered = true;
        RPC_RequestSpawn();
        Debug.Log("player hit trigger wall");
    }

    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    private void RPC_RequestSpawn()
    {
        enemySpawner.SpawnEnemyAtPoint(5, 0);
        Debug.Log("Enemy spawned on State Authority");
    }
}
