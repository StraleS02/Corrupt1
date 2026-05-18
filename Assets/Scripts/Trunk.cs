using System.Collections;
using UnityEngine;

public class Trunk : MonoBehaviour
{
    public GameObject player;
    public GameObject uiPrompt;

    private GameObject body, bodyTrunk;

    private Animator trunkAnimator;

    private Transform trunkLid; // objekat gepeka

    private float interactDistance = 4f;

    private bool isOpen = false;
    private bool isAnimating = false;

    private bool 
                 isPutDown = false;

    void Start()
    {
            //player = GameObject.Find("Player");
            //uiPrompt = HomeEnterTrigger.FindChildByName(player, "ItemPickUpPrompt");
        trunkAnimator = GetComponent<Animator>();
        trunkLid = GetComponent<Transform>();
        body = HomeEnterTrigger.FindChildByName(player, "ThugHand");
        bodyTrunk = HomeEnterTrigger.FindChildByName(GameObject.Find("Car"), "ThugCar");

    }

    void Update()
    {
        //Debug.Log(player.transform.position);
        //Debug.Log(transform.position);
        /*if(player == null)
        {
            player = GameObject.Find("Player");
            uiPrompt = HomeEnterTrigger.FindChildByName(player, "ItemPickUpPrompt");
        }

        if (!player.activeSelf)
        {
            player = GameObject.Find("Player");
            uiPrompt = HomeEnterTrigger.FindChildByName(player, "ItemPickUpPrompt");
        }*/

        if (isAnimating) return;

        float distance =
            Vector3.Distance(player.transform.position,
                             transform.position);

        if (distance <= interactDistance)
        {
            uiPrompt.SetActive(true);
            if (!Tasks.isCloseToDump && bodyTrunk.activeSelf && !isOpen && Input.GetKeyDown(KeyCode.E))
            {
                StartCoroutine(
                    FindAnyObjectByType<MonologueManager>()
                    .ShowTemporaryDialogue(
                        "I shouldn't do this here...",
                        3f
                    )
                );

                return;
            }
            if (Input.GetKeyDown(KeyCode.E) && body.activeSelf && isOpen)
            {
                body.SetActive(false);
                bodyTrunk.SetActive(true);
                isPutDown = true;
            }
            else if (Input.GetKeyDown(KeyCode.E) && bodyTrunk.activeSelf && isOpen && !isPutDown)
            {
                body.SetActive(true);
                bodyTrunk.SetActive(false);
            }
            else if (Input.GetKeyDown(KeyCode.E))
            {
                if (!isOpen)
                    StartCoroutine(OpenTrunk());
                else
                    StartCoroutine(CloseTrunk());

                if(isPutDown) isPutDown = false;
            }
        }
        else if(distance >= interactDistance && distance < interactDistance + 5f)
        {
            uiPrompt.SetActive(false);
        }
    }

    IEnumerator OpenTrunk()
    {
        isAnimating = true;

        trunkAnimator.enabled = true;
        trunkAnimator.Play("Trunk", 0, 0f);

        yield return null;

        float length =
            trunkAnimator.GetCurrentAnimatorStateInfo(0).length;

        yield return new WaitForSeconds(length);
        
        // 🔥 ostavi otvoren gepek
        Vector3 rot = trunkLid.localEulerAngles;
        rot.x = 70f;
        trunkLid.localEulerAngles = rot;

        trunkAnimator.enabled = false;

        isOpen = true;
        isAnimating = false;
    }

    IEnumerator CloseTrunk()
    {
        isAnimating = true;

        trunkAnimator.enabled = true;
        trunkAnimator.Play("CloseTrunk", 0, 0f);

        yield return null;

        float length =
            trunkAnimator.GetCurrentAnimatorStateInfo(0).length;

        yield return new WaitForSeconds(length);

        Vector3 rot = trunkLid.localEulerAngles;
        rot.x = 0f;
        trunkLid.localEulerAngles = rot;

        trunkAnimator.enabled = false;

        isOpen = false;
        isAnimating = false;
    }
}
