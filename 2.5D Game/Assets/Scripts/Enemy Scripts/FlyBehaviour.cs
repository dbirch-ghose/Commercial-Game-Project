using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using Fusion;
public class FlyBehaviour : NetworkBehaviour
{
    public NavMeshAgent agent;
    private bool navReady = false;

    public override void Spawned()
    {
        if (!Object.HasStateAuthority)
            return;
        agent = GetComponent<NavMeshAgent>();
        agent.enabled = true;
        StartCoroutine(InitNavMesh());
    }

    IEnumerator InitNavMesh()
    {
        yield return null; //  wait one frame

        NavMeshHit hit;
        if (NavMesh.SamplePosition(transform.position, out hit, 2f, NavMesh.AllAreas))
        {
            agent.Warp(hit.position);
            navReady = true;
        }
    }
}
