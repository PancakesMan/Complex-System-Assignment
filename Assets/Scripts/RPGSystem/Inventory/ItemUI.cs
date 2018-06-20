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
            // If the ItemUI has a text component for
            // displaying the item stack count
            if (itemStackCount)
                // Make raycasts go through the text component
                itemStackCount.raycastTarget = false;
        }

        public void SetItem(Item _item)
        {
            item = _item;

            // If item is not null
            if (item)
            {
                // If image of ItemUI is not null
                if (image)
                {
                    // Make the image visible and set it's sprite
                    image.gameObject.SetActive(true);
                    image.sprite = item.sprite;
                }

                // If itemStackCount of ItemUI is not null
                if (itemStackCount)
                {
                    // Set the text of the object
                    // And make it visible if the item can stack
                    itemStackCount.text = item.currentStacks.ToString();
                    itemStackCount.gameObject.SetActive(item.stackLimit > 1);
                }
            }
            else // If the item is null
            {
                // Deactivate the existing Image component
                if (image)
                    image.gameObject.SetActive(false);

                // Deactivate the exisiting Text object
                if (itemStackCount)
                    itemStackCount.gameObject.SetActive(false);
            }
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            // Save our start position
            originalPosition = transform.position;
            UIParent.draggedItem = this;

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
            UIParent.draggedItem = null;

            ItemUI target = null;
            List<RaycastResult> hitObjects = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventData, hitObjects);
            if (hitObjects.Count > 0)
            {
                // If we hit an object while ending the drag of an item
                // Check if the object is a valid drop target
                DropTarget dt = hitObjects[0].gameObject.GetComponent<DropTarget>();
                if (dt)
                    target = dt.GetComponentInChildren<ItemUI>(true);
            }
            else
            {
                // If we dropped the item over nothing
                if (item.modelPrefab)
                {
                    // Spawn the item in the scene if it has a model
                    UIParent.inventory.GetComponent<Animator>().SetTrigger("Throw Dice");
                    UIParent.inventory.gameObject.GetComponent<UserKeyBindings>().StartDropItemCoroutine(item, UIParent.inventory.transform, 1.8f);
                    //GameObject droppedItem = Instantiate(item.modelPrefab);
                    //droppedItem.GetComponent<ItemInstance>().SetItem(item, UIParent.inventory.transform);
                }
                
                // Remove item from player inventory
                UIParent.inventory.SetItem((int)positionInInventory.x, (int)positionInInventory.y, null);
            }

            // If the drop target is not null and not the current ItemUI object
            if (target && target != this)
            {
                if (target.UIParent.CanDrop(this, (int)target.positionInInventory.x, (int)target.positionInInventory.y))
                {
                    // Drop the item in the target if it will take the item
                    target.UIParent.Drop(this, target);
                }
            }

            // Reset the ItemUI's transform
            // and make it a raycast target
            transform.position = originalPosition;
            GetComponent<Image>().raycastTarget = true;
            dragging = false;
        }

        IEnumerator DropItem(Item item, Transform transform, int seconds)
        {
            yield return new WaitForSecondsRealtime(seconds);

            GameObject droppedItem = Instantiate(item.modelPrefab);
            droppedItem.GetComponent<ItemInstance>().SetItem(item, transform);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (!dragging)
            {
                // If we mouseOver this ItemUI and it's not currently being dragged
                // Setup the UITooltip object and activate it
                UITooltip.instance.SetTooltipObject(gameObject);
                UITooltip.instance.SetText("<b>" + item.name + "</b>\n\n" + item.description);
                UITooltip.instance.SetPosition(eventData.position);
                UITooltip.instance.gameObject.SetActive(true);
            }
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            // When the mouse leaves the ItemUI deactivate the UITooltip
            UITooltip.instance.gameObject.SetActive(false);
        }
    }
}
