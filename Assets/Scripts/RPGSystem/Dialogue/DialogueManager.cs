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
            // Add listeners to forward and back buttons to
            // move through dialogue when they're clicked
            if (Previous)
                Previous.onClick.AddListener(DisplayPreviousSentence);
            if (Next)
                Next.onClick.AddListener(DisplayNextSentence);
        }

        public void StartDialogue(Dialogue dialogue)
        {
            // store dialogue internally and set start index to -1
            _dialogue = dialogue;
            index = -1;

            // Make dialogue box display over everything and activate it
            DialogueBox.transform.SetAsLastSibling();
            DialogueBox.gameObject.SetActive(true);
            
            // Prevent moving when reading dialogue
            Time.timeScale = 0;

            // Set the name of the person talking in the dialogue box
            Name.text = dialogue.name;

            // Display the first sentence of the dialogue
            DisplayNextSentence();
        }

        public void DisplayNextSentence()
        {
            // Increase index
            index++;

            // If the dialogue is finished
            if (_dialogue.sentences.Length == index)
            {
                // Disable the dialogue box
                DialogueBox.gameObject.SetActive(false);

                // Fire the DialogueCompleted event for the dialogue
                _dialogue.OnDialogueCompleted.Invoke();

                // Allow player movement again
                Time.timeScale = 1;
                return;
            }

            // Enable/Disable buttons based on dialogue progress
            SetDialogueButtonsStates();

            // Stop typing the current sentence if it's still ongoing
            StopAllCoroutines();

            // Start typing the next sentence
            StartCoroutine(DisplaySentence(_dialogue.sentences[index]));
        }

        public void DisplayPreviousSentence()
        {
            // Decrease index
            index--;

            // Enable/Disable buttons based on dialogue progress
            SetDialogueButtonsStates();

            // Stop typing the current sentence if it's still ongoing
            StopAllCoroutines();

            // Start typing the previous sentence
            StartCoroutine(DisplaySentence(_dialogue.sentences[index]));
        }

        IEnumerator DisplaySentence(string sentence)
        {
            // Clear the text component of the Dialogue Box
            DialogueText.text = "";

            // For every letter in the current sentence
            foreach (char letter in sentence)
            {
                // Add it to the text component and wait a single frame
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
        }
    }
}
