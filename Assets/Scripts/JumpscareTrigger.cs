using System.Collections;
using UnityEngine;

public class JumpscareTrigger : MonoBehaviour
{
    public GameObject player, figure;
    private readonly float radius = 10f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float distance = Vector3.Distance(player.transform.position, this.transform.position);

        if (distance <= radius)
        {
            GameObject icon = HomeEnterTrigger.FindChildByName(player, "CompassIcon");
            icon.SetActive(false);

            StartCoroutine(ShowFigureTemporarily());
            Stage.isSeenMonster = true;

            this.enabled = false;
        }
    }

    IEnumerator ShowFigureTemporarily()
    {
        figure.SetActive(true);
        yield return new WaitForSeconds(0.25f);
        figure.SetActive(false);
    }

}
