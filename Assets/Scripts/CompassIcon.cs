using UnityEngine;

public class CompassIcon : MonoBehaviour
{
    public static Transform player;           // Igrac (kamera ili player objekt)
    public static Transform objective;        // Cilj ka kome pokazuje ikonica
    public RectTransform compassIcon;  // UI ikonica koja se pomera levo/desno
    private readonly float compassWidth = 2600f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (!Stage.compassCheck)
        {
            GameObject door = GameObject.Find("DoorArea");
            GameObject player1 = GameObject.Find("Player");
            objective = door.transform;
            player = player1.transform;

            Stage.compassCheck = true;
        }

        Stage.updatedPoint = objective;
    }

    // Update is called once per frame
    void Update()
    {
        if (compassIcon != null && player != null && objective != null)
        {
            // pravac ka cilju
            Vector3 directionToTarget = objective.position - player.position;

            // 🔥 ignoriši visinu
            directionToTarget.y = 0f;

            // forward bez pitch-a
            Vector3 forward = player.forward;
            forward.y = 0f;

            directionToTarget.Normalize();
            forward.Normalize();

            // ugao levo/desno
            float angle = Vector3.SignedAngle(forward, directionToTarget, Vector3.up);

            // pretvaranje ugla u UI poziciju
            float xPos = (angle / 180f) * compassWidth;

            // 🔥 clamp umesto if-a
            xPos = Mathf.Clamp(xPos, -460f, 460f);

            compassIcon.anchoredPosition =
                new Vector2(xPos, compassIcon.anchoredPosition.y);
        }
    }
}
