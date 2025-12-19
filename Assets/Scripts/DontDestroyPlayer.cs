using UnityEngine;

public class DontDestroyPlayer : MonoBehaviour
{
    private static bool playerExists = false;

    public GameObject postProcessingVolume;

    void Awake()
    {
        if (!playerExists)
        {
            DontDestroyOnLoad(gameObject);
            //if(postProcessingVolume != null)
                //DontDestroyOnLoad(postProcessingVolume);

            playerExists = true;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
