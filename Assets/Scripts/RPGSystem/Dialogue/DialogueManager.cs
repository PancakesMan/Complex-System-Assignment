using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour {

    private Queue<string> sentences;

    public Text DialogueTextObject;

	// Use this for initialization
	void Start () {
        sentences = new Queue<string>();
	}
	
	public void SetDialogue(Dialogue dialogue)
    {
        sentences.Clear();
        foreach (string sentence in dialogue.sentences)
            sentences.Enqueue(sentence);

        DialogueTextObject.gameObject.SetActive(true);
        StartDialogue();
    }

    void StartDialogue()
    {
        if (sentences.Count == 0)
            DialogueTextObject.gameObject.SetActive(false);

        StopAllCoroutines();
        StartCoroutine(DisplaySentence(sentences.Dequeue()));
    }

    IEnumerator DisplaySentence(string sentence)
    {
        foreach (char letter in sentence)
        {
            DialogueTextObject.text += letter;
            yield return null;
        }
    }
}
