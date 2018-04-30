using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace RPGSystem
{
    public class InventoryUI : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        public int CellPadding = 10;
        public bool Draggable = true;

        public Image sampleCell;
        public Inventory inventory;
        public GameObject itemPrefab;

        private bool dragging;

        public bool Visible
        {
            set {
                if (inventoryUI != null)
                    inventoryUI.gameObject.SetActive(value);
            }
            get {
                if (inventoryUI != null)
                    return inventoryUI.gameObject.activeSelf;
                return false;
            }
        }

        private Image inventoryUI;
        private Image[,] cells;
        private ItemUI[,] items;

        private void Start()
        {
            if (inventory != null)
                SetInventory(inventory);
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void SetInventory(Inventory inv)
        {
            // wire up the events from the inventory we're viewing, and detach the old one
            if (inventory != null)
                inventory.OnUpdate.RemoveListener(OnItemAdded);
            inventory = inv;
            if (inventory != null)
                inventory.OnUpdate.AddListener(OnItemAdded);

            // fill the inventory screen with items
            Setup();
        }

        void Setup()
        {
            if (cells != null)
            {
                for (int x = 0; x < cells.GetLength(0); x++)
                    for (int y = 0; y < cells.GetLength(1); y++)
                    {
                        Destroy(cells[x, y].gameObject);
                        Destroy(items[x, y].gameObject);
                    }
            }


            cells = new Image[inventory.Width, inventory.Height];
            items = new ItemUI[inventory.Width, inventory.Height];
            sampleCell.gameObject.SetActive(false);
            inventoryUI = sampleCell.transform.parent.GetComponent<Image>();

            if (inventory != null)
            {
                //Resize inventory to fit cells
                inventoryUI.rectTransform.sizeDelta = new Vector2(
                    (CellPadding * (inventory.Width + 1)) + (sampleCell.rectTransform.rect.width * inventory.Width),
                    (CellPadding * (inventory.Height + 1)) + (sampleCell.rectTransform.rect.height * inventory.Height)
                );

                // Add item cells to the inventory
                for (int x = 0; x < inventory.Width; x++)
                {
                    for (int y = 0; y < inventory.Height; y++)
                    {
                        Image nextCell = Instantiate(sampleCell);
                        nextCell.gameObject.SetActive(true);
                        nextCell.gameObject.name = name + "_cell_" + x + "_" + y;
                        nextCell.transform.SetParent(inventoryUI.transform);
                        Vector3 oldPos = sampleCell.transform.position;
                        nextCell.transform.position= new Vector3(
                            oldPos.x + sampleCell.rectTransform.rect.width * x + CellPadding * x,
                            oldPos.y - sampleCell.rectTransform.rect.height * y - CellPadding * y
                        );

                        cells[x, y] = nextCell;
                    }
                }

                // display the items in the inventory
                for (int x = 0; x < inventory.Width; x++)
                {
                    for (int y = 0; y < inventory.Height; y++)
                    {
                        GameObject go = Instantiate(itemPrefab);
                        items[x, y] = go.GetComponent<ItemUI>();
                        items[x, y].SetItem(inventory.GetItem(x, y));
                        items[x, y].image.transform.SetParent(cells[x, y].transform);
                        items[x, y].transform.localPosition = new Vector3(sampleCell.rectTransform.rect.width / 2, -sampleCell.rectTransform.rect.height / 2);
                        items[x, y].image.rectTransform.sizeDelta = new Vector2(
                            sampleCell.rectTransform.rect.width,
                            sampleCell.rectTransform.rect.height
                        );
                        items[x, y].positionInInventory = new Vector2(x, y);
                        items[x, y].UIParent = this;
                    }
                }
            }
        }

        public void OnItemAdded(Item item, int x, int y)
        {
            items[x, y].SetItem(item);
        }

        public bool CanDrop(ItemUI dragged, int x, int y)
        {
            // If the inventory displayed in this UI can not hold the item type
            // exit the function call by returning false
            if (dragged.item != null && (dragged.item.type & inventory.CanHold) == 0) return false;

            //int x = (int)dragged.positionInInventory.x, y = (int)dragged.positionInInventory.y;

            // If the spot the item is being dragged to is empty
            // the item can go in that spot
            if (!items[x, y].item)
                return true;

            // If the spot the item is being dragged into isn't empty
            if (items[x, y].item)
            {
                // If the item in that spot can't go in the other inventory
                // exit the funciton call by returning false
                if ((dragged.UIParent.inventory.CanHold & items[x, y].item.type) == 0) return false;

                // If the item in that spot can go in the other inventory, return true
                else
                    return true;
            }

            // Something funky happened, let's return false
            return false;
        }

        public void Drop(ItemUI dragged, ItemUI dropped)
        {
            // If the other inventory can hold my item, and I can hold the dragged item
            if (dropped.UIParent.CanDrop(dragged, (int)dropped.positionInInventory.x, (int)dropped.positionInInventory.y) &&
                dragged.UIParent.CanDrop(dropped, (int)dragged.positionInInventory.x, (int)dragged.positionInInventory.y))
            {
                Item myItem = dropped.item;
                Item draggedItem = dragged.item;

                if (myItem && draggedItem && myItem.name == draggedItem.name)
                {
                    if (myItem.currentStacks + draggedItem.currentStacks <= myItem.stackLimit)
                    {
                        dropped.item.currentStacks += draggedItem.currentStacks;
                        dragged.UIParent.inventory.SetItem((int)dragged.positionInInventory.x, (int)dragged.positionInInventory.y, null);

                        dropped.UIParent.inventory.OnUpdate.Invoke(myItem, (int)dropped.positionInInventory.x, (int)dropped.positionInInventory.y);
                    }
                    else if (myItem.currentStacks + draggedItem.currentStacks > myItem.stackLimit)
                    {
                        draggedItem.currentStacks -= (myItem.stackLimit - myItem.currentStacks);
                        myItem.currentStacks = myItem.stackLimit;

                        // Manually update the inventories as we are directly modifying the items
                        dragged.UIParent.inventory.OnUpdate.Invoke(draggedItem, (int)dragged.positionInInventory.x, (int)dragged.positionInInventory.y);
                        dropped.UIParent.inventory.OnUpdate.Invoke(myItem, (int)dropped.positionInInventory.x, (int)dropped.positionInInventory.y);
                    }
                }
                else
                {
                    dragged.UIParent.inventory.SetItem((int)dragged.positionInInventory.x, (int)dragged.positionInInventory.y, myItem);
                    dropped.UIParent.inventory.SetItem((int)dropped.positionInInventory.x, (int)dropped.positionInInventory.y, draggedItem);
                }
            }
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (Draggable)
            {
                transform.SetAsLastSibling();
                bool mouseOverInventory = false;
                List<RaycastResult> hitObjects = new List<RaycastResult>();

                EventSystem.current.RaycastAll(eventData, hitObjects);
                if (hitObjects.Count == 1)
                    mouseOverInventory = hitObjects[0].gameObject.GetComponent<InventoryUI>();

                dragging = mouseOverInventory;
            }
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (dragging)
                transform.position += new Vector3(eventData.delta.x, eventData.delta.y);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (!dragging) return;
            dragging = false;
        }
    }
}
