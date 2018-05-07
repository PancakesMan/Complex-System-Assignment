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

        private GameObject TooltipObject;

        // Use this for initialization
        void Start()
        {
            // Set static UITooltip for other scripts to access
            instance = this;
            instance.gameObject.SetActive(false);

            // Allow clicks to go trough the Tooltip
            background.raycastTarget = false;
            text.raycastTarget = false;
        }

        // Update is called once per frame
        void Update()
        {
            // If the Tooltip is currently being displayed
            if (gameObject.activeSelf)
            {
                // Move it to the position of the mouse
                SetPosition(Input.mousePosition);

                // if the object the tooltip is for is no longer active
                if (!TooltipObject.activeInHierarchy)
                    // Disable the tooltip
                    gameObject.SetActive(false);
            }
        }

        public void SetTooltipObject(GameObject obj)
        {
            TooltipObject = obj;
        }

        public void SetText(string msg)
        {
            // trim whitespace at the end
            while (msg.EndsWith("\n") || msg.EndsWith(" "))
                msg = msg.Remove(msg.Length - 1);

            // If the text we're changing to isn't the same
            if (text.text != msg)
            {
                text.text = msg;

                // Size the Tooltip to hold the text we've given it
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
            // Move the tooltip's position
            background.transform.position = new Vector3(
                position.x + background.rectTransform.sizeDelta.x / 2,
                position.y + background.rectTransform.sizeDelta.y / 2
            );

            // Make it draw over everything else
            transform.SetAsLastSibling();
        }
    }
}
