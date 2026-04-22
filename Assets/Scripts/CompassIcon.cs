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
        GameObject door = GameObject.Find("DoorArea");
        GameObject player1 = GameObject.Find("Player");
        objective = door.transform;
        player = player1.transform;
        Stage.updatedPoint = objective;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 directionToTarget = (objective.position - player.position).normalized;

        // Ugao između pogleda igrača i cilja
        float angle = Vector3.SignedAngle(player.forward, directionToTarget, Vector3.up);

        // Normalizuj ugao na -180 do 180, pa ga pretvori u X poziciju na kompasu
        float normalizedAngle = angle / 180f;

        // Pomeraj ikonicu levo/desno u zavisnosti od ugla
        float xPos = normalizedAngle * (compassWidth / 1f);

        if(xPos > -460f && xPos < 460f)
        {
            // Postavi poziciju ikone
            compassIcon.anchoredPosition = new Vector2(xPos, compassIcon.anchoredPosition.y);
        }
        else
        {
            float edgePos = angle < 0 ? -460f : 460f;
            compassIcon.anchoredPosition = new Vector2(edgePos, compassIcon.anchoredPosition.y);
        }
        
    }
}
