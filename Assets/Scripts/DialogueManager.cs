using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using System.Collections;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;

    public GameObject dialoguePanel;
    public TMP_Text dialogueText;
    public float letterDelay = 0.05f;

    private Coroutine current;
    private void Awake()
    {
        if (Instance == null) Instance = this;
        else { Destroy(gameObject); return; }

        dialoguePanel.SetActive(false);
    }

    public void StartDialogue(string text)
    {
        if (current != null) StopCoroutine(current);
        current = StartCoroutine(ShowThenHide(text));
    }

    private IEnumerator ShowThenHide(string fullText)
    {
        dialoguePanel.SetActive(true);
        dialogueText.text = "";

        // efeito “type-writer”
        foreach (char c in fullText)
        {
            dialogueText.text += c;
            yield return new WaitForSeconds(letterDelay);
        }

        // espera a tecla Enter do novo Input System
        yield return new WaitUntil(() =>
            Keyboard.current != null &&
            Keyboard.current.enterKey.wasPressedThisFrame
        );

        dialoguePanel.SetActive(false);
    }
}
