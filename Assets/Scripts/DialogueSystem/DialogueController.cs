using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class DialogueController : MonoBehaviour
{
    public static DialogueController instance;
    
    [SerializeField] private TMP_Text dialogueText;
    [SerializeField] private GameObject dialogueBox;
    
    private bool skipLine;

    public void Awake()
    {
        instance = this;
    }

    public void Start()
    {
        dialogueBox.SetActive(false);
    }

    public void DisplayDialogue(string[] dialogue, int startPosition)
    {
        dialogueBox.SetActive(true);
        
        StopAllCoroutines();
        StartCoroutine(RunDialogue(dialogue, startPosition));
    }

    IEnumerator RunDialogue(string[] dialogue, int startPosition)
    {
        for (int i = 0; i < dialogue.Length; i++)
        {
            dialogueText.text = dialogue[i];
            
            while (!skipLine)
            {
                //waiting for the line to be skipped
                yield return null;
            }
            
            skipLine = false;
        }
    }

    public void SkipLine()
    {
        if (dialogueText.text != null) skipLine = true;
    }

    public void EndDialogue()
    {
        dialogueText.text = null;
        dialogueBox.SetActive(false);
    }
}
