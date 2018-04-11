using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPGSystem;

public class UserKeyBindings : MonoBehaviour {

    public InventoryUI inventoryUI;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyUp(KeyCode.I))
            inventoryUI.Visible = !inventoryUI.Visible;
	}
}
