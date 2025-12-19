using UnityEngine;

public class Lamp : MonoBehaviour
{
    public Light lamp;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        lamp.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(!lamp.enabled && Input.GetMouseButtonDown(1))
        {
            lamp.enabled = true;
        }
        else if (lamp.enabled && Input.GetMouseButtonDown(1))
        {
            lamp.enabled = false;
        }
    }
}
