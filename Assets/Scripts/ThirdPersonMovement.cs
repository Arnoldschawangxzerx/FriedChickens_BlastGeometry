using UnityEngine;

public class ThirdPersonController : MonoBehaviour
{
    [Header("References")]
    public CharacterController controller;
    public Transform cameraTransform;
    public Transform groundCheck;
    public Animator animator;

    [Header("Movement")]
    public float walkSpeed = 4f;
    public float sprintSpeed = 7f;
    public float acceleration = 10f;
    public float rotationSmoothTime = 0.1f;
    public Animator animator1;

    [Header("Jumping")]
    public float gravity = -20f;
    public float jumpHeight = 2f;

    [Header("Ground Check")]
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    float rotationVelocity;
    float currentSpeed;
    Vector3 velocity;

    bool isGrounded;

    void Update()
    {
        GroundCheck();

        Move();

        Jump();

        ApplyGravity();

        UpdateAnimator();
    }

    void GroundCheck()
    {
        isGrounded = Physics.CheckSphere(
            groundCheck.position,
            groundDistance,
            groundMask
        );

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }
        animator1.SetBool("IsGrounded", isGrounded);
    }

    void Move()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        bool isSprinting = Input.GetKey(KeyCode.LeftShift);

        float targetSpeed = isSprinting ? sprintSpeed : walkSpeed;

        currentSpeed = Mathf.Lerp(
            currentSpeed,
            targetSpeed,
            acceleration * Time.deltaTime
        );

        if (direction.magnitude >= 0.1f)
        {
            float targetAngle =
                Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg
                + cameraTransform.eulerAngles.y;

            float angle = Mathf.SmoothDampAngle(
                transform.eulerAngles.y,
                targetAngle,
                ref rotationVelocity,
                rotationSmoothTime
            );

            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDir =
                Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;

            controller.Move(moveDir.normalized * currentSpeed * Time.deltaTime);
        }
    }

    void Jump()
    {
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);

            animator.SetTrigger("Jump");
        }
        animator1.SetTrigger("Jump");
    }

    void ApplyGravity()
    {
        velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);
    }

    void UpdateAnimator()
    {
        float moveAmount =
    new Vector2(
        Input.GetAxisRaw("Horizontal"),
        Input.GetAxisRaw("Vertical")
    ).magnitude;

        bool sprinting = Input.GetKey(KeyCode.LeftShift);

        float animationValue = sprinting ? 1f : 0.5f;

        animator1.SetFloat(
            "Speed",
            moveAmount * animationValue,
            0.1f,
            Time.deltaTime
        );

        animator1.SetBool("IsGrounded", isGrounded);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(groundCheck.position, groundDistance);
    }
}