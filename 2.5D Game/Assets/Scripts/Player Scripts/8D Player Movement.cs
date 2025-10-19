using UnityEngine;

public class PlayerMovement8D : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private Transform attackHitBox;  

    public float moveSpeed = 5f;
    public float rotationSpeed = 10f;

    private Vector2 inputDirection;

    public Animator animator;
    public SpriteRenderer sr;

    private void Start()
    {
        animator = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        inputDirection = new Vector2(horizontal, vertical).normalized;
    }

    void FixedUpdate()
    {
        //sets input direction
        Vector3 moveDir = new Vector3(inputDirection.x, 0f, inputDirection.y);
        rb.linearVelocity = new Vector3(moveDir.x * moveSpeed, rb.linearVelocity.y, moveDir.z * moveSpeed);


        //hitbox rotation
        if (moveDir.sqrMagnitude > 0.01f) // only rotate when moving
        {
            //global space rotation
            //Quaternion targetRotation = Quaternion.LookRotation(moveDir);
            //attackHitBox.rotation = Quaternion.Lerp(attackHitBox.rotation, targetRotation, rotationSpeed * Time.deltaTime);

            //local space rotation
            Quaternion targetRotation = Quaternion.LookRotation(moveDir, Vector3.up);

            // Make the hitbox's local rotation match that, relative to the player's transform
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
}
