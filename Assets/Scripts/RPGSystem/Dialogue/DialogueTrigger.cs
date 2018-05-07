using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPGSystem
{
    public class DialogueTrigger : MonoBehaviour
    {
        public Dialogue[] dialogues;

        public void TriggerDialogue(int dialogueIndex = 0)
        {
            FindObjectOfType<DialogueManager>().StartDialogue(dialogues[dialogueIndex]);
        }
    }
}
