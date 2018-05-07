using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPGSystem
{
    public class ItemInstance : MonoBehaviour
    {
        public Item _item;
        public float pickupTimer = 2.0f;

        private bool _pickable = false;

        // Use this for initialization
        void Start()
        {
            // Set pikcup bool to true in pickupTimer seconds
            Invoke("AllowPickup", pickupTimer);
        }

        public void SetItem(Item item, Transform spawnPosition)
        {
            // Set the item of the ItemInstance
            _item = item;

            // Give the ItemInstance a position and veocity
            GetComponent<Rigidbody>().position = spawnPosition.position + Vector3.up;
            GetComponent<Rigidbody>().velocity = (spawnPosition.forward + Vector3.up) * 3.0f;
        }

        private void OnTriggerEnter(Collider collider)
        {
            // If the ItemInstance collides with the player and the item can be picked up
            if (collider.gameObject.CompareTag("Player") && _pickable)
                // Instatiate the item so it doesn't change the prefab
                // and add it to the player's inventory
                if (collider.gameObject.GetComponent<Inventory>().AddItem(Item.Copy(_item)))
                    // If the item was completely added to the inventory
                    // Destroy the ItemInstance
                    Destroy(gameObject);

            // Else if we collided with the ground
            else if (collider.gameObject.CompareTag("Ground"))
                // Set the ItemInstances velocity to 0 so it doesn't roll around
                GetComponent<Rigidbody>().velocity = Vector3.zero;
        }

        private void AllowPickup()
        {
            _pickable = true;
        }
    }
}
