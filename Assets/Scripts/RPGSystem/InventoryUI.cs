using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RPGSystem {
    public class InventoryUI : MonoBehaviour {

        public int Width = 1;
        public int Height = 1;
        public int CellPadding = 10;

        public ItemTypes CanHold = (ItemTypes)~0;
        public Item[,] Items;
        public Image SampleCell;

        public bool Visible
        {
            set {
                if (inventory != null)
                    inventory.gameObject.SetActive(value);
            }
            get {
                if (inventory != null)
                    return inventory.gameObject.activeSelf;
                return false;
            }
        }

        public int Size
        {
            private set { }
            get {
                return Width * Height;
            }
        }

        private Image inventory;

        // Use this for initialization
        void Start() {
            SampleCell.gameObject.SetActive(false);
            Items = new Item[Width, Height];
            inventory = SampleCell.transform.parent.GetComponent<Image>();

            if (inventory != null)
            {
                //Resize inventory to fit cells
                inventory.rectTransform.sizeDelta = new Vector2(
                    (CellPadding * (Width + 1)) + (SampleCell.rectTransform.rect.width * Width),
                    (CellPadding * (Height + 1)) + (SampleCell.rectTransform.rect.height * Height)
                );

                // Add item cells to the inventory
                for (int x = 0; x < Width; x++)
                {
                    for (int y = 0; y < Height; y++)
                    {
                        Image nextCell = Instantiate(SampleCell);
                        nextCell.gameObject.SetActive(true);
                        nextCell.gameObject.name = name + "_cell_" + x + "_" + y;
                        nextCell.transform.SetParent(inventory.transform);
                        Vector3 oldPos = SampleCell.transform.position;
                        nextCell.transform.position= new Vector3(
                            oldPos.x + SampleCell.rectTransform.rect.width * x + CellPadding * x,
                            oldPos.y - SampleCell.rectTransform.rect.height * y - CellPadding * y
                        );
                    }
                }
            }
        }

        // Update is called once per frame
        void Update() {
            
        }

        public bool AddItem(Item item)
        {
            // Check if the inventory can hold this item
            // Exit if it cannot
            if ((item.type & CanHold) == 0) return false;

            // Check if the inventory already contains this item
            // if it does and it can stack, add it to the stack
            //foreach (Item i in Items)
            //{
            //    if (i == item &&
            //        i.stackLimit > 1 &&
            //        i.currentStacks < i.stackLimit)
            //    {
            //        i.currentStacks++;
            //        return true;
            //    }
            //}

            // If the item can't be added to an already existing stack
            // find an empty slot in the inventory and add it there
            foreach (Item i in Items)
            {
                if (i == null)
                {
                    // TODO 
                    // change this loop so i can be assigned
                    //i = item;
                    return true;
                }
            }

            // If there are no empty slots in the inventory
            // the item can't be added to it.
            return false;
        }
    }
}
