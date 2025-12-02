//using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.ShaderGraph.Internal.KeywordDependentCollection;

public class AkagaulBehaviour : MonoBehaviour
{
    //-------------------ATTACK-LOGIC--------------------
    private bool attackFinished = true;
    private bool isAttacking = false;
    private int attackCount;

    //-------------------HEALTH--------------------
    public EnemyTakeDamage enemyTakeDamage; //access health script
    public int health;

    //-------------------MELEE----------------------
    public bool hasMeleed = false;
    public bool hasRepositioned = false;
    public MeleeDamage meleeDamage;
    public GameObject cane;
    public float reposTime = 0.4f;
    public float ReposSpeed = 10f;
    

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
    // where to despawn, THESE BOUNDS MUST BE CHANGED TO FIT THE SIZE OF THE ROOM
    //public float leftBound = -20f;  
    //public float rightBound = 30f;
    public float rightSpawnX = 7f; //right side spawn
    public float leftSpawnX = -21f; //left side spawn
                                    //public float moveSpeed = 12f;

    //-------------------CHARGE----------------------
    //public float warmUpTime = 1f;
    public float chargeTime = 1f; //time spent charging
    public float chargeSpeed = 15f;
    public float rotationSpeed = 5f;
    private bool hasHitPlayer = false;
    private bool hasHitWall = false;

    private void Start()
    {
        //sets up health
        if (enemyTakeDamage != null)
        {
            health = enemyTakeDamage.health;
        }

        player = GameObject.FindGameObjectWithTag("Player").transform; //assigns player to the player transforms

    }

    private IEnumerator AttackLoop()
    {
        isAttacking = true; //marks that the attacks have started

        while (true)
        {
            //Melee Logic --- Can melee during other attacks
            float distance = Vector3.Distance(player.transform.position, transform.position); //calculates distance from the player
            if (distance <= 2f)
            {
                Debug.Log("player is close enough to be hit");
                yield return StartCoroutine(Melee());
                yield return StartCoroutine(Reposition());
            }

            //if (attackFinished == true) //only start a new attack when ready
            //{
            //    //attackloop logic
            //    attackFinished = false; //stops the loop from running everyframe

            //    int randAttack = UnityEngine.Random.Range(0, 3); //possible 
            //    if (randAttack == 0)
            //    {
            //        //Debug.Log("projectile");
            //        yield return StartCoroutine(LaunchProjectile());
            //    }
            //    else if (randAttack == 1)
            //    {
            //        //Debug.Log("horse");
            //        yield return StartCoroutine(SpawnHorse());
            //    }
            //    else if (randAttack == 2)
            //    {
            //        //Debug.Log("charge");
            //        yield return StartCoroutine(Charge());
            //    }
            //}
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

    private IEnumerator Charge()
    {
        attackFinished = false;
        hasHitPlayer = false;

        bool attackStart = false;
        float startTime = 0.1f; //time where collider isn't checked

        float lockedY = transform.position.y; //stores starting y position

        //calculate direction and rotation outside of the loop so it doesnt home mid charge
        Vector3 direction = player.transform.position - transform.position;

        //fallback to prevent no movement when the player and boss line  up
        if (direction.sqrMagnitude < 0.01f)
        {
            direction = transform.up;
        }
        direction = direction.normalized;

        Quaternion rotation = Quaternion.LookRotation(Vector3.forward, direction);
        rotation *= Quaternion.Euler(0f, 0f, 180f);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, rotationSpeed * Time.deltaTime);
        //}
        float timer = 0f;
        while (timer < chargeTime)
        {
            if(timer > startTime)//after start time
            {
                attackStart = true;
            }

            if (hasHitPlayer) //stops attack when it hits the player or a wall
            {
                Debug.Log("Charge was interrupted by player");
                timer = chargeTime;
                attackFinished = true;
            }

           if (CheckWallOverlap() && attackStart ){
                Debug.Log("Charge was interrupted by wall");
                //ends the attack
                timer = chargeTime;
                attackFinished = true;
           }

            //float distance = Vector3.Distance(player.transform.position, transform.position); //calculates distance from the player, used to decide what to do

            transform.Translate(direction * Time.deltaTime * chargeSpeed); //move the enemy at a specified speed

            //locks y position
            Vector3 pos = transform.position;
            pos.y = lockedY;
            transform.position = pos;

            timer += Time.deltaTime;
            yield return null; //runs every frame 
        }
            attackFinished = true;
        }

    private IEnumerator Melee()
    {
        attackFinished = false;
        Debug.Log("melee");
        //meleeDamage.MeleeAnim();
        //collider logic handled in melee damage script
        yield return new WaitForSeconds(1f); // duration of attack
        attackFinished = true;
    }

    private IEnumerator Reposition()
    {
        attackFinished = false;

        Debug.Log("repositioning");
        float lockedY = transform.position.y; //stores starting y position

        //create a random direction vector
        //Vector3 randDirection = new Vector3(Random.Range(-5.0f, 5.0f), 0, Random.Range(-5.0f, 5.0f)).normalized;

        float radius = 2f;
        Vector2 ring = Random.insideUnitCircle.normalized * radius; //random value normalised so its always on the outside
        Vector3 randomPos = new Vector3(ring.x, transform.position.y, ring.y);
         
        float timer = 0f;
        while (timer < reposTime)
        {
            if (CheckWallOverlap()) //if it hits a wall, repos again
            {
                Debug.Log("reposition was interrupted by wall");
                //yield return StartCoroutine(Reposition());
            }
            else
            {
                transform.Translate(randomPos * Time.deltaTime * ReposSpeed); //moves to a random pos

                //locks y position
                Vector3 pos = transform.position;
                pos.y = lockedY;
                transform.position = pos;
            }
            timer += Time.deltaTime;
            yield return null; // wait 1 frame

        }

        

        yield return new WaitForSeconds(1f); // duration of attack
        attackFinished = true; //reset attack
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            hasHitPlayer = true;
            Debug.Log("Player has been hit");
        }
    }

    private bool CheckWallOverlap()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, 0.2f);
        foreach(var h in hits)
        {
            if (h.CompareTag("Wall"))
            {
                Debug.Log("hit wall");
                return true;
            }
        }
        return false;
    }

    void Update()
    {
        //to control when the attacks begin
        if (!isAttacking)
        {
            StartCoroutine(AttackLoop());
        }

        //if (Input.GetKeyDown(KeyCode.Space))
        //{
        //    //Debug.Log("charge");
        //    StartCoroutine(Charge());
        //}

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
