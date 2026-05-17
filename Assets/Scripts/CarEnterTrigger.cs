using System.Collections.Generic;
using UnityEngine;

public class CarEnterTrigger : MonoBehaviour
{
    public Light light1;
    public Light light2;
    public GameObject uiPrompt;          // UI tekst (npr. "Press F to enter vehicle")
    public GameObject player;            // igra? koga ?emo sakriti
    public GameObject car;               // objekat automobila
    public Camera carCamera;            // kamera iz auta
    public MonoBehaviour carController, enterEnd;  // skripta za vožnju

    public static List<GameObject> activeCars = new List<GameObject>();

    public Transform playerTransform;    // transform igra?a
    public Transform carTransform;       // transform auta

    public float enterDistance = 100f;     // rastojanje pri kojem se može u?i

    private bool hasEntered = false;

    private void Start()
    {
        //player = GameObject.Find("Player");
    }

    void Update()
    {
        /*
        GameObject gm = GameObject.Find("CutsceneCameraRig");
        if (!gm.activeSelf)
        {
            player = GameObject.Find("Player");
            playerTransform = player.transform;
        }
        */
        if (hasEntered)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                ExitVehicle();
            }
            return;
        }

        float distance = Vector3.Distance(playerTransform.position, carTransform.position);

        if (distance <= enterDistance)
        {
            uiPrompt.SetActive(true);

            if (Input.GetKeyDown(KeyCode.F))
            {
                EnterVehicle();
            }
        }
        else
        {
            uiPrompt.SetActive(false);
        }
    }

    void EnterVehicle()
    {
        GameObject carCanvas = HomeEnterTrigger.FindChildByName(car, "CanvasCar");
        GameObject carCam = HomeEnterTrigger.FindChildByName(car, "CarCamera");
        
        if (CompassIcon.objective)
        {
            CompassIcon.player = carCam.transform;
            CompassIcon.objective = Stage.updatedPoint.transform;
        }
            
        carCanvas.SetActive(true);

        hasEntered = true;
        EnterCarEnd.isEntering = true;

        light1.enabled = true;
        light2.enabled = true;

        uiPrompt.SetActive(false);
        player.SetActive(false);

        GameObject carDoor = HomeEnterTrigger.FindChildByName(car, "CDoor_FL");
        GameObject animCam = HomeEnterTrigger.FindChildByName(car, "EnteringCarAnim");

        Animator animatorDoor = carDoor.GetComponent<Animator>();
        Animator animatorCam = animCam.GetComponent<Animator>();

        animatorDoor.enabled = true;
        animatorDoor.Play("CarEnterDoor", 0, 0f);

        animCam.SetActive(true);
        animatorCam.Play("CarEnter", 0, 0f);

        enterEnd.enabled = true;
    }

    void ExitVehicle()
    {
        GameObject carCanvas = HomeEnterTrigger.FindChildByName(car, "CanvasCar");
        
        if (CompassIcon.objective)
        {
            CompassIcon.player = player.transform;
            CompassIcon.objective = Stage.updatedPoint.transform;
        }
            
        carCanvas.SetActive(false);

        hasEntered = false;
        EnterCarEnd.isEntering = false;

        CarSpawner carSpawner = GetComponent<CarSpawner>();
        carSpawner.enabled = false;

        carCamera.enabled = false;
        carCamera.GetComponent<AudioListener>().enabled = false;
        carController.enabled = false;
        light1.enabled = false;
        light2.enabled = false;

        GameObject carDoor = HomeEnterTrigger.FindChildByName(car, "CDoor_FL");
        GameObject animCam = HomeEnterTrigger.FindChildByName(car, "ExitCarAnim");

        Animator animatorDoor = carDoor.GetComponent<Animator>();
        Animator animatorCam = animCam.GetComponent<Animator>();

        animatorDoor.enabled = true;
        animatorDoor.Play("CarEnterDoor", 0, 0f);

        animCam.SetActive(true);
        animatorCam.Play("ExitCar", 0, 0f);

        enterEnd.enabled = true;

        PlayerItems.carPosition = carTransform.position;
        PlayerItems.carRotation = carTransform.rotation;
    }
}
