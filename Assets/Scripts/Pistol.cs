using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class Pistol : MonoBehaviour
{
    public GameObject pistolFire;
    public Animator recoil;
    public PostProcessVolume volume;
    private DepthOfField dof;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    private void OnEnable()
    {
        volume.profile.TryGetSettings(out dof);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown((int)MouseButton.Left))
        {
            StartCoroutine(GunFire());
        }
    }

    IEnumerator GunFire()
    {
        
        pistolFire.SetActive(true);
        recoil.enabled = true;
        recoil.Play("PistolFire", 0, 0f);
        StartCoroutine(ChangeFocalLength(97f, 250f, 0.1f));
        float length = recoil.GetCurrentAnimatorStateInfo(0).length;
        yield return new WaitForSeconds(length);
        StartCoroutine(ChangeFocalLength(250f, 97f, 0.2f));
        recoil.enabled=false;
        pistolFire.SetActive(false);
    }

    IEnumerator ChangeFocalLength(float from, float to, float duration)
    {
        float t = 0f;
        while (t < duration)
        {
            dof.focalLength.value = Mathf.Lerp(from, to, t / duration);
            t += Time.deltaTime;
            yield return null;
        }
        dof.focalLength.value = to;
    }

}
