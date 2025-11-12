using UnityEngine;

public class AkagaulBehaviour : MonoBehaviour
{
    //-------------------HEALTH--------------------
    public int health = 20;


    //-----------------PROJECTILE------------------
    public GameObject projectilePrefab;
    public Transform firePoint; //projectile spawnpoint
    public Transform player; //target
    public float fireAngle = 45f; //height of arc
    public float fireRate = 2f;
    private float nextFireTime;

    //-------------------HORSE----------------------
    public GameObject Horse;  
    public float arenaWidth = 20f;  //width of arena
    public float arenaHeight = 10f; //height of arena
    private bool horseActive = false;


    void SpawnHorse()
    {
        Vector3 spawnPos = Vector3.zero;
        Vector3 targetPos = Vector3.zero;

        int side = Random.Range(0, 4); // 0=left, 1=right, 2=top, 3=bottom

        switch (side)
        {
            case 0: // Left
                spawnPos = new Vector3(-arenaWidth / 2, 0, player.position.z);
                targetPos = new Vector3(arenaWidth / 2, 0, player.position.z);
                break;
            case 1: // Right
                spawnPos = new Vector3(arenaWidth / 2, 0, player.position.z);
                targetPos = new Vector3(-arenaWidth / 2, 0, player.position.z);
                break;
            case 2: // Top
                spawnPos = new Vector3(player.position.x, 0, arenaHeight / 2);
                targetPos = new Vector3(player.position.x, 0, -arenaHeight / 2);
                break;
            case 3: // Bottom
                spawnPos = new Vector3(player.position.x, 0, -arenaHeight / 2);
                targetPos = new Vector3(player.position.x, 0, arenaHeight / 2);
                break;
        }

        GameObject enemy = Instantiate(Horse, spawnPos, Quaternion.identity);
        enemy.GetComponent<HorseBehaviour>().SetTarget(targetPos);
    }



    void Update()
    {
        //-----------------PROJECTILE------------------
        if (Time.time >= nextFireTime) //controls when the next projectile is thrown
        {
            LaunchProjectile();
            nextFireTime = Time.time + 1f / fireRate;
        }

        if (horseActive == false)
        {
            Debug.Log("the horse is here");
            SpawnHorse();   
            horseActive = true;
        }
        
    }

    void LaunchProjectile()
    {
        //creates projectile at the fire point
        GameObject projectile = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity); 
        Rigidbody rb = projectile.GetComponent<Rigidbody>();

        //calculates direction and distance to the player
        Vector3 targetPos = player.position;
        Vector3 direction = targetPos - firePoint.position;
        float yDiff = direction.y; //saves vertical diff seperately
        direction.y = 0; //allows direction to just be on x axis
        float distance = direction.magnitude; //gets distance to player
        float angleRad = fireAngle * Mathf.Deg2Rad; //converts fireangle to radians

        float gravity = Mathf.Abs(Physics.gravity.y);

        //calculates speed needed to hit the player
        float velocity = Mathf.Sqrt((distance * gravity) / (Mathf.Sin(2 * angleRad)));

        //calculate launch velocity for both x and y
        Vector3 velocityVector = direction.normalized * Mathf.Cos(angleRad) * velocity;
        velocityVector.y = Mathf.Sin(angleRad) * velocity;

        //fires projectile
        rb.linearVelocity = velocityVector;
    }
}
