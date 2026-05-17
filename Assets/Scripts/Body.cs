using UnityEngine;

public class Body : MonoBehaviour
{
    private GameObject player;
    private GameObject uiPrompt;
    private Renderer[] renderers;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.Find("Player");
        uiPrompt = HomeEnterTrigger.FindChildByName(player, "ItemPickUpPrompt");
        renderers =
            GetComponentsInChildren<Renderer>(true);
    }

    // Update is called once per frame
    void Update()
    {
        float distance = Vector3.Distance(player.transform.position, this.transform.position);
        if(distance <= 3f)
        {
            if (renderers[0].enabled)
            {
                uiPrompt.SetActive(true);
            }

            if (Input.GetKeyDown(KeyCode.E))
            {
                HideBody();
                GameObject thugHand = HomeEnterTrigger.FindChildByName(player, "ThugHand");
                thugHand.SetActive(true);
                uiPrompt.SetActive(false);
            }
        }
        else
        {
            uiPrompt.SetActive(false);
        }
    }

    void HideBody()
    {
        foreach (Renderer r in renderers)
        {
            r.enabled = false;
        }
    }
}
