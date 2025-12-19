using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FadeController : MonoBehaviour
{
    public static FadeController Instance;
    private Image fadeImage;

    void Awake()
    {
        Instance = this;
        fadeImage = GetComponent<Image>();
    }

    public IEnumerator FadeOut(float duration)
    {
        fadeImage.enabled = true;
        Color c = fadeImage.color;
        float t = 0f;
        while (t < duration)
        {
            t += Time.deltaTime;
            c.a = Mathf.Lerp(0, 1, t / duration);
            fadeImage.color = c;
            yield return null;
        }
        c.a = 1;
        fadeImage.color = c;
        fadeImage.enabled = false;
    }

    public IEnumerator FadeIn(float duration)
    {
        Color c = fadeImage.color;
        float t = 0f;
        while (t < duration)
        {
            t += Time.deltaTime;
            c.a = Mathf.Lerp(1, 0, t / duration);
            fadeImage.color = c;
            yield return null;
        }
        c.a = 0;
        fadeImage.color = c;
    }
}
