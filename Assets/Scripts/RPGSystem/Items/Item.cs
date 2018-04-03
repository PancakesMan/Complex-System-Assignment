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
        public MeshRenderer    model;

        public int stackLimit, currentStacks;
    }
}
