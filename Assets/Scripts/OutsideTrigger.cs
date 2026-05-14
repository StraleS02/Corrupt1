using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering.PostProcessing;

public class OutsideTrigger : MonoBehaviour
{
    public GameObject door;
    
    public float enterDistance;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        GameObject player = GameObject.Find("Player");

        float distance = Vector3.Distance(player.transform.position, 
                                          door.transform.position);
        if (distance < enterDistance)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                SceneManager.sceneLoaded += OnSceneLoaded;
                SceneManager.LoadScene("SampleScene");
            }
        }
    }

    public static void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
    {
        CarSpawner.sceneChanged =    true;
        GameObject player =          GameObject.Find("Player");
        GameObject car =             GameObject.Find("Car");
        GameObject carEnterUI =      HomeEnterTrigger.FindChildByName(player, "EnterCarPrompt");
        GameObject itemPickUpUI =    HomeEnterTrigger.FindChildByName(player, "ItemPickUpPrompt");
        GameObject spawnPoint =      GameObject.Find("SpawnPointHouse");
        GameObject cutScene =        GameObject.Find("CutsceneCameraRig");
        GameObject spawnPointDream = GameObject.Find("SpawnPointDream");
        GameObject volumeObject =    GameObject.Find("PostProcessingVolume");
        PostProcessVolume volume =   volumeObject.GetComponent<PostProcessVolume>();

        HomeEnterTrigger homeEnterTrigger = FindAnyObjectByType<HomeEnterTrigger>();
        CarEnterTrigger carTrigger =        FindAnyObjectByType<CarEnterTrigger>();
        EnterCarEnd enterCarEnd =           FindAnyObjectByType<EnterCarEnd>();
        Pistol pistol =                     player.GetComponentInChildren<Pistol>(true);

        if (homeEnterTrigger != null && 
            carTrigger != null &&
            enterCarEnd != null)
        {
            pistol.volume = volume;
            enterCarEnd.player =         player;
            homeEnterTrigger.player =    player;
            carTrigger.player =          player;
            carTrigger.playerTransform = player.transform;
            carTrigger.uiPrompt =        carEnterUI;
            car.transform.position =     PlayerItems.carPosition;
            car.transform.rotation =     PlayerItems.carRotation;
        }

        ItemPickup[] allPickups = FindObjectsByType<ItemPickup>(FindObjectsSortMode.None);
        foreach (ItemPickup pickup in allPickups)
        {
            pickup.playerTransform = player.transform;
            pickup.uiPrompt =        itemPickUpUI;

            // ✴️ Pronađi itemHand objekat u playeru po imenu
            string expectedName = pickup.item.name + "Hand"; // npr. "Axe" → "Axe_Hand"
            GameObject itemHand = HomeEnterTrigger.FindChildByName(player, expectedName);

            string expectedNameIcon = pickup.item.name + "HandIcon";
            GameObject itemIcon = HomeEnterTrigger.FindChildByName(player, expectedNameIcon);

            if (itemHand != null)
            {
                pickup.itemHand =          itemHand;
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

        if (player != null && spawnPoint != null)
        {
            cutScene.SetActive(false);
            player.transform.position = spawnPoint.transform.position;
            player.SetActive(true);
        }

        if (player != null && spawnPointDream != null && Stage.dream)
        {
            Stage.dream = false;

            cutScene.SetActive(false);
            player.transform.position = spawnPointDream.transform.position;
            player.SetActive(true);

            GameObject jumpscare = GameObject.Find("JumpscareObject");
            MonoBehaviour triggerScript = jumpscare.GetComponent<MonoBehaviour>();
            JumpscareTrigger trigger = FindAnyObjectByType<JumpscareTrigger>();
            triggerScript.enabled = true;
            trigger.player = player;
            //GameObject volumeObject = GameObject.Find("PostProcessingVolume");
            //PostProcessVolume volume = volumeObject.GetComponent<PostProcessVolume>();

            GameObject jmpScare = GameObject.Find("JumpscareObject");
            if(jmpScare != null)
            {
                GameObject compassIcon = HomeEnterTrigger.FindChildByName(player, "CompassIcon");
                compassIcon.SetActive(true);
                CompassIcon.objective = jmpScare.transform;
            }

            if (volume != null && volume.profile != null)
            {
                if (volume.profile.TryGetSettings(out ColorGrading colorGrading))
                {
                    colorGrading.temperature.value = -80f;
                }
                else
                {
                    Debug.LogWarning("ColorGrading effect nije dodat u profil!");
                }
            }
        }
        if (player != null && Stage.dreamOver)
        {
            GameObject compassIcon = HomeEnterTrigger.FindChildByName(player, "CompassIcon");
            GameObject point = GameObject.Find("FabricPoint3");
            compassIcon.SetActive(true);
            CompassIcon.objective = point.transform;
            Stage.updatedPoint = point.transform;
            MonoBehaviour convo = point.GetComponent<MonoBehaviour>();
            convo.enabled = true;
            FindInScene("Thug").SetActive(true);
        }

        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public static GameObject FindInScene(string name)
    {
        Transform[] allTransforms = Resources.FindObjectsOfTypeAll<Transform>();

        foreach (Transform t in allTransforms)
        {
            // filtriraj samo objekte iz scene (ne prefabe iz Projecta)
            if (t.hideFlags != HideFlags.None)
                continue;

            if (t.gameObject.scene.isLoaded && t.name == name)
                return t.gameObject;
        }

        return null;
    }
}
