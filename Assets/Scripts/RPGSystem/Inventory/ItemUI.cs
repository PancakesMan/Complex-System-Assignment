using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RPGSystem
{
    public class ItemUI : MonoBehaviour
    {
        public Item item;
        public Image image;
        public Text itemStackCount;

        // Use this for initialization
        void Start()
        {
            //SetItem(null);
        }

        // Update is called once per frame
        void Update()
        {
            if (item && itemStackCount)
            {
                image.gameObject.SetActive(true);
                itemStackCount.text = item.currentStacks.ToString();
            }
        }

        public void SetItem(Item _item)
        {
            item = _item;
            if (item)
            {
                if (image)
                {
                    image.gameObject.SetActive(true);
                    image.sprite = item.sprite;
                }
                if (itemStackCount)
                {
                    itemStackCount.text = item.currentStacks.ToString();
                    itemStackCount.gameObject.SetActive(item.stackLimit > 1);
                }
            }
            else
            {
                if (image)
                    image.gameObject.SetActive(false);
            }
        }
    }
}
