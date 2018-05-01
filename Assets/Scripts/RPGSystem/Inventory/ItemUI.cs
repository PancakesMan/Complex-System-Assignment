using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace RPGSystem
{
    public class ItemUI : DropTarget, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler
    {
        public Item item;
        public Image image;
        public Text itemStackCount;

        [HideInInspector]
        public Vector2 positionInInventory = Vector2.zero;
        [HideInInspector]
        public InventoryUI UIParent;

        private Vector3 originalPosition;
        private bool dragging = false;

        public void Start()
        {
            if (itemStackCount)
                itemStackCount.raycastTarget = false;
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
                if (itemStackCount)
                    itemStackCount.gameObject.SetActive(false);
            }
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            // Save our start position
            originalPosition = transform.position;

            // Move this and parent to front of UI Canvas, so they are
            // drawn over everything else on the canvas
            Transform t = transform;
            while (t != null)
            {
                t.SetAsLastSibling();
                t = t.parent;
            }

            // Make it ignored by raycasts so we can get
            // the objects under it
            GetComponent<Image>().raycastTarget = false;

            // Set dragging to true so we can be moved
            dragging = true;
            UITooltip.instance.gameObject.SetActive(false);
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (dragging)
                transform.position = eventData.position;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (!dragging) return;

            ItemUI target = null;
            List<RaycastResult> hitObjects = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventData, hitObjects);
            if (hitObjects.Count > 0)
            {
                DropTarget dt = hitObjects[0].gameObject.GetComponent<DropTarget>();
                if (dt)
                    target = dt.GetComponentInChildren<ItemUI>(true);
            }
            else
            {
                if (item.modelPrefab)
                {
                    //TODO Drop item on the ground
                    GameObject droppedItem = Instantiate(item.modelPrefab);
                    droppedItem.GetComponent<ItemInstance>().SetItem(item, UIParent.inventory.transform);
                }
                

                // Remove item from player inventory
                UIParent.inventory.SetItem((int)positionInInventory.x, (int)positionInInventory.y, null);
            }

            if (target && target != this)
            {
                if (target.UIParent.CanDrop(this, (int)target.positionInInventory.x, (int)target.positionInInventory.y))
                {
                    target.UIParent.Drop(this, target);
                }
            }

            transform.position = originalPosition;
            GetComponent<Image>().raycastTarget = true;
            dragging = false;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (!dragging)
            {
                UITooltip.instance.SetTooltipObject(gameObject);
                UITooltip.instance.SetText("<b>" + item.name + "</b>\n\n" + item.description);
                UITooltip.instance.SetPosition(eventData.position);
                UITooltip.instance.gameObject.SetActive(true);
            }
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            UITooltip.instance.gameObject.SetActive(false);
        }
    }
}
