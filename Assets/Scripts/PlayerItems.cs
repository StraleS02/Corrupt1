using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class PlayerItems : MonoBehaviour
{
    public static List<GameObject> Items;
    public GameObject emptyHand;
    public static Vector3 carPosition;
    public static Quaternion carRotation;
    private int removedIndex = -1;
    private GameObject removedItem = null;
    private bool isRemoved = false,
                 isBack = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Items = new List<GameObject>();
        carPosition = Vector3.zero;
        //emptyHand = new GameObject();

        if(!Items.Contains(emptyHand))
            Items.Add(emptyHand);

        FindAnyObjectByType<MonologueManager>().StartMonologue();
        FindAnyObjectByType<MonologueManager>().StartInstructions();
    }

    // Update is called once per frame
    void Update()
    {
        if(Stage.dream && !Stage.isSeenMonster && !isRemoved)
        {
            for (int i = 0; i < Items.Count; i++)
            {
                if (Items[i].name == "RevolverHand")
                {
                    GameObject player = GameObject.Find("Player");
                    GameObject icon = HomeEnterTrigger.FindChildByName(player, "RevolverHandIcon");
                    Image image = icon.GetComponent<Image>();
                    Color color = image.color;
                    color.a = 0f;
                    image.color = color;

                    removedIndex = i;
                    removedItem = Items[i];
                    Items.RemoveAt(i);
                    isRemoved = true;
                    break;
                }
            }
        }
        else if(Stage.dream && Stage.isSeenMonster && !isBack)
        {
            Items.Insert(removedIndex, removedItem);
            GameObject player = GameObject.Find("Player");
            GameObject icon = HomeEnterTrigger.FindChildByName(player, "RevolverHandIcon");
            Image image = icon.GetComponent<Image>();
            Color color = image.color;
            color.a = 0.3f;
            image.color = color;
            isBack = true;
        }

        if (Input.mouseScrollDelta.y > 0)
        {
            for(int i=0; i < Items.Count; i++)
            {
                if (Items[i].activeSelf && Items.Count > 1)
                {
                    GameObject player = GameObject.Find("Player");
                    GameObject icon = HomeEnterTrigger.FindChildByName(player, Items[i].name + "Icon");
                    //Items[i + 1].SetActive(true);
                    
                    Image image = icon.GetComponent<Image>();
                    
                    Color color = image.color;
                    

                    if (i + 1 == Items.Count)
                    {
                        Items[i].SetActive(false);
                        color.a = 0.3f;
                        image.color = color;
                        Items[0].SetActive(true);
                        GameObject emptyIcon = HomeEnterTrigger.FindChildByName(player, "emptyHandIcon");
                        Image imageEmpty = emptyIcon.GetComponent<Image>();
                        Color colorEmpty = imageEmpty.color;
                        colorEmpty.a = 1f;
                        imageEmpty.color = colorEmpty;
                        break;
                    }
                    GameObject iconNext = HomeEnterTrigger.FindChildByName(player, Items[i + 1].name + "Icon");
                    Image imageNext = iconNext.GetComponent<Image>();
                    Color colorNext = imageNext.color;

                    Items[i].SetActive(false);
                    color.a = 0.3f;
                    image.color = color;
                    Items[i + 1].SetActive(true);
                    colorNext.a = 1f;
                    imageNext.color = colorNext;

                    StartCoroutine(PickingUpAnimation(Items[i + 1].transform));
                    break;
                }
            }
        }
        else if(Input.mouseScrollDelta.y < 0)
        {
            for (int i = 0; i < Items.Count; i++)
            {
                if (Items[i].activeSelf && Items.Count > 1)
                {
                    GameObject player = GameObject.Find("Player");
                    GameObject icon = HomeEnterTrigger.FindChildByName(player, Items[i].name + "Icon");
                    
                    Image image = icon.GetComponent<Image>();
                    
                    Color color = image.color;
                    

                    if (i - 1 == -1)
                    {
                        Items[i].SetActive(false);
                        color.a = 0.3f;
                        image.color = color;
                        Items[Items.Count - 1].SetActive(true);
                        GameObject emptyIcon = HomeEnterTrigger.FindChildByName(player, Items[Items.Count - 1].name + "Icon");
                        Image imageEmpty = emptyIcon.GetComponent<Image>();
                        Color colorEmpty = imageEmpty.color;
                        colorEmpty.a = 1f;
                        imageEmpty.color = colorEmpty;

                        StartCoroutine(PickingUpAnimation(Items[Items.Count - 1].transform));
                        break;
                    }
                    GameObject iconNext = HomeEnterTrigger.FindChildByName(player, Items[i - 1].name + "Icon");
                    Image imageNext = iconNext.GetComponent<Image>();
                    Color colorNext = imageNext.color;

                    Items[i].SetActive(false);
                    color.a = 0.3f;
                    image.color = color;
                    Items[i - 1].SetActive(true);
                    colorNext.a = 1f;
                    imageNext.color = colorNext;

                    StartCoroutine(PickingUpAnimation(Items[i - 1].transform));
                    break;
                }
            }
        }
    }

    public IEnumerator PickingUpAnimation(Transform target)
    {
        Vector3 startPos = target.localPosition - new Vector3(0, 0.5f, 0);
        Vector3 endPos = target.localPosition;
        target.localPosition = startPos;

        float elapsed = 0f;

        while (elapsed < 0.3f)
        {
            target.localPosition = Vector3.Lerp(startPos, endPos, elapsed / 0.3f);
            elapsed += Time.deltaTime;
            yield return null;
        }
        target.localPosition = endPos;
    }
}
