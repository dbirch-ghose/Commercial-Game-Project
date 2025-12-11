using Fusion;
using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UIElements;

public class AkagaulBehaviour : NetworkBehaviour
{
    public BasicSpawner spawner;
    public Transform player1;
    public Transform player2;
    public Transform closestPlayer; //target


    //-------------------ATTACK-LOGIC--------------------
    [Networked] public bool attackFinished { get; set; }
    [Networked] public bool isAttacking { get; set; }


    //-------------------HEALTH--------------------
    public EnemyTakeDamage enemyTakeDamage; //access health script
    [Networked] public int health { get; set; }

    //-------------------MELEE----------------------
    //public MeleeDamage meleeDamage;
    private Collider meleeCollider;
    public Transform meleePoint;
    public LayerMask playerLayers;
    public float attackRange = 1f;
    public float reposTime = 0.4f;
    public float ReposSpeed = 10f;


    //-----------------PROJECTILE------------------
    public NetworkObject projectilePrefab;
    public Transform firePoint; //projectile spawnpoint
    public float fireAngle = 45f; //height of arc
    public float fireRate = 2f;


    //-------------------HORSE----------------------
    public NetworkObject Horse;
    public NetworkObject Horse2;
    //public float spawnZ = 2f;       // where along Y to spawn (center)
    public float targetPos;
    // where to despawn, THESE BOUNDS MUST BE CHANGED TO FIT THE SIZE OF THE ROOM
    //public float leftBound = -20f;  
    //public float rightBound = 30f;
    public float rightSpawnX = 7f; //right side spawn
    public float leftSpawnX = -21f; //left side spawn
    //public float moveSpeed = 12f;

    //-------------------CHARGE----------------------
    public float chargeTime = 1f; //time spent charging
    public float chargeSpeed = 15f;
    public float currentSpeed = 0f;
    public float acceleration = 5f;
    public float rotationSpeed = 5f;
    private bool hasHitPlayer = false;


    public BasicSpawner basicSpawner;
    public Animator animator;
    public SpriteRenderer sr;

    public float moveX;
    public float moveY;
    public float speed;
    private Vector3 lastPos;


    public override void Spawned()
    {
       animator = GetComponent<Animator>();
       sr = GetComponent<SpriteRenderer>();

        meleeCollider = GetComponent<Collider>();
        if (meleeCollider != null)
            meleeCollider.enabled = false;
    }

    public void StartAttackLoop()
    {
        isAttacking = false;
        if (!Object.HasStateAuthority) return;
        if (!isAttacking)
        {
            StartCoroutine(AttackLoop());
            Debug.Log("starting attack loop");

        }
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


        if (Object.HasStateAuthority && basicSpawner.players.Count < 2)
        {
            StartCoroutine(WaitForPlayer()); //waits for player ref
        }


        if (player1 == null) //check for player 
        {
            Debug.Log("waiting for player to be assigned");
            return;
        }
        Debug.Log(closestPlayer); 
        
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

       


        //sets up health
        if (enemyTakeDamage != null)
        {
            health = enemyTakeDamage.health;
        }
        //to control when the attacks begins


        //if (Input.GetKeyDown(KeyCode.Space))
        //{
        //    //Debug.Log("charge");
        //    StartCoroutine(Charge());
        //}


        //animator controller
        //access speed
        Vector3 vel = (transform.position - lastPos) / Runner.DeltaTime;
        lastPos = transform.position;


        //sets direction
        moveX = vel.x;
        moveY = vel.z;  // Z = forward/back in 3D world
        speed = vel.magnitude;

        //flips sprite
        if (moveX > 0.1f) sr.flipX = false;
        if (moveX < -0.1f) sr.flipX = true;

        bool isWalking = speed > 0.1f;
        animator.SetBool("isWalking", isWalking);

        animator.SetFloat("moveX", moveX);
        animator.SetFloat("moveY", moveY);


        //update melee point pos every frame
        Vector3 direction = (closestPlayer.position - meleePoint.position).normalized;
        // Keep rotation only on the Y axis so it doesn't tilt up/down
        direction.y = 0;
        if (direction != Vector3.zero)
        {
            meleePoint.position = transform.position + direction;
        }

        Die();
    }


    private void OnCollisionEnter(Collision other)
    {
        if (!Object.HasStateAuthority) return;

        if (other.gameObject.CompareTag("Player"))
        {
            hasHitPlayer = true;
            //Debug.Log("Player has been hit");
        }
    }

