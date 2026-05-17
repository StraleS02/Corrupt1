using UnityEngine;

public class CutsceneEnd : MonoBehaviour
{
    public GameObject player, cutsceneCam;
    public Animator animator;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        AnimatorStateInfo state = animator.GetCurrentAnimatorStateInfo(0);
        if (state.normalizedTime >= 1f)
        {
            player.SetActive(true);
            cutsceneCam.SetActive(false);
            GameObject trunk = HomeEnterTrigger.FindChildByName(GameObject.Find("Car"), "Car_trunk");
            MonoBehaviour script = trunk.GetComponent<MonoBehaviour>();
            script.enabled = true;
        }
    }
}
