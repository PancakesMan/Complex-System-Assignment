using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPGSystem {
    public class Inventory : MonoBehaviour {

        public int Width = 1;
        public int Height = 1;

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
    }
}
