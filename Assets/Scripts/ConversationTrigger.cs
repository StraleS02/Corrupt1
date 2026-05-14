using System.Collections;
using UnityEngine;

public class ConversationTrigger : MonoBehaviour
{
    private GameObject player;
    private readonly float radius = 10f;
    private bool triggered = false;
    public Camera cam;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (triggered) return;

        float distance = Vector3.Distance(player.transform.position, this.transform.position);

        if (distance <= radius)
        {
            triggered = true;
            StartCoroutine(CutsceneSequence());
        }
    }

    IEnumerator CutsceneSequence()
    {
        // fade out
        yield return FadeOutsideController.Instance.FadeOut(1f);

        // ugasi player
        player.SetActive(false);

        // upali kameru
        if (cam != null)
            cam.enabled = true;

        // trajanje scene
        yield return new WaitForSeconds(2f);

        // fade out pre povratka
        yield return FadeOutsideController.Instance.FadeOut(1f);

        // 🔥 PREBACUJ PLAYER DOK JE JOŠ UGAŠEN
        player.transform.position = cam.transform.position;
        player.transform.rotation = cam.transform.rotation;

        // ugasi kameru
        if (cam != null)
            cam.enabled = false;

        // upali player
        player.SetActive(true);

        // fade in
        //yield return FadeOutsideController.Instance.FadeIn(1f);

        // 🔥 UGASI OVU SKRIPTU ZAUVEK
        this.enabled = false;
    }
}
