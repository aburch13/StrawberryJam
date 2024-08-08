using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogueBox : MonoBehaviour
{
    public static DialogueBox instance;
    [SerializeField] TextMeshProUGUI dialogueText;
    [SerializeField] TextMeshProUGUI nameText;
    CanvasGroup dialogueBox;
    UIPopUp effect;

    public static event Action OnDialogueStarted;
    public static event Action OnDialogueEnded;
    bool skipLineTriggered;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    private void Start()
    {
        dialogueBox = transform.parent.gameObject.GetComponent<CanvasGroup>();
        effect = transform.parent.gameObject.GetComponent<UIPopUp>();
    }

    public void StartDialogue(string[] dialogue, int startPosition, string name)
    {
        nameText.text = name + "...";
        effect.PopUp();
        StopAllCoroutines();
        StartCoroutine(RunDialogue(dialogue, startPosition));
    }

    IEnumerator RunDialogue(string[] dialogue, int startPosition)
    {
        skipLineTriggered = false;
        OnDialogueStarted?.Invoke();

        for (int i = startPosition; i < dialogue.Length; i++)
        {
            dialogueText.text = dialogue[i];
            while (skipLineTriggered == false)
            {
                // Wait for the current line to be skipped
                yield return null;
            }
            skipLineTriggered = false;
        }

        OnDialogueEnded?.Invoke();
        dialogueBox.gameObject.SetActive(false);
    }

    public void SkipLine()
    {
        skipLineTriggered = true;
    }
}
