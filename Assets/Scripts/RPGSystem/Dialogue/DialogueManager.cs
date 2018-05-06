using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RPGSystem
{
    public class DialogueManager : MonoBehaviour
    {
        private Dialogue _dialogue;
        private int index;

        public Image DialogueBox;
        public Text Name;
        public Text DialogueText;
        public Button Previous, Next;

        // Use this for initialization
        void Start()
        {
            if (Previous)
                Previous.onClick.AddListener(DisplayPreviousSentence);

            if (Next)
                Next.onClick.AddListener(DisplayNextSentence);
        }

        public void SetDialogue(Dialogue dialogue)
        {
            _dialogue = dialogue;
            index = -1;

            DialogueBox.transform.SetAsLastSibling();
            DialogueBox.gameObject.SetActive(true);
            
            Time.timeScale = 0;
            Name.text = dialogue.name;
            DisplayNextSentence();
        }

        public void DisplayNextSentence()
        {
            index++;
            if (_dialogue.sentences.Length == index)
            {
                DialogueBox.gameObject.SetActive(false);
                _dialogue.OnDialogueCompleted.Invoke();
                Time.timeScale = 1;
                return;
            }

            SetDialogueButtonsStates();
            StopAllCoroutines();
            StartCoroutine(DisplaySentence(_dialogue.sentences[index]));
        }

        public void DisplayPreviousSentence()
        {
            index--;
            SetDialogueButtonsStates();
            StopAllCoroutines();
            StartCoroutine(DisplaySentence(_dialogue.sentences[index]));
        }

        IEnumerator DisplaySentence(string sentence)
        {
            DialogueText.text = "";
            foreach (char letter in sentence)
            {
                DialogueText.text += letter;
                yield return null;
            }
        }

        private void SetDialogueButtonsStates()
        {
            // Disable the back button if we're reading the first sentence
            Previous.gameObject.SetActive(index != 0);

            // Change text of Next button to Ok if we're reading the last sentence
            Next.GetComponentInChildren<Text>().text = _dialogue.sentences.Length - 1 == index ? "Ok" : "Next";
            // TODO Check if Dialogue has a quest
        }
    }
}
