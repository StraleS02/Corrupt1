using UnityEngine;

public class CarController : MonoBehaviour
{
    public float acceleration = 10000f;
    public float steering = 45f;
    public float maxSpeed = 50f;

    public Transform steeringWheel;

    public float maxWheelAngle = 60f;
    public float steeringSmoothness = 5f;

    private Rigidbody rb;

    float moveInput;
    float steerInput;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.centerOfMass = new Vector3(0, 0, 0); // ni�e te�i�te za stabilnost
    }

    void Update()
    {
        moveInput = Input.GetAxis("Vertical");   // W/S ili strelice gore/dole
        steerInput = Input.GetAxis("Horizontal"); // A/D ili strelice levo/desno
    }

    void FixedUpdate()
    {
        // Ograni?i maksimalnu brzinu
        if (rb.linearVelocity.magnitude < maxSpeed)
        {
            rb.AddForce(transform.forward * moveInput * acceleration * Time.fixedDeltaTime);
        }

        // Rotacija automobila (samo ako se kre?e)
        if (rb.linearVelocity.magnitude > 0.5f)
        {
            float forwardVelocity =
                Vector3.Dot(rb.linearVelocity, transform.forward);

            float speedPercent =
                rb.linearVelocity.magnitude / maxSpeed;

                        // napravi "peak" u sredini brzine
            float steeringMultiplier =
                Mathf.Sin(speedPercent * Mathf.PI);

                        // minimalni steering da ne bude 0
            steeringMultiplier =
                0.45f + Mathf.Sin(speedPercent * Mathf.PI) * 0.55f;

            float turn =
                steerInput *
                steering *
                steeringMultiplier *
                Time.fixedDeltaTime *
                Mathf.Sign(forwardVelocity);

            rb.MoveRotation(rb.rotation * Quaternion.Euler(0, turn, 0));

            // Anti-slip: smanji lateralnu (bočnu) brzinu da deluje kao da ima grip
            Vector3 localVelocity = transform.InverseTransformDirection(rb.linearVelocity);
            localVelocity.x *= 0.1f; // smanji bočnu brzinu (što je bliže 0, to više "grip")
            rb.linearVelocity = transform.TransformDirection(localVelocity);
        }

        float wheelMultiplier =
                Mathf.Lerp(1f, 0.5f,
                rb.linearVelocity.magnitude / maxSpeed);

        float targetAngle =
            -steerInput *
            maxWheelAngle *
            wheelMultiplier;

        Quaternion targetRotation =
            Quaternion.Euler(
                0f,
                0f,
                targetAngle
            );

        steeringWheel.localRotation =
            Quaternion.Lerp(
                steeringWheel.localRotation,
                targetRotation,
                steeringSmoothness * Time.deltaTime
            );
    }
}
