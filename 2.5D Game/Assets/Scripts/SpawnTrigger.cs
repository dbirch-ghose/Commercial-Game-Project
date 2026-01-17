using UnityEngine;
using Fusion;

public class SpawnTrigger : NetworkBehaviour
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
        enemySpawner.SpawnEnemyAtPoint(5, 0);
        enemySpawner.SpawnEnemyAtPoint(6, 0);
        Debug.Log("Enemy spawned on State Authority");
    }


    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    public void RPC_RequestSpawnX(int i , int x)
    {
        enemySpawner.SpawnEnemyAtPoint(i, x);
        Debug.Log("Enemy spawned on State Authority");

    }
}