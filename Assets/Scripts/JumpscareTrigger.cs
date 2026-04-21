using System.Collections;
using UnityEngine;

public class JumpscareTrigger : MonoBehaviour
{
    public GameObject player, figure;
    private readonly float radius = 10f;
    private GameObject nextPoint;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        nextPoint = GameObject.Find("FabricPoint");
    }

    // Update is called once per frame
    void Update()
    {
        float distance = Vector3.Distance(player.transform.position, this.transform.position);

        if (distance <= radius)
        {
            //GameObject icon = HomeEnterTrigger.FindChildByName(player, "CompassIcon");
            //icon.SetActive(false);

            StartCoroutine(ShowFigureTemporarily());
            Stage.isSeenMonster = true;

            CompassIcon.objective = GameObject.Find("FabricPoint").transform;

            MonoBehaviour script = nextPoint.GetComponent<MonoBehaviour>();
            script.enabled = true;

            enabled = false;
        }
    }

    IEnumerator ShowFigureTemporarily()
    {
        figure.SetActive(true);
        yield return new WaitForSeconds(0.25f);
        figure.SetActive(false);
    }

}
