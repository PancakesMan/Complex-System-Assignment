using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RPGSystem
{
    public class DialogueManager : MonoBehaviour
    {
        private Queue<string> sentences;
        private Dialogue _dialogue;
        private int index;

        public Image DialogueBox;
        public Text Name;
        public Text DialogueText;
        public Button Previous, Next, Accept, Decline;

        // Use this for initialization
        void Start()
        {
            sentences = new Queue<string>();
            if (Previous)
                Previous.onClick.AddListener(DisplayPreviousSentence);

            if (Next)
                Next.onClick.AddListener(DisplayNextSentence);
        }

        public void SetDialogue(Dialogue dialogue)
        {
            sentences.Clear();
            foreach (string sentence in dialogue.sentences)
                sentences.Enqueue(sentence);

            _dialogue = dialogue;
            index = -1;

            DialogueBox.transform.SetAsLastSibling();
            DialogueBox.gameObject.SetActive(true);

            //Previous.gameObject.SetActive(false);
            
            Time.timeScale = 0;
            Name.text = dialogue.name;
            //StartCoroutine(DisplaySentence(sentences.Dequeue()));
            DisplayNextSentence();
        }

        public void AdvanceDialogue()
        {
            if (sentences.Count == 0)
            {
                DialogueBox.gameObject.SetActive(false);
                //
                Time.timeScale = 1;
                return;
            }

            StopAllCoroutines();
            StartCoroutine(DisplaySentence(sentences.Dequeue()));
        }

        public void DisplayNextSentence()
        {
            index++;
            if (_dialogue.sentences.Length == index)
            {
                DialogueBox.gameObject.SetActive(false);
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
            if (index == 0)
                Previous.gameObject.SetActive(false);
            else
                Previous.gameObject.SetActive(true);

            if (_dialogue.sentences.Length - 1 == index)
            {
                // TODO check if Dialogue has a quest
                Next.GetComponentInChildren<Text>().text = "Ok";
            }
            else
                Next.GetComponentInChildren<Text>().text = "Next";
        }
    }
}
