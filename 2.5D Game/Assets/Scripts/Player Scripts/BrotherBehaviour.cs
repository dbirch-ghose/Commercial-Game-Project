using Fusion;
using System.Collections;
using UnityEngine;

public class BrotherBehaviour : NetworkBehaviour
{
    //[SerializeField] private Rigidbody rb;
    [SerializeField] private Transform attackHitBox;

    public PlayerHealth playerHealth;

    public float moveSpeed = 5f;
    public float rotationSpeed = 10f;

    public PlayerMelee melee;

    private Vector2 inputDirection;
    public Vector2 lastMoveDir = Vector2.down;
    public Vector2 LastMoveDir => lastMoveDir; // read-only
    

    //private Vector3 lastMoveDir;
    private bool canDash = true;
    private bool isDashing;
    private float dashingPower = 10f;
    private float dashingTime = 0.2f;
    private float dashingCooldown = 1f;

    public Animator animator;
    public SpriteRenderer sr;
    public TrailRenderer tr;


    private NetworkCharacterController _cc;
    private Vector3 _forward = Vector3.forward;
    //private ChangeDetector _changeDetector;


    private void Awake()
    {
        _cc = GetComponent<NetworkCharacterController>();

    }

   
    public override void Spawned()
    {
        ////_changeDetector = GetChangeDetector(ChangeDetector.Source.SimulationState);
        //if (HasInputAuthority)
        //{
        //    camera = Camera.main;
        //    camera.GetComponent<CameraBehaviour>().target = transform;
        //}
        animator = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        if (playerHealth == null)
            playerHealth = GetComponent<PlayerHealth>();
    }

    public override void FixedUpdateNetwork()
    {
        

        if (!GetInput(out NetworkInputData data))
        {
            return;
        }
        if (melee.isAttacking)
        {
            if (Object.HasStateAuthority)
                _cc.Move(Vector3.zero);
            return; // stop movement during attack
        }

        //prevents movement when dead
        if (playerHealth.playerDead)
        {
            if (Object.HasInputAuthority)
            {
                animator.SetBool("isDead", true); //change sprite just for this player
            }
            if (Object.HasStateAuthority)
            {
                _cc.Move(Vector3.zero);
                _cc.Velocity = Vector3.zero;
            }
            return;
        }

        if (Object.HasStateAuthority)
        {
            Vector3 move = new Vector3(data.direction.x, 0, data.direction.z);
            _cc.Move(move * moveSpeed * Runner.DeltaTime);
        }
        if (!Object.HasInputAuthority)
            return;

        

        //blend tree animation controller
        Vector2 input = new Vector2(data.direction.x, data.direction.z);

        bool isMoving = input.sqrMagnitude > 0.01f;
        animator.SetBool("isMoving", isMoving);

        if (isMoving)
            lastMoveDir = input.normalized;

        animator.SetFloat("MoveX", lastMoveDir.x);
        animator.SetFloat("MoveY", lastMoveDir.y);

        if (lastMoveDir.x != 0)
            sr.flipX = lastMoveDir.x > 0;

       
        ////hitbox rotation
        if (data.direction.sqrMagnitude > 0.01f)
        {
            // Convert 2D input - 3D world direction
            Vector3 moveDir = new Vector3(data.direction.x, 0, data.direction.z);

            // Ensure it isn't a zero vector when moving on Z axis
            moveDir.Normalize();

            // Position hitbox in front of the player
            attackHitBox.position = transform.position + moveDir;

        }

       



        //prevents movement while dashing
        //if (isDashing)
        //{
        //    return;
        //}

        //transform.rotation = Quaternion.identity;

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
        //StartCoroutine(Dash());
        //}
    }

    //old movement
    //data.direction.Normalize();
    //_cc.Move(5 * data.direction * Runner.DeltaTime);

    //if (data.direction.sqrMagnitude > 0)
    //    _forward = data.direction;

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

    //private IEnumerator Dash()
    //{
    //    canDash = false;
    //    isDashing = true;
    //    rb.linearVelocity = _forward * dashingPower; //creates dash force
    //    tr.emitting = true;
    //    yield return new WaitForSeconds(dashingTime); //duration
    //    isDashing = false;
    //    tr.emitting = false;
    //    yield return new WaitForSeconds(dashingCooldown); //cooldown
    //    canDash = true;
    //}
}


