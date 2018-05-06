using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace RPGSystem
{
    public class Inventory : MonoBehaviour
    {
        public int Width = 1;
        public int Height = 1;
        public int Size
        {
            private set { }
            get
            {
                return Width * Height;
            }
        }

        public Item ItemToAdd;

        public ItemTypes CanHold = ItemTypes.All;
        public Item[][] Items;

        [System.Serializable]
        public class InventoryEvent : UnityEvent<Item, int, int>
        {
        }

        public InventoryEvent OnUpdate;

        public Item GetItem(int x, int y)
        {
            // lazy initialisation of our array
            if (Items == null)
            {
                Items = new Item[Width][];
                for (int i = 0; i < Width; i++)
                    Items[i] = new Item[Height];
            }

            return Items[x][y];
        }

        public int GetItemCount(Item item)
        {
            int count = 0;
            for (int x = 0; x < Width; x++)
                for (int y = 0; y < Height; y++)
                    count += GetItem(x, y).name == item.name ? GetItem(x, y).currentStacks : 0;
            return count;
        }

        public void SetItem(int x, int y, Item item)
        {
            // lazy initialisation
            if (Items == null)
            {
                Items = new Item[Width][];
                for (int i = 0; i < Width; i++)
                    Items[i] = new Item[Height];
            }

            Items[x][y] = item;
            OnUpdate.Invoke(item, x, y);
        }

        public bool RemoveItems(Item item, int amount)
        {
            for (int x = 0; x < Width; x++)
                for (int y = 0; y < Height; y++)
                {
                    Item temp = GetItem(x, y);
                    if (temp.name == item.name)
                    {
                        if (temp.currentStacks <= amount)
                        {
                            amount -= temp.currentStacks;
                            SetItem(x, y, null);
                        }
                        else if (temp.currentStacks > amount)
                            temp.currentStacks -= amount;
                    }

                    if (amount == 0)
                        return true;
                }

            return false;
        }

        public int GetEmptySlotCount()
        {
            int count = 0;
            for (int x = 0; x < Width; x++)
                for (int y = 0; y < Height; y++)
                    if (GetItem(x, y) == null)
                        count++;
            return count;
        }

        // Use this for initialization
        void Start()
        {
            // force the array of items to initialise if it hasnt already done so
            GetItem(0, 0);
        }

        // Update is called once per frame
        void Update()
        {
            if (ItemToAdd != null)
            {
                AddPrefabItem(ItemToAdd);
                ItemToAdd = null;
            }
        }

        private void AddPrefabItem(Item item)
        {
            string name = item.name;
            item = Instantiate(item);
            item.name = name;

            AddItem(item);
        }

        public bool AddItem(Item item)
        {
            // Check if the inventory can hold this item
            // Exit if it cannot
            if ((item.type & CanHold) == 0) return false;

            // Check if the inventory already contains this item
            // if it does and it can stack, add it to the stack
            for (int x = 0; x < Items.Length; x++)
            {
                for (int y = 0; y < Items[x].Length; y++)
                {
                    Item i = Items[x][y];
                    if (i != null &&
                        i.name == item.name &&
                        i.stackLimit > 1 &&
                        i.currentStacks < i.stackLimit)
                    {
                        if (i.currentStacks + item.currentStacks > i.stackLimit)
                        {
                            int fillAmount = i.stackLimit - i.currentStacks;
                            i.currentStacks = i.stackLimit;
                            item.currentStacks -= fillAmount;
                            OnUpdate.Invoke(i, x, y);
                            return AddItem(item);
                        }
                        else
                        {
                            i.currentStacks += item.currentStacks;
                            OnUpdate.Invoke(i, x, y);
                            return true;
                        }
                    }
                }
            }

            // If the item can't be added to an already existing stack
            // find an empty slot in the inventory and add it there
            for (int x = 0; x < Items.Length; x++)
            {
                for (int y = 0; y < Items[x].Length; y++)
                {
                    if (GetItem(x,y) == null)
                    {
                        SetItem(x, y, item);
                        OnUpdate.Invoke(item, x, y);
                        return true;
                    }
                }
            }

            // If there are no empty slots in the inventory
            // the item can't be added to it.
            return false;
        }
    }
}
