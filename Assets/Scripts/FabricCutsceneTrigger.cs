using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FabricCutsceneTrigger : MonoBehaviour
{
    private GameObject player;
    public GameObject cutsceneCamera;
    public GameObject muzzleFlash;
    private readonly float radius = 10f;
    private bool triggered = false;
    private ParticleSystem muzzlePS;

    public GameObject figure1,
                      figure2;

    public Animator arm,
                    forearm,
                    pistol;

    public GameObject thug;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.Find("Player");
        muzzlePS = muzzleFlash.GetComponent<ParticleSystem>();
        //cutsceneCamera = GameObject.Find("DreamCutscene");
        //arm = HomeEnterTrigger.FindChildByName(GameObject.Find("Figure (1)"), "RightArm").GetComponent<Animator>();
        //forearm = HomeEnterTrigger.FindChildByName(GameObject.Find("Figure (1)"), "RightForeArm").GetComponent<Animator>();
        //pistol = HomeEnterTrigger.FindChildByName(GameObject.Find("Figure (1)"), "RevolverFigure").GetComponent<Animator>();
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
        Stage.dreamOver = true;

        // 1. Fade Out
        yield return FadeOutsideController.Instance.FadeOut(1f);

        // 2. Isključi player
        player.SetActive(false);

        // 3. Uključi cutscene kameru
        if (cutsceneCamera != null)
            cutsceneCamera.SetActive(true);

        // 4. POKRENI animaciju OD POČETKA
        if (figure1 != null && figure2 != null)
        {
            figure1.SetActive(true);
            figure2.SetActive(true);

            figure1.transform.rotation = Quaternion.identity;
            figure2.transform.rotation = Quaternion.identity;

            arm.Play(0, 0, 0f);
            forearm.Play(0, 0, 0f);
            pistol.Play(0, 0, 0f);
        }

        // 5. Fade In da se vidi cutscene
        //yield return FadeController.Instance.FadeIn(1f);

        // 6. ČEKAJ DA SE ANIMACIJA ZAVRŠI
        if (arm != null)
        {
            yield return null; // sačekaj 1 frame da krene

            bool fired = false;

            AnimatorStateInfo state = arm.GetCurrentAnimatorStateInfo(0);
            float clipLength = state.length; // dužina u sekundama

            // koliko traje 1 frame (pretpostavimo 60 FPS animaciju)
            float frameTime = 1f / 60f;

            // poslednjih 10 frejmova
            float triggerTime = clipLength - (10f * frameTime);

            while (arm.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f)
            {
                float currentTime = arm.GetCurrentAnimatorStateInfo(0).normalizedTime * clipLength;

                if (!fired && currentTime >= triggerTime)
                {
                    fired = true;

                    if (muzzleFlash != null)
                        muzzleFlash.SetActive(true);

                    if (muzzlePS != null)
                        muzzlePS.Play();
                }

                yield return null;
            }
        }

        thug.SetActive(true);

        // 8. Isključi cutscene kameru
        if (cutsceneCamera != null)
        {
            cutsceneCamera.SetActive(false);
            figure1.SetActive(false);
            figure2.SetActive(false);
        }
        player.SetActive(true);
        GameObject compass = HomeEnterTrigger.FindChildByName(player, "CompassIcon");
        compass.SetActive(false);

        SceneManager.sceneLoaded += HomeEnterTrigger.OnSceneLoaded;
        SceneManager.LoadScene("DemoScene");
    }
}
