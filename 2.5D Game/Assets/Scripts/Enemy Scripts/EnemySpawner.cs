using UnityEngine;
using Fusion;

public class EnemySpawner : NetworkBehaviour
{
    public Transform[] spawnPoints;
    public NetworkPrefabRef[] enemyPrefabs;

    public override void Spawned()
    {
        if (!Object.HasStateAuthority)
            return;

        Debug.Log("EnemySpawner active on State Authority");
    }

    public void SpawnEnemyAtPoint(int enemyType, int spawnPointIndex)
    {
        if (!Object.HasStateAuthority)
            return;

        if (enemyType < 0 || enemyType >= enemyPrefabs.Length)
        {
            Debug.LogError("Invalid enemyType");
            return;
        }

        if (spawnPointIndex < 0 || spawnPointIndex >= spawnPoints.Length)
        {
            Debug.LogError("Invalid spawnPointIndex");
            return;
        }

        Transform spawnPoint = spawnPoints[spawnPointIndex];

        Runner.Spawn(
            enemyPrefabs[enemyType], spawnPoint.position, spawnPoint.rotation);

        Debug.Log($"Spawning enemy {enemyType} at point {spawnPointIndex}");
    }
}
