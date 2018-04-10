using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPGSystem
{
    public class ItemInstance : MonoBehaviour
    {
        private Item _item;
        private bool _landed = false;

        // Use this for initialization
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void SetItem(Item item, Transform spawnPosition)
        {
            _item = item;
            GetComponent<Rigidbody>().position = spawnPosition.position;
            GetComponent<Rigidbody>().velocity = (spawnPosition.forward + Vector3.up) * 3.0f;
        }

        private void OnTriggerEnter(Collider collider)
        {
            if (collider.gameObject.CompareTag("Player") && _landed)
            {
                if (collider.gameObject.GetComponent<Inventory>().AddItem(_item))
                    Destroy(gameObject);
            }
            else if (collider.gameObject.CompareTag("Ground"))
            {
                GetComponent<Rigidbody>().velocity = Vector3.zero;
                _landed = true;
            }
        }
    }
}
