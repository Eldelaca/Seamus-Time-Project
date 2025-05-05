    using System;
    using System.Collections;
    using TMPro;
    using UnityEngine;

    public class DialogueController : MonoBehaviour
    {
        public static DialogueController instance;
        public bool conversationOver;
        
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

        public void DisplayDialogue(string[] dialogue, int startPosition, DialogueTrigger dialogueTrigger)
        {
            dialogueBox.SetActive(true);
            
            StopAllCoroutines();
            StartCoroutine(RunDialogue(dialogue, startPosition, dialogueTrigger));
        }

        IEnumerator RunDialogue(string[] dialogue, int startPosition, DialogueTrigger dialogueTrigger)
        {
            conversationOver = false;

            foreach (var t in dialogue)
            {
                dialogueText.text = t;

                while (!skipLine)
                {
                    yield return null;
                }

                skipLine = false;
            }

            conversationOver = true;
            EndDialogue();

            if (dialogueTrigger != null && dialogueTrigger.destroyOnDialogueEnd)
            {
                Destroy(dialogueTrigger.gameObject);
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
