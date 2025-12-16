using UnityEngine;
using Fusion;

public class SpawnTrigger : NetworkBehaviour
{
    public EnemySpawner enemySpawner;

    private void OnTriggerEnter(Collider other)
    {
        if (!Object.HasStateAuthority) return;

        if (other.CompareTag("Player"))
        {
            RPC_RequestSpawn();
            Debug.Log("player hit trigger wall");
        }

      
    }

    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    private void RPC_RequestSpawn()
    {
        enemySpawner.SpawnEnemyAtPoint(5, 0);
        Debug.Log("Enemy spawned on State Authority");
    }
}
