using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RPGSystem
{
    public class InventoryUI : MonoBehaviour
    {
        public int CellPadding = 10;

        public Image sampleCell;
        public Inventory inventory;
        public GameObject itemPrefab;

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

        void SetInventory(Inventory inv)
        {
            // wire up the events from the inventory we're viewing, and detach the old one
            if (inventory != null)
                inventory.OnAdd.RemoveListener(OnItemAdded);
            inventory = inv;
            if (inventory != null)
                inventory.OnAdd.AddListener(OnItemAdded);

            // fill the inventory screen with items
            Setup();
        }

        void Setup()
        {
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
                        items[x, y].item = inventory.GetItem(x, y);
                        items[x, y].image.transform.SetParent(cells[x, y].transform);
                        items[x, y].transform.localPosition = new Vector3(sampleCell.rectTransform.rect.width / 2, -sampleCell.rectTransform.rect.height / 2);
                    }
                }
            }
        }

        public void OnItemAdded(Item item, int x, int y)
        {
            items[x, y].SetItem(item);
        }
    }
}
