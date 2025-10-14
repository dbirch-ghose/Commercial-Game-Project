using UnityEngine;

public class PlayerMovement8D : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private Transform model;  // The visual part of your player (child object)

    public float moveSpeed = 5f;
    public float rotationSpeed = 10f;

    private Vector2 inputDirection;

    void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        inputDirection = new Vector2(horizontal, vertical).normalized;
    }

    void FixedUpdate()
    {
        Vector3 moveDir = new Vector3(inputDirection.x, 0f, inputDirection.y);

        rb.linearVelocity = new Vector3(moveDir.x * moveSpeed, rb.linearVelocity.y, moveDir.z * moveSpeed);

        if (moveDir.sqrMagnitude > 0.01f) // only rotate when moving
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDir);
            model.rotation = Quaternion.Lerp(model.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }
}
