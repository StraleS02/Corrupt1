using UnityEngine;

public class EnterCarEnd : MonoBehaviour
{
    public GameObject enteringCam, exitingCam, player;
    public Animator door;
    public Camera carCam;
    public MonoBehaviour carController;
    public GameObject uiPrompt;
    public static bool isEntering;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        AnimatorStateInfo stateDoor = door.GetCurrentAnimatorStateInfo(0);

        if (isEntering)
        {
            if (stateDoor.normalizedTime >= 1f)
            {
                enteringCam.SetActive(false);
                door.enabled = false;
                carCam.enabled = true;
                carCam.GetComponent<AudioListener>().enabled = true;
                carController.enabled = true;
                CarSpawner carSpawner = GetComponent<CarSpawner>();
                carSpawner.enabled = true;

                this.enabled = false;
            }
        }
        else
        {
            if(stateDoor.normalizedTime >= 1f)
            {
                exitingCam.SetActive(false);
                door.enabled = false;

                GameObject car = GameObject.Find("Car");
                Transform carTransform = car.GetComponent<Transform>();

                //GameObject player = GameObject.Find("Player");
                
                //Transform playerTransform = player.GetComponent<Transform>();

                Vector3 exitOffset = carTransform.right * 3f;  // 2 metra desno od auta
                Vector3 exitPosition = carTransform.position - exitOffset;

                player.transform.position = new Vector3(exitPosition.x, player.transform.position.y, exitPosition.z);
                uiPrompt.SetActive(true);
                player.SetActive(true);

                this.enabled = false;
            }
        }
    }
}
