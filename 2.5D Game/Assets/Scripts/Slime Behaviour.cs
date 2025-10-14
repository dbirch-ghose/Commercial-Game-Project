using UnityEngine;
using UnityEngine.AI;

public class SlimeBehaviour : MonoBehaviour
{
    public NavMeshAgent agent;
    public float range;
    public Transform centerPoint;

    public Transform player;
    public bool isPatrolling = true;

    public float health = 1;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player").transform; //assigns player to the player transform

    }

    // Update is called once per frame
    void Update()
    {
        if (health <= 0)
        {
            Destroy(gameObject);
        }

        if (isPatrolling == true) //patrol is on by defualt
        {
            Patrol();
        }

        Chase();

        agent.updateRotation = false; //locks rotation
    }

    bool RandomPoint(Vector3 center, float range, out Vector3 result)
    {
        Vector3 randomPoint = center + Random.insideUnitSphere * range; //makes a random point in a sphere
        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomPoint, out hit, 1.0f, NavMesh.AllAreas))
        {
            result = hit.position;
            return true;
        }
        result = Vector3.zero;
        return false;

    }

    void Patrol()
    {
        if (agent.remainingDistance <= agent.stoppingDistance) 
        {
            Vector3 point;
            if (RandomPoint(centerPoint.position, range, out point)) 
            {
                Debug.DrawRay(point, Vector3.up, Color.blue, 1.0f);
                agent.SetDestination(point); //sets agent destination to the random point everytime it reaches it
            }
        }
    }

    void Chase()
    {
        float distance = Vector3.Distance(player.transform.position, transform.position);
        if (distance <= 5)
        {
            agent.SetDestination(player.position); //sets the agent destination to the player
            isPatrolling = false;
        }
        else if (distance > 5 )
        {
            isPatrolling = true;
        }
    }

    public void TakeDamage()
    {
        health -= 1;
    }
}
