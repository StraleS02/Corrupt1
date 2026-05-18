using System.Collections;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public class Trashcan : MonoBehaviour
{
    public GameObject player,
                      uiPrompt, body;     

    private GameObject 
                       bodyDumpster;

    private Animator lidAnimator;

    private Transform dumpsterLid;

    private bool isOpen = false;
    private bool isAnimating = false;
    //private bool check = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    { 
            //player = OutsideTrigger.FindInScene("Player");
            //uiPrompt = HomeEnterTrigger.FindChildByName(player, "ItemPickUpPrompt");
            //body = HomeEnterTrigger.FindChildByName(player, "ThugHand");
        bodyDumpster = HomeEnterTrigger.FindChildByName(transform.parent.gameObject, "ThugDump");
        lidAnimator = GetComponent<Animator>();
        dumpsterLid = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        /*
        if(!player.activeSelf)
        {
            player = GameObject.Find("Player");
            uiPrompt = HomeEnterTrigger.FindChildByName(player, "ItemPickUpPrompt");
            body = HomeEnterTrigger.FindChildByName(player, "ThugHand");
        }*/

        if (isAnimating) return;

        float distance =
            Vector3.Distance(player.transform.position,
                             transform.position);

        if(distance <= 30f)
        {
            Tasks.isCloseToDump = true;
        }
        else if(distance > 30f && distance < 35f)
        {
            Tasks.isCloseToDump = false;
        }

        if(distance <= 5f && (body.activeSelf || bodyDumpster.activeSelf))
        {
            uiPrompt.SetActive(true);

            if(Input.GetKeyDown(KeyCode.E) && isOpen && !bodyDumpster.activeSelf)
            {
                body.SetActive(false);
                bodyDumpster.SetActive(true);
            }
            else if (Input.GetKeyDown(KeyCode.E))
            {
                if (!isOpen)
                    StartCoroutine(OpenLid());
                else
                    StartCoroutine(CloseLid());
            }
        }
        else if (distance > 5f && distance < 10f)
        {
            uiPrompt.SetActive(false);
        }
    }

    IEnumerator OpenLid()
    {
        isAnimating = true;

        lidAnimator.enabled = true;
        lidAnimator.Play("OpenLid", 0, 0f);

        yield return null;

        float length =
            lidAnimator.GetCurrentAnimatorStateInfo(0).length;

        yield return new WaitForSeconds(length);

        // 🔥 ostavi otvoren gepek
        dumpsterLid.localRotation =
            Quaternion.Euler(-140f, 0f, 0f);

        lidAnimator.enabled = false;

        isOpen = true;
        isAnimating = false;
    }

    IEnumerator CloseLid()
    {
        isAnimating = true;

        lidAnimator.enabled = true;
        lidAnimator.Play("CloseLid", 0, 0f);

        yield return null;

        float length =
            lidAnimator.GetCurrentAnimatorStateInfo(0).length;

        yield return new WaitForSeconds(length);

        dumpsterLid.localRotation =
            Quaternion.Euler(-90f, 0f, 0f);

        lidAnimator.enabled = false;

        isOpen = false;
        isAnimating = false;
    }
}
