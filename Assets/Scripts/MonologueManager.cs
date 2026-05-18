using System.Collections;
using TMPro;
using UnityEngine;

public class MonologueManager : MonoBehaviour
{
    public TMP_Text dialogueText, instructionsText;
    private int currentIndex = 0,
                currentIndexInstruction = 0;
    private bool isDialogueActive = false;
    private bool isInstructionActive = false;
    private Coroutine coroutine, coroutineInstruction;
    private readonly float fadeDuration = 0.5f;
    public static bool isFlashPickedUp = false;

    public string[] sentences = new string[]
    {
        "Press E to show next sentence.",
        "I feel so tired.",
        "Can't do this anymore.",
        "If I could just run away from everything.",
        "Gonna go home and try to sleep for a bit."
    };

    public string[] instructions = new string[]
    {

    };

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        dialogueText.text = "";
    }

    // Update is called once per frame
    void Update()
    {
        if(isDialogueActive && Input.GetKeyDown(KeyCode.E))
        {
            ShowNextSentence();
        }
    }

    public void StartMonologue()
    {
        currentIndex = 0;
        isDialogueActive = true;

        if(coroutine != null) StopCoroutine(coroutine);
        coroutine = StartCoroutine(FadeInSentence(sentences[currentIndex]));

        //dialogueText.text = sentences[currentIndex];
    }

    public void StartInstructions()
    {
        currentIndexInstruction = 0;
        isInstructionActive = true;

        if (coroutineInstruction != null) StopCoroutine(coroutineInstruction);
        coroutineInstruction = StartCoroutine(FadeInInstruction(instructions[currentIndexInstruction]));

        // Automatski startuj korutinu koja ce gasiti instrukciju posle 10s
        StartCoroutine(AutoFadeOutInstructionsAfterDelay(10f));
    }

    IEnumerator AutoFadeOutInstructionsAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (isInstructionActive) // Ako u međuvremenu nije već ugašena
        {
            if (coroutineInstruction != null) StopCoroutine(coroutineInstruction);
            coroutineInstruction = StartCoroutine(FadeOutInstruction());
        }
    }

    void ShowNextSentence()
    {
        currentIndex++;

        if(currentIndex < sentences.Length)
        {
            if (coroutine != null) StopCoroutine(coroutine);
            coroutine = StartCoroutine(FadeTransition(sentences[currentIndex]));
            //dialogueText.text = sentences[currentIndex];
        }
        else
        {
            if(coroutine != null) StopCoroutine(coroutine);
            coroutine = StartCoroutine(FadeOutAndEnd());
            //EndMonologue();
        }
    }

    public void ShowNextInstruction()
    {
        currentIndexInstruction++;

        if (currentIndexInstruction < instructions.Length)
        {
            if (coroutineInstruction != null) StopCoroutine(coroutineInstruction);
            coroutineInstruction = StartCoroutine(FadeTransitionInstruction(instructions[currentIndexInstruction]));
            StartCoroutine(AutoFadeOutInstructionsAfterDelay(10f));
            //dialogueText.text = sentences[currentIndex];
        }
        else
        {
            if (coroutineInstruction != null) StopCoroutine(coroutineInstruction);
            coroutineInstruction = StartCoroutine(FadeOutAndEndInstruction());
            //EndMonologue();
        }
        
    }

    void EndMonologue()
    {
        isDialogueActive = false;
        dialogueText.text = "";
    }

    IEnumerator FadeTransition(string nextText)
    {
        yield return StartCoroutine(FadeOut());
        dialogueText.text = nextText;
        yield return StartCoroutine(FadeIn());
    }

    IEnumerator FadeTransitionInstruction(string nextText)
    {
        yield return StartCoroutine(FadeOutInstruction());
        instructionsText.text = nextText;
        yield return StartCoroutine(FadeInInstructionAlpha());
    }

    IEnumerator FadeInSentence(string text)
    {
        dialogueText.text = text;
        yield return StartCoroutine(FadeIn());
    }

    IEnumerator FadeInInstruction(string text)
    {
        instructionsText.text = text;
        yield return StartCoroutine(FadeInInstructionAlpha());
    }

    IEnumerator FadeIn()
    {
        float timer = 0f;
        while (timer <= fadeDuration)
        {
            float alpha = timer / fadeDuration;
            SetTextAlpha(alpha);
            timer += Time.deltaTime;
            yield return null;
        }
        SetTextAlpha(1f);
    }

    IEnumerator FadeOut()
    {
        float timer = 0f;
        while (timer <= fadeDuration)
        {
            float alpha = 1f - (timer / fadeDuration);
            SetTextAlpha(alpha);
            timer += Time.deltaTime;
            yield return null;
        }
        SetTextAlpha(0f);
    }

    IEnumerator FadeOutAndEnd()
    {
        yield return StartCoroutine(FadeOut());
        dialogueText.text = "";
        isDialogueActive = false;
    }

    IEnumerator FadeOutAndEndInstruction()
    {
        yield return StartCoroutine(FadeOutInstruction());
        instructionsText.text = "";
        isInstructionActive = false;
    }

    void SetTextAlpha(float alpha)
    {
        Color color = dialogueText.color;
        color.a = alpha;
        dialogueText.color = color;
    }

    void SetInstructionAlpha(float alpha)
    {
        Color color = instructionsText.color;
        color.a = alpha;
        instructionsText.color = color;
    }

    IEnumerator FadeOutInstruction()
    {
        float timer = 0f;
        while (timer <= fadeDuration)
        {
            float alpha = 1f - (timer / fadeDuration);
            SetInstructionAlpha(alpha);
            timer += Time.deltaTime;
            yield return null;
        }
        SetInstructionAlpha(0f);
    }

    IEnumerator FadeInInstructionAlpha()
    {
        float timer = 0f;
        while (timer <= fadeDuration)
        {
            float alpha = timer / fadeDuration;
            SetInstructionAlpha(alpha);
            timer += Time.deltaTime;
            yield return null;
        }
        SetInstructionAlpha(1f);
    }

    public IEnumerator ShowTemporaryDialogue(string text, float duration)
    {
        dialogueText.text = text;

        yield return StartCoroutine(FadeIn());

        yield return new WaitForSeconds(duration);

        yield return StartCoroutine(FadeOut());

        dialogueText.text = "";
    }
}
