using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ItemPickup : MonoBehaviour
{
    public GameObject itemHand;        // item on the ground

    public GameObject uiPrompt, uiIcon;          // UI text

    public GameObject item;            // item to hide

    public Transform  itemTransform,
                      itemHandTransform,
                      playerTransform;

    public float pickUpDistance = 3f;

    public bool isPickedUp = false;

    //private GameObject currentItem;

    private void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        MeshRenderer itemRenderer = item.GetComponent<MeshRenderer>();

        //droping the item
        if (isPickedUp && Input.GetKeyDown(KeyCode.G) && itemHand.activeSelf)
        {
            GameObject player = GameObject.Find("Player");

            if (PlayerItems.Items.IndexOf(itemHand) == 1 && PlayerItems.Items.Count == 3)
            {
                if (uiIcon.name.Equals("RevolverHandIcon"))
                {
                    GameObject icon = HomeEnterTrigger.FindChildByName(player, "FlashLightHandIcon");
                    RectTransform position = icon.GetComponent<RectTransform>();
                    position.offsetMin = new Vector2(1750, 130);
                    position.offsetMax = new Vector2(10, -770);
                }
                else
                {
                    GameObject icon = HomeEnterTrigger.FindChildByName(player, "RevolverHandIcon");
                    RectTransform position = icon.GetComponent<RectTransform>();
                    position.offsetMin = new Vector2(1695, 65);
                    position.offsetMax = new Vector2(70, -715);
                }
            }

            itemHand.SetActive(false);

            Vector3 dropOffset=playerTransform.forward * 3f;
            Vector3 dropPosition=playerTransform.position + dropOffset;

            itemTransform.position = new Vector3(dropPosition.x, itemTransform.position.y + 2f, dropPosition.z);

            isPickedUp = false;
            PlayerItems.Items.Remove(itemHand);
            PlayerItems.Items[0].SetActive(true);
            itemRenderer.enabled = true;
            uiPrompt.SetActive(true);

            GameObject emptyIcon = HomeEnterTrigger.FindChildByName(player, "emptyHandIcon");
            Image emptyImage = emptyIcon.GetComponent<Image>();
            Color emptyColor = emptyImage.color;
            emptyColor.a = 1f;
            emptyImage.color = emptyColor;

            Image image = uiIcon.GetComponent<Image>();
            Color color = image.color;
            color.a = 0.3f;
            image.color = color;
            uiIcon.SetActive(false);
        }

        float distance = Vector3.Distance(playerTransform.position, itemTransform.position);

        //picking up the item
        if (distance <= pickUpDistance && !isPickedUp)
        {
            uiPrompt.SetActive(true);

            if (Input.GetKeyDown(KeyCode.E))
            {
                isPickedUp = true;
                PlayerItems.Items.Add(itemHand);
                itemRenderer.enabled = false;
                uiPrompt.SetActive(false);

                if (itemHand.name.Equals("FlashLightHand") && !Tasks.flashlight)
                {
                    Tasks.flashlight = true;
                    FindAnyObjectByType<MonologueManager>().ShowNextInstruction();
                }

                if(PlayerItems.Items.IndexOf(itemHand) == 1)
                {
                    RectTransform position = uiIcon.GetComponent<RectTransform>();
                    if(uiIcon.name == "FlashLightHandIcon")
                    {
                        position.offsetMin = new Vector2(1750, 130);
                        position.offsetMax = new Vector2(10, -770);
                    }
                    else
                    {
                        position.offsetMin = new Vector2(1695, 65);
                        position.offsetMax = new Vector2(70, -715);
                    }
                }
                else if(PlayerItems.Items.IndexOf(itemHand) == 2)
                {
                    RectTransform position = uiIcon.GetComponent<RectTransform>();
                    if(uiIcon.name == "RevolverHandIcon")
                    {
                        position.offsetMin = new Vector2(1700, 200);
                        position.offsetMax = new Vector2(70, -580);
                    }
                    else
                    {
                        position.offsetMin = new Vector2(1750, 260);
                        position.offsetMax = new Vector2(10, -640);
                    }
                }
                uiIcon.SetActive(true);
            }
        }
        else if(distance > pickUpDistance && distance < pickUpDistance + 2f)
        {
            uiPrompt.SetActive(false);
        }
    }
}
