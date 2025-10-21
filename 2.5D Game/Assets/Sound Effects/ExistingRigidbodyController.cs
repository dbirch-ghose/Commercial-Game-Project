using UnityEngine;

public class ExistingRigidbodyController : MonoBehaviour
{
    // ���е��ƶ�����
    private Rigidbody rb;
    private bool isGrounded;
    private bool isMoving;
    private bool isRunning;
    private Vector2 inputDirection;

    // �����ӵĽŲ���Ƶ
    public FootstepAudioSettings footstepSettings;
    private FootstepAudioManager footstepManager;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        // ��ʼ���Ų���Ƶ������
        footstepManager = gameObject.AddComponent<FootstepAudioManager>();
        footstepManager.settings = footstepSettings;
    }

    void Update()
    {
        // ���е�������
        GetInput();
        CheckGround();
        UpdateMovementState();

        // �����ӵ���Ƶ����
        if (footstepManager != null)
        {
            footstepManager.UpdateFootstepState(isGrounded, isMoving, isRunning);
            footstepManager.UpdateFootstepTimer();
            footstepManager.CheckLanding();
        }

        // ���е���Ծ����
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            Jump();
            if (footstepManager != null)
                footstepManager.PlayJumpSound();
        }
    }

    void FixedUpdate()
    {
        // ���е������ƶ��߼�
        HandleMovement();
    }

    // ���еķ������ֲ���
    void GetInput()
    {
        inputDirection = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        isRunning = Input.GetKey(KeyCode.LeftShift);
    }

    void CheckGround()
    {
        // ���еĵ������߼�
        RaycastHit hit;
        isGrounded = Physics.Raycast(transform.position, Vector3.down, out hit, 1.1f);
    }

    void UpdateMovementState()
    {
        Vector3 horizontalVelocity = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);
        isMoving = inputDirection.magnitude > 0.1f && isGrounded && horizontalVelocity.magnitude > 0.1f;
    }

    void HandleMovement()
    {
        // ���е��ƶ��߼�
        if (inputDirection.magnitude > 0.1f)
        {
            // �ƶ�ʵ��...
        }
    }

    void Jump()
    {
        // ���е���Ծ�߼�
        rb.AddForce(Vector3.up * 7f, ForceMode.Impulse);
    }
}