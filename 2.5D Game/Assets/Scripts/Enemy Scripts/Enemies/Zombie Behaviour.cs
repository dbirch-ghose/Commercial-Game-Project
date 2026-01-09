using UnityEngine;
using System.Collections;
using UnityEngine.AI;
using Fusion;


public class ZombieBehaviour : NetworkBehaviour
{
    public NavMeshAgent agent;

    public Transform player1;
    public Transform player2;
    public Transform closestPlayer;

    public BasicSpawner basicSpawner;
    public SpriteRenderer sr;

    private bool navReady = false;
    private bool waitingForPlayers = false;

    public override void Spawned()
    {
        if (!Object.HasStateAuthority)
            return;

        agent = GetComponent<NavMeshAgent>();
        agent.enabled = true;

        sr = GetComponent<SpriteRenderer>();
        basicSpawner = FindFirstObjectByType<BasicSpawner>();

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

    public override void FixedUpdateNetwork()
    {
        if (!Object.HasStateAuthority || !navReady)
            return;

        if (!waitingForPlayers && player1 == null)
        {
            waitingForPlayers = true;
            StartCoroutine(WaitForPlayer());
            return;
        }

        if (player1 == null)
            return;

        agent.updateRotation = false; //locks rotation


        UpdateClosestPlayer();
        Chase();
        UpdateSprite();
    }

    IEnumerator WaitForPlayer()
    {
        while (basicSpawner.players.Count == 0)
            yield return null;

        player1 = basicSpawner.players[0].transform;

        if (basicSpawner.players.Count > 1)
            player2 = basicSpawner.players[1].transform;
    }

    void UpdateClosestPlayer()
    {
        if (player2 == null)
        {
            closestPlayer = player1;
            return;
        }

        float d1 = Vector3.Distance(player1.position, transform.position);
        float d2 = Vector3.Distance(player2.position, transform.position);

        closestPlayer = d1 < d2 ? player1 : player2;
    }

    void Chase()
    {
        if (closestPlayer == null)
            return;

        float distance = Vector3.Distance(closestPlayer.position, transform.position);

        if (distance <= 5f)
        {
            agent.SetDestination(closestPlayer.position);
        }
    }

    void UpdateSprite()
    {
        Vector3 v = agent.velocity;

        if (v.x > 0.1f) sr.flipX = false;
        else if (v.x < -0.1f) sr.flipX = true;
    }
}
