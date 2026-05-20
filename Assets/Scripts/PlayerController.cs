using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float walkSpeed = 3f;
    public float runSpeed = 6f;
    public float turnSpeed = 15f;

    private Rigidbody rb;
    private Animator anim;
    private Vector3 moveInput;
    private bool isRunning;
    public GameObject bombPrefab; // Assign your Bomb_bag model prefab here
    private GameObject currentTarget;
    void Start()
    {
        rb = GetComponent<Rigidbody>();

        // Change this from GetComponentInChildren to plain GetComponent
        anim = GetComponent<Animator>();

        // Safety Check
        if (anim == null)
        {
            Debug.LogError("ERROR: Animator component missing from this GameObject! Please add an Animator component directly to the parent Player object.", this);
        }
    }

    void Update()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveZ = Input.GetAxisRaw("Vertical");
        moveInput = new Vector3(moveX, 0f, moveZ).normalized;

        isRunning = Input.GetKey(KeyCode.LeftShift);

        float currentSpeed = moveInput.magnitude * (isRunning ? runSpeed : walkSpeed);

        // ONLY run this if anim actually exists so the game doesn't crash
        if (anim != null)
        {
            anim.SetFloat("Speed", currentSpeed);
        }
        if (Input.GetKeyDown(KeyCode.E) && currentTarget != null)
        {
            PlantBomb();
        }

        void PlantBomb()
        {
            Debug.Log("Bomb Planted! Get clear!");
            Instantiate(bombPrefab, currentTarget.transform.position + Vector3.up * 0.5f, Quaternion.identity);
            // Optional: Destroy or change target state
        }
    }

    void FixedUpdate()
    {
        // 3. Move Player
        float targetSpeed = isRunning ? runSpeed : walkSpeed;
        Vector3 velocity = moveInput * targetSpeed;
        rb.MovePosition(rb.position + velocity * Time.fixedDeltaTime);

        // 4. Rotate Player towards movement direction
        if (moveInput != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveInput);
            rb.rotation = Quaternion.Slerp(rb.rotation, targetRotation, turnSpeed * Time.fixedDeltaTime);
        }
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Target"))
        {
            currentTarget = other.gameObject;
            Debug.Log("Press E to plant explosive on target.");
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Target"))
        {
            currentTarget = null;
        }
    }
}