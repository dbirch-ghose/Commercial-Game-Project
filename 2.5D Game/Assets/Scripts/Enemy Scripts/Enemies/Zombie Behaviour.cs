using UnityEngine;
using System.Collections;
using UnityEngine.AI;
using Fusion;


public class ZombieBehaviour : NetworkBehaviour
{
    public NavMeshAgent agent;
    public float range;
    //public Transform centerPoint;

    public Transform player;
    public bool isPatrolling = false;

    public Transform player1;
    public Transform player2;
    public Transform closestPlayer; //target

    public BasicSpawner basicSpawner;

    public SpriteRenderer sr;

 
    public override void Spawned()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.enabled = Object.HasStateAuthority; //allows nav mesh to work with fusion
        //player = GameObject.FindGameObjectWithTag("Player").transform; //assigns player to the player transform
        sr = GetComponent<SpriteRenderer>();

        basicSpawner = FindFirstObjectByType<BasicSpawner>();
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
    }

    public override void FixedUpdateNetwork()
    {

        if (Object.HasStateAuthority)
        {
            StartCoroutine(WaitForPlayer()); //waits for player ref
        }


        if (player1 == null) //check for player 
        {
            Debug.Log("waiting for player to be assigned");
            return;
        }

        if (player2 == null)
        {
            closestPlayer = player1;
        }

        if (player2 != null)
        {
            float p1Distance = Vector3.Distance(player1.transform.position, transform.position); //calculates distance from the player
            float p2Distance = Vector3.Distance(player2.transform.position, transform.position); //calculates distance from the player
            if (p1Distance < p2Distance)
            {
                closestPlayer = player1;
            }
            else if (p1Distance > p2Distance)
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
            Debug.Log("no close player for slime");
            return;
        }  //checks for player 

        float distance = Vector3.Distance(closestPlayer.transform.position, transform.position);
        if (distance <= 5)
        {
            agent.SetDestination(closestPlayer.position); //sets the agent destination to the player
            isPatrolling = false;
        }
        else if (distance > 5 )
        {
            isPatrolling = true;
        }
    }

    
}
