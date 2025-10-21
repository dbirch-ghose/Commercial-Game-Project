using System.Collections;
using UnityEngine;

public class PlayerMovement8D : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private Transform attackHitBox;


    public float moveSpeed = 5f;
    public float rotationSpeed = 10f;

    private Vector2 inputDirection;

    private Vector3 lastMoveDir;
    private bool canDash = true;
    private bool isDashing;
    private float dashingPower = 10f;
    private float dashingTime = 0.2f;
    private float dashingCooldown = 1f;

    public Animator animator;
    public SpriteRenderer sr;
    public TrailRenderer tr;

    private void Start()
    {
        animator = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        //prevents movement while dashing
        if(isDashing)
        {
            return;
        }


        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        inputDirection = new Vector2(horizontal, vertical).normalized;

        //dash controller
        Vector3 dashDir = new Vector3(horizontal, 0f, vertical);
        if (dashDir != Vector3.zero)
        {
            lastMoveDir = dashDir.normalized;
        }

        if (Input.GetKeyDown(KeyCode.E) && canDash)
        {
            StartCoroutine(Dash());
        }
    }

    void FixedUpdate()
    {
        //prevents movement while dashing
        if (isDashing)
        {
            return;
        }

        //sets input direction
        Vector3 moveDir = new Vector3(inputDirection.x, 0f, inputDirection.y);
        rb.linearVelocity = new Vector3(moveDir.x * moveSpeed, rb.linearVelocity.y, moveDir.z * moveSpeed);


        //hitbox rotation
        if (moveDir.sqrMagnitude > 0.01f) // only rotate when moving
        {
            //local space rotation
            Quaternion targetRotation = Quaternion.LookRotation(moveDir, Vector3.up);
            attackHitBox.rotation = Quaternion.Lerp(attackHitBox.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            // Keep the hitbox in front of the player 
            float distanceFromPlayer = 1f; 
            attackHitBox.position = transform.position + moveDir.normalized * distanceFromPlayer;
        }

        //sprite controller
        if (rb.linearVelocity.x < 0)
        {
           animator.SetBool("isWalking", true);
            sr.flipX = false;
        }
        else if(rb.linearVelocity.x > 0)
        {
            animator.SetBool("isWalking", true);
            sr.flipX = true;
        }
    }

    private IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;
        rb.linearVelocity = lastMoveDir * dashingPower; //creates dash force
        tr.emitting = true;
        yield return new WaitForSeconds(dashingTime); //duration
        isDashing = false;
        tr.emitting = false;
        yield return new WaitForSeconds(dashingCooldown); //cooldown
        canDash = true;
    }

}
