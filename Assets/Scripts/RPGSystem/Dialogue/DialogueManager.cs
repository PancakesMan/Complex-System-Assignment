using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RPGSystem
{
    public class DialogueManager : MonoBehaviour
    {
        private Queue<string> sentences;

        public Image DialogueBox;
        public Text Name;
        public Text DialogueText;

        // Use this for initialization
        void Start()
        {
            sentences = new Queue<string>();
        }

        public void SetDialogue(Dialogue dialogue)
        {
            sentences.Clear();
            foreach (string sentence in dialogue.sentences)
                sentences.Enqueue(sentence);

            DialogueBox.gameObject.SetActive(true);
            DialogueBox.transform.SetAsLastSibling();
            Name.text = dialogue.name;
            StartCoroutine(DisplaySentence(sentences.Dequeue()));
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

        IEnumerator DisplaySentence(string sentence)
        {
            DialogueText.text = "";
            foreach (char letter in sentence)
            {
                DialogueText.text += letter;
                yield return null;
            }
        }
    }
}
