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

    private void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
    {
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
}
