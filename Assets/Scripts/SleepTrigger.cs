using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SleepTrigger : MonoBehaviour
{
    private float sleepDistance = 4;
    private GameObject uiPrompt;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        GameObject player = GameObject.Find("Player");
        Transform playerTransform = player.transform;
        Transform positionObject = GameObject.Find("SleepObject").transform;
        uiPrompt = HomeEnterTrigger.FindChildByName(player, "ItemPickUpPrompt");

        float distance = Vector3.Distance(playerTransform.position, positionObject.position);

        if (distance <= sleepDistance)
        {
            if(Tasks.flashlight) uiPrompt.SetActive(true);

            if (Input.GetKeyDown(KeyCode.E) && Tasks.flashlight)
            {
                StartCoroutine(Sleep());
            }
        }
        else
        {
            if (Tasks.flashlight)  uiPrompt.SetActive(false);
        }
    }

    IEnumerator Sleep()
    {
        uiPrompt?.SetActive(false);

        Camera animCam = GetComponent<Camera>();
        animCam.enabled = true;

        GameObject player = GameObject.Find("Player");
        player.SetActive(false);

        Animator sleepAnimation = GetComponent<Animator>();
        sleepAnimation.enabled = true;
        sleepAnimation.Play("SleepAnim", 0, 0f);

        yield return new WaitForSeconds(sleepAnimation.GetCurrentAnimatorStateInfo(0).length - 1f);

        yield return FadeController.Instance.FadeOut(1f);

        player.SetActive(true);
        Stage.dream = true;

        SceneManager.sceneLoaded += OutsideTrigger.OnSceneLoaded;
        SceneManager.LoadScene("SampleScene");
    }
}
