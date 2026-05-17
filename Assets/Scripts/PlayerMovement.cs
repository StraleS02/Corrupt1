using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController controller;

    public float speed = 12f;
    public float gravity = -10f * 2;
    public float jumpHeight = 1.3f;
    public float sprintSpeed = 20f;

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    public Vector2 moveInput;
    public bool jumpInput;

    public Vector3 velocity;
    public bool isGrounded;
    private bool isSprinting = false;

    private Animator bobbing;

    void Start()
    {
        bobbing = HomeEnterTrigger.FindChildByName(transform.gameObject, "Main Camera").GetComponent<Animator>();
    }

    // Novi Input System koristi ove metode za pokret i skok
    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            jumpInput = true;
        }
    }

    public void OnSprint(InputAction.CallbackContext context)
    {
        isSprinting = context.ReadValueAsButton();
    }

    void Update()
    {
        isGrounded = controller.isGrounded;

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        Vector3 cameraForward = Camera.main.transform.forward;
        Vector3 cameraRight = Camera.main.transform.right;

        cameraForward.y = 0f;
        cameraRight.y = 0f;

        cameraForward.Normalize();
        cameraRight.Normalize();

        float currentSpeed = isSprinting ? sprintSpeed : speed;

        // Pomicanje igrača
        Vector3 move = cameraRight * moveInput.x + cameraForward * moveInput.y;
        bool isMoving = moveInput.magnitude > 0.1f && isGrounded;
        bobbing.enabled = isMoving;
        if (isSprinting)
        {
            bobbing.speed = 2.5f; // 50% brže
        }
        else
        {
            bobbing.speed = 1.5f;
        }
        controller.Move(currentSpeed * Time.deltaTime * move);

        // Skakanje
        if (jumpInput && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            jumpInput = false; // resetuj nakon skoka
        }

        // Gravitacija
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
}