    private bool CheckWallOverlap()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, 0.2f);
        foreach (var h in hits)
        {
            if (h.CompareTag("Wall"))
            {
                Debug.Log("hit wall");
                return true;
            }
        }
        return false;
    }


    public IEnumerator AttackLoop()
    {
        if (!Object.HasStateAuthority) yield break;

        if (Object.HasStateAuthority)
        {
            attackFinished = true;
            //Debug.Log("Attacks have started");
            isAttacking = true; //marks that the attacks have started

            while (true)
            {
                if (closestPlayer == null)
                {
                    yield return null;
                    Debug.Log("there is no player");
                    continue;
                }

                //Melee Logic --- Can melee during other attacks
                float distance = Vector3.Distance(closestPlayer.transform.position, transform.position); //calculates distance from the player
                if (distance <= 2f)
                {
                    //Debug.Log("player is close enough to be hit");
                    yield return StartCoroutine(Melee());
                    yield return StartCoroutine(Reposition());
                }

                if (attackFinished == true) //only start a new attack when ready
                {
                    //attackloop logic
                    attackFinished = false; //stops the loop from running everyframe

                    int randAttack = UnityEngine.Random.Range(0, 3); //possible 
                    if (randAttack == 0)
                    {
                        //Debug.Log("projectile");
                        yield return StartCoroutine(LaunchProjectile());
                    }
                    else if (randAttack == 1)
                    {
                        //Debug.Log("horse");
                        yield return StartCoroutine(SpawnHorse());
                    }
                    else if (randAttack == 2)
                    {
                        //Debug.Log("charge");
                        yield return StartCoroutine(Charge());
                    }
                }
                yield return null;
            }
        }
    }


    IEnumerator LaunchProjectile()
    {
        attackFinished = false;
        int projectileCount = 0;

        while (projectileCount < 3) //only throws 3 at a time
        {
            animator.SetBool("isThrowing", true);
            //creates projectile at the fire point
            NetworkObject projectile = Runner.Spawn(projectilePrefab, firePoint.position, Quaternion.Euler(0, 0, Random.Range(-70f, 70)), Object.InputAuthority);
            Rigidbody rb = projectile.GetComponent<Rigidbody>();

            //calculates direction and distance to the player
            Vector3 targetPos = closestPlayer.position;
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
            animator.SetBool("isThrowing", false);
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
            targetPos = closestPlayer.position.z; //gets the players z axis position

            bool spawnLeft = Random.value > 0.5f; //randomly choose left or right spawn each time                

            Vector3 spawnPos = spawnLeft ? new Vector3(leftSpawnX, 1.5f, targetPos) : new Vector3(rightSpawnX, 1.5f, targetPos);

            //Vector3 spawnPos = new Vector3(rightSpawnX, 1.5f, targetPos); //spawn horse on the right

            NetworkObject horse = Runner.Spawn(Horse, spawnPos, Quaternion.identity);
            //NetworkObject horse2 = Runner.Spawn(Horse2, spawnPos, Quaternion.identity);
            horse.GetComponent<HorseBehaviour>().SetDirection(spawnLeft ? 1 : -1); //chooses movement direction based of the random spawn location

            //int randHorse = UnityEngine.Random.Range(0, 2);  
            //if (randHorse == 0)
            //{
            //    horse.GetComponent<HorseBehaviour>().SetDirection(spawnLeft ? 1 : -1); //chooses movement direction based of the random spawn location
            //}
            //else if (randHorse == 1) 
            //{
            //    horse2.GetComponent<HorseBehaviour>().SetDirection(spawnLeft ? 1 : -1); //chooses movement direction based of the random spawn location

            //}
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
        float startTime = 0.1f; //time where collider isn't active

        float lockedY = transform.position.y; //stores starting y position

        //calculate direction and rotation outside of the loop so it doesnt home mid charge
        Vector3 direction = closestPlayer.transform.position - transform.position;

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
                //Debug.Log("Charge was interrupted by player");
                timer = chargeTime;
                attackFinished = true;
            }

           if (CheckWallOverlap() && attackStart ){
                //Debug.Log("Charge was interrupted by wall");
                //ends the attack
                timer = chargeTime;
                attackFinished = true;
           }

            currentSpeed += acceleration * Time.deltaTime;
            currentSpeed = Mathf.Min(currentSpeed, chargeSpeed);

            transform.Translate(direction * currentSpeed * Time.deltaTime, Space.World);
            //transform.Translate(direction * Time.deltaTime * chargeSpeed); //move the enemy at a specified speed

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
        //Debug.Log("melee");
        animator.SetBool("isSlashing", true);

        if (meleeCollider != null)
        meleeCollider.enabled = true;
 

        Collider[] hitPlayers = Physics.OverlapSphere(meleePoint.position, attackRange, playerLayers);
        foreach (Collider player in hitPlayers)
        {
            // Apply damage only if the enemy has a Networked health component
            PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();

            if (playerHealth != null && closestPlayer != null)
            {
                playerHealth.TakeDamage(1); // or whatever damage value
            }
        }

        yield return new WaitForSeconds(1f); // duration of attack
        if (meleeCollider != null)
        meleeCollider.enabled = false;
        attackFinished = true;
        animator.SetBool("isSlashing", false);

    }

    private void OnDrawGizmosSelected()
    {
        if (meleePoint != null)
            Gizmos.DrawSphere(meleePoint.position, attackRange);
    }



    private IEnumerator Reposition()
    {
        attackFinished = false;

        //Debug.Log("repositioning");
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
                //Debug.Log("reposition was interrupted by wall");
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

    void Die() 
    {
        //object is destroyed in enemyTakeDamage script
        if (health <= 0)
        {
            //play death anim and and sound
            //Debug.Log("boss is dead");
        }        
    }

}
