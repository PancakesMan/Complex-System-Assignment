using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RPGSystem
{
    public class UITooltip : MonoBehaviour
    {
        public static UITooltip instance;
        public Image background;
        public Text text;

        // Use this for initialization
        void Start()
        {
            instance = this;
            instance.gameObject.SetActive(false);
            background.raycastTarget = false;
            text.raycastTarget = false;
        }

        // Update is called once per frame
        void Update()
        {
            if (gameObject.activeSelf)
                SetPosition(Input.mousePosition);
        }

        public void SetText(string msg)
        {
            // trim whitespace at the end
            while (msg.EndsWith("\n") || msg.EndsWith(" "))
                msg = msg.Remove(msg.Length - 1);

            if (text.text != msg)
            {
                text.text = msg;

                // size it to hold the text we've given it
                TextGenerator textGen = new TextGenerator();
                TextGenerationSettings generationSettings = text.GetGenerationSettings(background.rectTransform.sizeDelta);
                float width = textGen.GetPreferredWidth(msg, generationSettings);
                float height = textGen.GetPreferredHeight(msg, generationSettings);

                background.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, width + 20);
                background.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height + 20);
                text.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, width);
                text.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height);
            }
        }

        public void SetPosition(Vector2 position)
        {
            background.transform.position = new Vector3(
                position.x + background.rectTransform.sizeDelta.x / 2,
                position.y + background.rectTransform.sizeDelta.y / 2
            );
            transform.SetAsLastSibling();
        }
    }
}
