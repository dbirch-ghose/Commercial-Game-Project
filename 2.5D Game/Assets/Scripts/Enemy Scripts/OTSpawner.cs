using UnityEngine;
using Fusion;

public class OTSpawner : NetworkBehaviour
{
    public Transform spawnPoint;
    public NetworkPrefabRef enemyPrefab1;
    private bool hasTriggered = false;

    [Networked] private TickTimer spawnTimer { get; set; }
    public float spawnInterval = 20f;


    public override void Spawned()
    {
        if (!Object.HasStateAuthority)
            return;

        Debug.Log("EnemySpawner active on State Authority");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!Object.HasStateAuthority) return;
        if (!other.CompareTag("Player")) return;

        // If timer not running or has expired, allow spawn
        if (!spawnTimer.IsRunning || spawnTimer.Expired(Runner))
        {
            SpawnBoar();
            spawnTimer = TickTimer.CreateFromSeconds(Runner, spawnInterval);
        }
    }


    private void SpawnBoar()
    {

        RPC_RequestSpawn();
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
    }
}
