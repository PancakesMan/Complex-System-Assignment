using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace RPGSystem
{
    [System.Serializable]
    public class Dialogue
    {
        [System.Serializable]
        public class DialogueCompletedEvent : UnityEvent { }

        public string name;

        [TextArea(3, 10)]
        public string[] sentences;

        public DialogueCompletedEvent OnDialogueCompleted;
    }
}
