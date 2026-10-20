using UnityEngine;
using UnityEngine.InputSystem;

// Roll-a-Ball keyboard movement + jumping (uses the class Input System actions).
public class PlayerControl : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private float jumpForce = 5.5f;
    [SerializeField] private float maxSpeed = 8f;

    private InputAction moveAction;
    private InputAction jumpAction;
    private Rigidbody rb;
    private bool jumpQueued;
    private float speedMultiplier = 1f;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        moveAction = InputSystem.actions.FindAction("Move");
        jumpAction = InputSystem.actions.FindAction("Jump");

        if (moveAction == null || jumpAction == null)
            Debug.LogError("PlayerControl: Move/Jump input actions were not found.");

        rb.linearDamping = 1.25f;
    }

    private void Update()
    {
        if (moveAction == null || jumpAction == null) return;

        // Only jump when the sphere is on a platform, not repeatedly in the air.
        if (jumpAction.WasPressedThisFrame() && IsGrounded())
            jumpQueued = true;

        if (Keyboard.current != null && Keyboard.current.rKey.wasPressedThisFrame)
            GameManager.Instance.RespawnBall();
    }

    private void FixedUpdate()
    {
        if (moveAction == null || rb == null) return;

        if (jumpQueued)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            jumpQueued = false;
        }

        Vector2 moveValue = moveAction.ReadValue<Vector2>();
        Vector3 movement = new Vector3(moveValue.x, 0f, moveValue.y);
        rb.AddForce(movement * speed * speedMultiplier, ForceMode.Acceleration);

        // Keep rolling controllable while making the power-up noticeably faster.
        Vector3 velocity = rb.linearVelocity;
        Vector3 horizontal = new Vector3(velocity.x, 0f, velocity.z);
        float limit = maxSpeed * speedMultiplier;
        if (horizontal.magnitude > limit)
        {
            horizontal = horizontal.normalized * limit;
            rb.linearVelocity = new Vector3(horizontal.x, velocity.y, horizontal.z);
        }
    }

    private bool IsGrounded()
    {
        // Sphere radius is 0.5. Cast downward and ignore collectible triggers.
        if (Physics.SphereCast(transform.position, 0.31f, Vector3.down,
            out RaycastHit hit, 0.37f, ~0, QueryTriggerInteraction.Ignore))
        {
            return hit.collider.CompareTag("Floor");
        }
        return false;
    }

    public void SetSpeedMultiplier(float multiplier)
    {
        speedMultiplier = multiplier;
    }
}
