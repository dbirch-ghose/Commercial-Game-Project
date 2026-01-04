using Fusion;

using UnityEngine;

public class SisterBehaviour : NetworkBehaviour
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private Transform attackHitBox;


    public float moveSpeed = 5f;
    public float rotationSpeed = 10f;

    private Vector2 inputDirection;

    private Vector3 lastMoveDir;

    public Animator animator;
    public SpriteRenderer sr;

    public Camera camera;

    private NetworkCharacterController _cc;
    private Vector3 _forward = Vector3.forward;
    private ChangeDetector _changeDetector;

    private bool canPossess = false;
    private weakMind wm;
    private weakMind Twm;
    private GameObject enemy;
    private NetworkObject enemyNO;
    public NetworkObject thisDude;

    



    private void Awake()
    {
        _cc = GetComponent<NetworkCharacterController>();
        thisDude = GetComponent<NetworkObject>();
    }

    private void Start()
    {
        animator = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
    }

    public override void Spawned()
    {
        ////_changeDetector = GetChangeDetector(ChangeDetector.Source.SimulationState);
        //if (HasInputAuthority)
        //{
        //    camera = Camera.main;
        //    camera.GetComponent<CameraBehaviour>().target = transform;

        //}
    }


    public override void FixedUpdateNetwork()
    {
        if (GetInput(out NetworkInputData data))
        {
            //data.direction.Normalize();
            //_cc.Move(5 * data.direction * Runner.DeltaTime);

            //if (data.direction.sqrMagnitude > 0)
            //    _forward = data.direction;

            Vector3 move = new Vector3(data.direction.x, 0, data.direction.z);
            _cc.Move(move * moveSpeed * Runner.DeltaTime);

        }

        //sprite controller
        animator.SetBool("isIdle", false);
        animator.SetBool("isWalkingSide", false);
        animator.SetBool("isWalkingDown", false);
        animator.SetBool("isWalkingUp", false);

        if (data.direction.sqrMagnitude <= 0)
        {
            animator.SetBool("isIdle", true);
        }
        else
        {
            animator.SetBool("isIdle", false);
        }

        if (data.direction.x < 0) //left
        {
            //Debug.Log("Left");
            animator.SetBool("isWalkingSide", true);
            sr.flipX = false;
        }
        else if (data.direction.x > 0) //right
        {
            //Debug.Log("right");
            animator.SetBool("isWalkingSide", true);
            sr.flipX = true;
        }
        else if (data.direction.z < 0) //down
        {
            //Debug.Log("down");
            animator.SetBool("isWalkingDown", true);
        }
        else if (data.direction.z > 0) //up
        {
            //Debug.Log("up");
            animator.SetBool("isWalkingUp", true);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (wm == null)
            {
                Debug.Log("wm null");
            }
            if (wm != null)
            {
                Debug.Log("wm got something");
            }
            Debug.Log("Space Pressed");
            Debug.Log("State authority: " + HasStateAuthority);
            Debug.Log("canPossess: " + canPossess);
        }
        
            if (HasStateAuthority && canPossess == true && Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("SA+canPosess+PressSpace");
            BasicSpawner BS = FindFirstObjectByType<BasicSpawner>();
            NetworkPrefabRef creatureType = wm.creatureType;
            Vector3 spawnPoint = enemy.transform.position;
            BS.RPC_RequestDestroy(enemyNO);
            BS.WMSpawn(thisDude, creatureType, spawnPoint);
            canPossess = false;
        }

        transform.rotation = Quaternion.identity;
    }



    void OnTriggerEnter(Collider other)
    {
        // Check if the other object has the weakMind script
        
        Twm = other.gameObject.GetComponent<weakMind>();

        if (Twm != null)
        {
            wm = Twm;
            enemy = other.gameObject;
            enemyNO = enemy.GetComponent<NetworkObject>();
            // The object has the weakMind script
            Debug.Log("Collided with an object that has weakMind!");
        }
    }

    private void OnTriggerStay(Collider other)
    {
        Twm = other.gameObject.GetComponent<weakMind>();


        if (Twm != null)
        {
            wm = Twm;
            enemy = other.gameObject;
            enemyNO = enemy.GetComponent<NetworkObject>();
            // The object has the weakMind script
            Debug.Log("Collided with an object that has weakMind!");
            canPossess = true;
            Debug.Log("Can Possess!");
            

        }
        

    }
}

    //float horizontal = Input.GetAxisRaw("Horizontal");
    //float vertical = Input.GetAxisRaw("Vertical");
    //inputDirection = new Vector2(horizontal, vertical).normalized;

    ////dash controller
    //Vector3 dashDir = new Vector3(horizontal, 0f, vertical);
    //if (dashDir != Vector3.zero)
    //{
    //    lastMoveDir = dashDir.normalized;
    //}

    //if (Input.GetKeyDown(KeyCode.E) && canDash)
    //{
    //    StartCoroutine(Dash());
    //}




    //void Update()
    //{
    //    //prevents movement while dashing
    //    if(isDashing)
    //    {
    //        return;
    //    }


    //    float horizontal = Input.GetAxisRaw("Horizontal");
    //    float vertical = Input.GetAxisRaw("Vertical");
    //    inputDirection = new Vector2(horizontal, vertical).normalized;

    //    //dash controller
    //    Vector3 dashDir = new Vector3(horizontal, 0f, vertical);
    //    if (dashDir != Vector3.zero)
    //    {
    //        lastMoveDir = dashDir.normalized;
    //    }

    //    if (Input.GetKeyDown(KeyCode.E) && canDash)
    //    {
    //        StartCoroutine(Dash());
    //    }
    //}

    //void FixedUpdate()
    //{
    //    //prevents movement while dashing
    //    if (isDashing)
    //    {
    //        return;
    //    }

    //    //sets input direction
    //    Vector3 moveDir = new Vector3(inputDirection.x, 0f, inputDirection.y);
    //    rb.linearVelocity = new Vector3(moveDir.x * moveSpeed, rb.linearVelocity.y, moveDir.z * moveSpeed);


    //    //hitbox rotation
    //    if (moveDir.sqrMagnitude > 0.01f) // only rotate when moving
    //    {
    //        //local space rotation
    //        Quaternion targetRotation = Quaternion.LookRotation(moveDir, Vector3.up);
    //        attackHitBox.rotation = Quaternion.Lerp(attackHitBox.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    //        // Keep the hitbox in front of the player 
    //        float distanceFromPlayer = 1f; 
    //        attackHitBox.position = transform.position + moveDir.normalized * distanceFromPlayer;
    //    }

    //    //sprite controller
    //    if (rb.linearVelocity.x < 0)
    //    {
    //       animator.SetBool("isWalking", true);
    //        sr.flipX = false;
    //    }
    //    else if(rb.linearVelocity.x > 0)
    //    {
    //        animator.SetBool("isWalking", true);
    //        sr.flipX = true;
    //    }
    //}



