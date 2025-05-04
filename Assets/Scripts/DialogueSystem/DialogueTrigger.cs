using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public PlayerMovement playerMovement;
    public DialogueAsset dialogueAsset;

    public bool destroyOnDialogueEnd = true;

    private int StartPosition
    {
        get
        {
            if (firstConversation)
            {
                firstConversation = false;
                return 0;
            }
            else
            {
                return repeatDialoguePosition;
            }
        }
    }
    
    
    private bool inConversation;
    private bool firstConversation;
    public int repeatDialoguePosition;

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && CompanionAI.instance.inPlace)
        {
            if (!inConversation)
            {
                DialogueController.instance.DisplayDialogue(dialogueAsset.dialogue, StartPosition, this);
                inConversation = true;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            DialogueController.instance.EndDialogue();
            inConversation = false;
        }
    }

}
