using UnityEngine;
using Fusion;

public class InfirmarySpawner : NetworkBehaviour
{
    public Transform spawnPoint;
    public NetworkPrefabRef enemyPrefab1;
    public NetworkPrefabRef enemyPrefab2;
    private bool hasTriggered = false;
    
    public override void Spawned()
    {
        if (!Object.HasStateAuthority)
            return;

        Debug.Log("EnemySpawner active on State Authority");
    }

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
            SpawnEnemy();
            Debug.Log("Enemy spawned on State Authority");
        }

    public void SpawnEnemy()
    {
        if (!Object.HasStateAuthority) return;

        Runner.Spawn(enemyPrefab1, spawnPoint.position, spawnPoint.rotation);
        Runner.Spawn(enemyPrefab2, spawnPoint.position, spawnPoint.rotation);
    }
}
