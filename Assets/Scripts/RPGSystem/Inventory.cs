using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPGSystem {
    public class Inventory : MonoBehaviour {

        public int Width = 1;
        public int Height = 1;

        public ItemTypes CanHold = (ItemTypes)2147483647;

        public int Size
        {
            private set { }
            get {
                return Width * Height;
            }
        }

        Item[,] items;

        // Use this for initialization
        void Start() {
            items = new Item[Width, Height];
        }

        // Update is called once per frame
        void Update() {

        }

        public bool AddItem(Item item)
        {
            // Check if the inventory can hold this item
            // Exit if it cannot
            //if (!(int)(item.itemType & CanHold)) return false;

            // Check if the inventory already contains this item
            // if it does and it can stack, add it to the stack
            foreach (Item i in items)
            {
                if (i == item &&
                    i.stackLimit > 1 &&
                    i.currentStacks < i.stackLimit)
                {
                    i.currentStacks++;
                    return true;
                }
            }

            // If the item can't be added to an already existing stack
            // find an empty slot in the inventory and add it there
            foreach (Item i in items)
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
