using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPGSystem
{
    [CreateAssetMenu(fileName = "New Item", menuName = "RPG System/Item")]
    public class Item : ScriptableObject
    {
        public ItemTypes    itemType;
        public Sprite       itemSprite;
    }
}
