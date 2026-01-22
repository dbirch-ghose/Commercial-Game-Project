using UnityEngine;using System.Collections;
using UnityEngine.AI;
using Fusion;

// Performance: Configure interpolation for responsive network gameplay
[OrderBefore(typeof(NetworkTransform))]
public class BoarBehaviour : NetworkBehaviour
{
    public NavMeshAgent agent;
    public float range;
    public Transform centerPoint;

    public Transform player;
    public bool isPatrolling = false;

    public Transform player1;
    public Transform player2;
    public Transform closestPlayer; //target

    public BasicSpawner basicSpawner;

    public SpriteRenderer sr;

    // Performance: Prevent coroutine spam
    private bool isWaitingForPlayer = false;


    public override void Spawned()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.enabled = Object.HasStateAuthority; //allows nav mesh to work with fusion
        //player = GameObject.FindGameObjectWithTag("Player").transform; //assigns player to the player transform
        sr = GetComponent<SpriteRenderer>();
    }

    IEnumerator WaitForPlayer()
    {
        while (basicSpawner.players.Count == 0)
        {
            yield return null;
        }

        //closestPlayer = basicSpawner.players[0].transform;

        player1 = basicSpawner.players[0].transform;

        if (basicSpawner.players.Count > 1)
        {
            player2 = basicSpawner.players[1].transform;
        }

        isWaitingForPlayer = false; // Reset flag when complete
    }

    public override void FixedUpdateNetwork()
    {

        if (Object.HasStateAuthority && !isWaitingForPlayer && player1 == null)
        {
            isWaitingForPlayer = true;
            StartCoroutine(WaitForPlayer()); //waits for player ref - only once
        }


        if (player1 == null) //check for player 
        {
#if UNITY_EDITOR
            Debug.Log("waiting for player to be assigned");
#endif
            return;
        }

        if (player2 == null)
        {
            closestPlayer = player1;
        }

        if (player2 != null)
        {
            // Performance: Use SqrMagnitude to avoid expensive sqrt calculation
            float p1SqrDistance = (player1.transform.position - transform.position).sqrMagnitude;
            float p2SqrDistance = (player2.transform.position - transform.position).sqrMagnitude;
            if (p1SqrDistance < p2SqrDistance)
            {
                closestPlayer = player1;
            }
            else if (p1SqrDistance > p2SqrDistance)
            {

                closestPlayer = player2;
            }
        }

        if (isPatrolling == true) //patrol is on by defualt
        {
            //Patrol();
        }

        Chase();

        agent.updateRotation = false; //locks rotation

        Vector3 velocity = agent.velocity;

        //flip sprite based on movement direction
        if (velocity.x > 0.1f)
            sr.flipX = false;
        else if (velocity.x < -0.1f)
            sr.flipX = true;

    }

    //bool RandomPoint(Vector3 center, float range, out Vector3 result)
    //{
    //    Vector3 randomPoint = center + Random.insideUnitSphere * range; //makes a random point in a sphere
    //    NavMeshHit hit;
    //    if (NavMesh.SamplePosition(randomPoint, out hit, 1.0f, NavMesh.AllAreas))
    //    {
    //        result = hit.position;
    //        return true;
    //    }
    //    result = Vector3.zero;
    //    return false;

    //}

    //void Patrol()
    //{
    //    if (agent.remainingDistance <= agent.stoppingDistance) 
    //    {
    //        Vector3 point;
    //        if (RandomPoint(centerPoint.position, range, out point)) 
    //        {
    //            Debug.DrawRay(point, Vector3.up, Color.blue, 1.0f);
    //            agent.SetDestination(point); //sets agent destination to the random point everytime it reaches it
    //        }
    //    }
    //}

    void Chase()
    {
        if (closestPlayer == null)
        {
#if UNITY_EDITOR
            Debug.Log("no close player for slime");
#endif
            return;
        }  //checks for player 

        // Performance: Use SqrMagnitude (5*5 = 25) instead of Distance to avoid sqrt
        float sqrDistance = (closestPlayer.transform.position - transform.position).sqrMagnitude;
        if (sqrDistance <= 25f) // 5 * 5
        {
            agent.SetDestination(closestPlayer.position); //sets the agent destination to the player
            isPatrolling = false;
        }
        else if (sqrDistance > 25f)
        {
            isPatrolling = true;
        }
    }


}
