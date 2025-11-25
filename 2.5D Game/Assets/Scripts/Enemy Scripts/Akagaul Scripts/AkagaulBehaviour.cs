//using System;
using System.Collections;
using UnityEngine;

public class AkagaulBehaviour : MonoBehaviour
{
    //-------------------ATTACK-LOGIC--------------------
    private bool attackFinished = true;    


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
    //public float spawnZ = 2f;       // where along Y to spawn (center)
    public float targetPos;
    
    // where to despawn
    public float leftBound = -20f;  
    public float rightBound = 30f;

    public float rightSpawnX = 20f; //right side spawn
    public float leftSpawnX = -20f; //left side spawn
    
    public float moveSpeed = 12f;

  



    private void Start()
    {
        //sets up health
        if (enemyTakeDamage != null)
        {
            health = enemyTakeDamage.health;
        }

        StartCoroutine(AttackLoop());
    }

    private IEnumerator AttackLoop()
    {
        while (true) 
        {
            if (attackFinished == true)
            {
                int randAttack = UnityEngine.Random.Range(0, 3); //possible attacks
                attackFinished = false; //stops the loop from running everyframe
                if (randAttack == 0)
                {
                    Debug.Log("projectile");
                    //StartCoroutine(LaunchProjectile());
                    yield return StartCoroutine(LaunchProjectile());

                }
                else if (randAttack == 1)
                {
                    Debug.Log("horse");
                    yield return StartCoroutine(SpawnHorse());
                }
                else if (randAttack == 2)
                {
                    Debug.Log("charge");
                    //function to be added
                    attackFinished = true;
                }

            }
            yield return null;
        }
    }

    private IEnumerator LaunchProjectile()
    {
        attackFinished = false;
        int projectileCount = 0;

        while (projectileCount < 3) //only throws 3 at a time
        {
            //creates projectile at the fire point
            GameObject projectile = Instantiate(projectilePrefab, firePoint.position, Quaternion.Euler(0, 0, Random.Range(-70f, 70)));
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

            yield return new WaitForSeconds(1f); //time between shots

            projectileCount++;
        }
        attackFinished = true; //ends the loop
    }

    private IEnumerator SpawnHorse()
    {
        attackFinished = false;
        int horseCount= 0;

        while (horseCount < 2)
        {
            //targetPos = Random.Range(-15f, 15f); //random spawn height
            targetPos = player.position.z; //gets the players z axis position

            bool spawnLeft = Random.value > 0.5f; //randomly choose left or right spawn each time                

            Vector3 spawnPos = spawnLeft ? new Vector3(leftSpawnX, 1.5f, targetPos) : new Vector3(rightSpawnX, 1.5f, targetPos);

            //Vector3 spawnPos = new Vector3(rightSpawnX, 1.5f, targetPos); //spawn horse on the right

            GameObject horse = Instantiate(Horse, spawnPos, Quaternion.identity);

            horse.GetComponent<HorseBehaviour>().SetDirection(spawnLeft ? 1 : -1); //chooses movement direction based of the random spawn location

            yield return new WaitForSeconds(2f); // duration of attack
            horseCount ++;
        }
        attackFinished = true;  
    }


    void Update()
    {
        //if player is close + previous attacks have finished
        //cane melee attack, then move position

        //choose randomly between 3 attacks
        //if random 1
        //-----------------PROJECTILE------------------
        //if (Time.time >= nextFireTime) //controls when the next projectile is thrown
        //{
        //    LaunchProjectile();
        //    nextFireTime = Time.time + 1f / fireRate;
        //}
        //if random 2
        //SpawnHorse();
        //if random 3
        //charge player
        Die();
    } 

    void Die() 
    {
        //object is destroyed in enemyTakeDamage script
        if (health <= 0)
        {
            //play death anim and and sound
            Debug.Log("boss is dead");
        }        
    }

}
