using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPGSystem
{
    [CreateAssetMenu(fileName = "New Item", menuName = "RPG System/Item")]
    public class Item : ScriptableObject
    {
        public ItemTypes       type;
        public Sprite          sprite;
        public GameObject      modelPrefab;

        public int stackLimit, currentStacks;
        public string description;

        public static Item Copy(Item item)
        {
            string name = item.name;
            Item _item = Instantiate(item);
            _item.name = name;

            return _item;
        }
    }
}
