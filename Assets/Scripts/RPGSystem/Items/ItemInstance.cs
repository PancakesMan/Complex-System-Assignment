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
        private float _timer = 0.0f;

        // Use this for initialization
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {
            _timer += Time.deltaTime;
            if (_timer > pickupTimer)
                _pickable = true;
        }

        public void SetItem(Item item, Transform spawnPosition)
        {
            _item = item;
            GetComponent<Rigidbody>().position = spawnPosition.position + Vector3.up;
            GetComponent<Rigidbody>().velocity = (spawnPosition.forward + Vector3.up) * 3.0f;
        }

        private void OnTriggerEnter(Collider collider)
        {
            if (collider.gameObject.CompareTag("Player") && _pickable)
                // Instatiate the item when added so it doesn't change the prefab
                if (collider.gameObject.GetComponent<Inventory>().AddItem(MakeCopyOf(_item)))
                    Destroy(gameObject);

            else if (collider.gameObject.CompareTag("Ground"))
                GetComponent<Rigidbody>().velocity = Vector3.zero;
        }

        private Item MakeCopyOf(Item item)
        {
            string name = item.name;
            Item _item = Instantiate(item);
            _item.name = name;

            return _item;
        }
    }
}
