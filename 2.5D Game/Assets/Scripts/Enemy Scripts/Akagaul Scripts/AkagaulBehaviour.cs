using UnityEngine;

public class AkagaulBehaviour : MonoBehaviour
{
    //-------------------HEALTH--------------------
    public EnemyTakeDamage enemyTakeDamage; //access health script
    public int health;

    //-----------------PROJECTILE------------------
    public GameObject projectilePrefab;
    public Transform firePoint; //projectile spawnpoint
    public Transform player; //target
    public float fireAngle = 45f; //height of arc
    public float fireRate = 2f;
    private float nextFireTime;

    //-------------------HORSE----------------------
    public GameObject Horse;
    public float spawnZ = 2f;       // where along Y to spawn (center)
    public float moveSpeed = 12f;    // how fast it runs
    public float leftBound = -20f;  // when to despawn
    public float rightSpawnX = 20f; // where to spawn



    private void Start()
    {
        if (enemyTakeDamage != null)
        {
            health = enemyTakeDamage.health;
        }

        SpawnHorse();

    }

    void SpawnHorse()
    {
        Vector3 spawnPos = new Vector3(rightSpawnX, 1.5f, spawnZ);
        Instantiate(Horse, spawnPos, Quaternion.identity);
    }



    void Update()
    {
        //-----------------PROJECTILE------------------
        if (Time.time >= nextFireTime) //controls when the next projectile is thrown
        {
            LaunchProjectile();
            nextFireTime = Time.time + 1f / fireRate;
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
