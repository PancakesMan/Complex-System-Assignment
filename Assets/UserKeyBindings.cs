using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPGSystem;

public class UserKeyBindings : MonoBehaviour {

    public InventoryUI inventoryUI;
    public InventoryUI ExternalInventoryUI;

    public float inventoryOpenDistance = 5.0f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyUp(KeyCode.I))
            inventoryUI.Visible = !inventoryUI.Visible;

        if (Input.GetKeyUp(KeyCode.Escape))
            ExternalInventoryUI.Visible = false;

        if (ExternalInventoryUI.Visible && Vector3.Distance(ExternalInventoryUI.inventory.transform.position, transform.position) > inventoryOpenDistance)
            ExternalInventoryUI.Visible = false;

        if (Input.GetMouseButtonDown(1))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, 30.0f))
                if (hit.transform != null && hit.transform != transform)
                {
                    Inventory inv = hit.transform.GetComponent<Inventory>();
                    if (inv && Vector3.Distance(hit.transform.position, transform.position) < inventoryOpenDistance)
                    {
                        if (ExternalInventoryUI.inventory != inv)
                            ExternalInventoryUI.SetInventory(inv);
                        ExternalInventoryUI.Visible = true;
                    }
                }
        }
	}
}
