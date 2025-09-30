using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private float horizontal;
    private float vertical;
    private float horizontalSpeed = 5f;
    private float verticalSpeed = 8.5f;
    private float jumpForce = 5f;
    private bool isFacingRight = true;

    [SerializeField] private Rigidbody rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;

    void Start()
    {

    }

    void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal"); //calculates which way the player is facing   
        vertical = Input.GetAxisRaw("Vertical");

        if (Input.GetButtonDown("Jump") && IsGrounded())
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }

        Flip();
    }

    private void FixedUpdate()
    {
        //rb.linearVelocity = new Vector2(horizontal * speed, rb.linearVelocity.y);
        //rb.linearVelocity = new Vector2(vertical* speed, rb.linearVelocity.y);

        rb.linearVelocity = new Vector3(horizontal * horizontalSpeed, rb.linearVelocity.y, vertical * verticalSpeed);

    }

    private bool IsGrounded() //allows the player to jump when the ground object and ground collide
    {
        return Physics.CheckSphere(groundCheck.position, 0.2f, groundLayer);

    }

    private void Flip() //flips player sprite however this wont work with animations
    {
        if (isFacingRight && horizontal < 0f || !isFacingRight && horizontal > 0f)
        {
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }
}
