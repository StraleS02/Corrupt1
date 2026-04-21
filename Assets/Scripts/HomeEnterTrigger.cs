using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HomeEnterTrigger : MonoBehaviour
{
    public GameObject door, player;
    public float enterDistance;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        float distance = Vector3.Distance(player.transform.position, door.transform.position);
        if (distance < enterDistance)
        {
            if(Input.GetKeyDown(KeyCode.E))
            {
                if(!Tasks.goHome) 
                {
                    GameObject car = GameObject.Find("Car");
                    GameObject icon = FindChildByName(player, "CompassIcon");
                    GameObject iconCar = FindChildByName(car, "CompassIcon");
                    if (icon != null)
                    {
                        icon.SetActive(false);
                        iconCar.SetActive(false);
                    }
                    Tasks.goHome = true; 
                    FindAnyObjectByType<MonologueManager>().ShowNextInstruction();
                }
                SceneManager.sceneLoaded += OnSceneLoaded;
                SceneManager.LoadScene("DemoScene");
            }
        }
    }

    public static void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
    {
        GameObject player = GameObject.Find("Player");
        GameObject spawnPoint = GameObject.Find("SpawnPoint");
        GameObject itemPickUpUI = FindChildByName(player, "ItemPickUpPrompt");

        ItemPickup[] allPickups = FindObjectsByType<ItemPickup>(FindObjectsSortMode.None);
        foreach (ItemPickup pickup in allPickups)
        {
            pickup.playerTransform = player.transform;
            pickup.uiPrompt = itemPickUpUI;

            // ✴️ Pronađi itemHand objekat u playeru po imenu
            string expectedName = pickup.item.name + "Hand"; // npr. "Axe" → "Axe_Hand"
            GameObject itemHand = FindChildByName(player, expectedName);

            string expectedNameIcon = pickup.item.name + "HandIcon";
            GameObject itemIcon = FindChildByName(player, expectedNameIcon);

            if (itemHand != null)
            {
                pickup.itemHand = itemHand;
                pickup.itemHandTransform = itemHand.transform;
                pickup.uiIcon = itemIcon;

                foreach (var item in PlayerItems.Items)
                {
                    if (item.name == itemHand.name)
                    {
                        MeshRenderer meshRenderer = pickup.item.GetComponent<MeshRenderer>();
                        ItemPickup itemPickup = pickup.item.GetComponent<ItemPickup>();

                        meshRenderer.enabled = false;
                        itemPickup.isPickedUp = true;
                    }
                }
            }
            else
            {
                Debug.LogWarning("Nisam pronašao itemHand za: " + pickup.item.name);
            }
        }

        if ( player != null && spawnPoint != null )
        {
            player.transform.position = spawnPoint.transform.position;
        }

        if (Stage.dreamOver && !Tasks.wokeUp)
        {
            Tasks.wokeUp = true;

            // 🔴 nađi fade controller
            FadeController fade = Object.FindAnyObjectByType<FadeController>();
            GameObject sleepAnim = HomeEnterTrigger.FindChildByName(GameObject.Find("House_Prefab"), "WakeAnim");
            GameObject wakeUpPoint = GameObject.Find("WakeUpPoint");

            player.SetActive(false);

            // 🔴 osiguraj da je ekran crn na startu
            if (fade != null)
            {
                var img = fade.GetComponent<UnityEngine.UI.Image>();
                if (img != null)
                {
                    img.enabled = true;
                    Color c = img.color;
                    c.a = 1f;
                    img.color = c;
                }
            }

            // 🔴 uzmi animator sa playera
            Animator anim = sleepAnim.GetComponent<Animator>();
            Camera animCam = sleepAnim.GetComponent<Camera>();
            animCam.enabled = true;

            if (anim != null)
            {
                // 🔥 PUSTI ANIMACIJU UNAZAD (ustajanje iz kreveta)
                anim.enabled = true;
                //anim.speed = -1f;
                anim.Play("WakeAnim", 0, 0f);
                //anim.Update(0f);
            }

            // 🔴 pokreni fade in malo kasnije (korutina helper)
            CoroutineRunner.Instance.StartCoroutine(FadeInAfterWake(player, fade, anim, animCam));

            player.transform.position = wakeUpPoint.transform.position;
        }

        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public static GameObject FindChildByName(GameObject parent, string childName)
    {
        Transform[] children = parent.GetComponentsInChildren<Transform>(true); // true = uključuje i deaktivirane

        foreach (Transform child in children)
        {
            if (child.name == childName)
                return child.gameObject;
        }

        return null; // nije pronađen
    }

    public static IEnumerator FadeInAfterWake(GameObject player, FadeController fade, Animator anim, Camera animCam)
    {
        yield return null; // da animacija krene

        // fade in
        if (fade != null)
            yield return fade.FadeIn(1f);

        // čekaj da animacija završi (ide unazad do 0)
        while (anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f)
        {
            yield return null;
        }

        // vrati speed na normalno
        //anim.speed = 1f;
        anim.enabled = false;
        animCam.enabled = false;

        player.SetActive(true);
    }
}
