using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPGSystem
{
    public class DialogueTrigger : MonoBehaviour
    {
        public Dialogue dialogue;

        public void TriggerDialogue()
        {
            FindObjectOfType<DialogueManager>().StartDialogue(dialogue);
        }
    }
}
