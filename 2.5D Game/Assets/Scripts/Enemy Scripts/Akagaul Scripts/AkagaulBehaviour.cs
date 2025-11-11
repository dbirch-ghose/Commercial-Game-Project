using UnityEngine;

public class AkagaulBehaviour : MonoBehaviour
{
    public GameObject projectilePrefab;
    public Transform firePoint; //projectile spawnpoint
    public Transform player; //target
    public float fireAngle = 45f; //height of arc
    public float fireRate = 2f;

    private float nextFireTime;

    void Update()
    {
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
