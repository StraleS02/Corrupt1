using UnityEngine;
using UnityEngine.InputSystem;

public class MouseMovementX : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public float mouseSensitivity = 20f;

    float xRotation = 0f;
    //float yRotation = 0f;

    private InputSystem_Actions controls;

    //[SerializeField] public Transform playerBody;

    private void Awake()
    {
        controls = new InputSystem_Actions(); // inicijalizuj input actions
    }

    private void OnEnable()
    {
        controls.Player.Look.Enable();
    }

    private void OnDisable()
    {
        controls.Player.Look.Disable();
    }

    void Start()
    {
        //Locking the cursor to the middle of the screen and making it invisible
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        Vector2 mouseInput = controls.Player.Look.ReadValue<Vector2>();

        //float mouseX = mouseInput.x * mouseSensitivity * Time.deltaTime;
        float mouseY = mouseInput.y * mouseSensitivity * Time.deltaTime;

        //control rotation around x axis (Look up and down)
        xRotation -= mouseY;

        //we clamp the rotation so we cant Over-rotate (like in real life)
        xRotation = Mathf.Clamp(xRotation, -89f, 89f);

        //control rotation around y axis (Look up and down)
        //yRotation += mouseX;

        //applying both rotations
        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

    }
}
