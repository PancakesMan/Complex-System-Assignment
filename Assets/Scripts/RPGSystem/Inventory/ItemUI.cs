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

        [HideInInspector]
        public Vector2 positionInInventory = Vector2.zero;
        [HideInInspector]
        public InventoryUI UIParent;

        public void SetItem(Item _item)
        {
            item = _item;
            UIParent.inventory.SetItem((int)positionInInventory.x, (int)positionInInventory.y, _item);

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
                if (itemStackCount)
                    itemStackCount.gameObject.SetActive(false);
            }
        }
    }
}
