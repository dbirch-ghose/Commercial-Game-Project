using UnityEngine;
using Fusion;

public class SpawnTrigger1 : NetworkBehaviour
{
    public EnemySpawner enemySpawner;
    public bool hasTriggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (!Object.HasStateAuthority) return;

        if (other.CompareTag("Player") && hasTriggered == false)
        {
            RPC_RequestSpawn();
            Debug.Log("player hit trigger wall");
            hasTriggered = true;
        }

      
    }

    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    private void RPC_RequestSpawn()
    {
        enemySpawner.SpawnEnemyAtPoint(1, 1);
        enemySpawner.SpawnEnemyAtPoint(2, 1);
        Debug.Log("Enemy spawned on State Authority");
    }


}