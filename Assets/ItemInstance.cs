using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPGSystem
{
    public class ItemInstance : MonoBehaviour
    {
        private Item _item;

        // Use this for initialization
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {

        }

        void SetItem(Item item, Transform spawnPosition)
        {
            _item = item;
            GetComponent<MeshFilter>().mesh = _item.model;

            GetComponent<Rigidbody>().position = spawnPosition.position;

            GetComponent<Rigidbody>().velocity = (spawnPosition.forward + Vector3.up) * 10.0f;
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                if (collision.gameObject.GetComponent<Inventory>().AddItem(_item))
                    Destroy(this);
            }
        }
    }
}
