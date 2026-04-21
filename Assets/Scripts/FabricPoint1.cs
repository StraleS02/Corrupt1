using UnityEngine;

public class FabricPoint1 : MonoBehaviour
{
    public GameObject player;
    private readonly float radius = 10f;
    private GameObject nextPoint;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.Find("Player");
        nextPoint = GameObject.Find("FabricPoint2");
    }

    // Update is called once per frame
    void Update()
    {
        float distance = Vector3.Distance(player.transform.position, this.transform.position);

        if (distance <= radius)
        {
            CompassIcon.objective = GameObject.Find("FabricPoint2").transform;
            MonoBehaviour script = nextPoint.GetComponent<MonoBehaviour>();

            script.enabled = true;

            enabled = false;
        }
    }
}
