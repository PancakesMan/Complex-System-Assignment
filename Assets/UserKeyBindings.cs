using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPGSystem;

public class UserKeyBindings : MonoBehaviour {

    public InventoryUI inventoryUI;
    public InventoryUI ExternalInventoryUI;

    public float interactDistance = 5.0f;

    private float oldTimeScale;
    public bool paused = false;
    public Canvas pauseMenu;

	// Use this for initialization
	void Start () {
        GetComponent<DialogueTrigger>().TriggerDialogue();
	}
	
	// Update is called once per frame
	void Update () {
        // Show players inventory
        if (Input.GetKeyUp(KeyCode.I))
            inventoryUI.Visible = !inventoryUI.Visible;

        // Pause game
        if (Input.GetKeyUp(KeyCode.P))
        {
            if (!paused) oldTimeScale = Time.timeScale;
            paused = !paused;
            pauseMenu.gameObject.SetActive(paused);
            Time.timeScale = paused ? 0 : oldTimeScale;
        }

        // Hide opened external inventory
        if (Input.GetKeyUp(KeyCode.Escape))
            ExternalInventoryUI.Visible = false;

        // Hide opened external inventory if we're too far away
        if (ExternalInventoryUI.Visible && Vector3.Distance(ExternalInventoryUI.inventory.transform.position, transform.position) > interactDistance)
            ExternalInventoryUI.Visible = false;

        // If we right click
        if (Input.GetMouseButtonDown(1))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            // Raycast to get an object
            if (Physics.Raycast(ray, out hit, 30.0f))
            {
                if (hit.transform != null && hit.transform != transform)
                {
                    // If we did, check if it is interactable
                    Interactable interactable = hit.transform.GetComponent<Interactable>();
                    if (interactable)
                    {
                        // If object is interactable
                        // Interact based on it's interaction type
                        switch (interactable.type)
                        {
                            // If it's an inventory
                            case InteractionType.Inventory:
                                // Get the inventory component
                                Inventory inv = hit.transform.GetComponent<Inventory>();
                                if (inv && Vector3.Distance(hit.transform.position, transform.position) < interactDistance)
                                {
                                    // If it exists and we're in range
                                    // Display the inventory
                                    if (ExternalInventoryUI.inventory != inv)
                                        ExternalInventoryUI.SetInventory(inv);
                                    ExternalInventoryUI.Visible = true;
                                }
                                break;
                            // If it's a Dialogue
                            case InteractionType.Dialogue:
                                // Get the DialogueTrigger component
                                DialogueTrigger trigger = hit.transform.GetComponent<DialogueTrigger>();
                                if (trigger && Vector3.Distance(hit.transform.position, transform.position) < interactDistance)
                                {
                                    // If the component exists trigger the dialogue
                                    trigger.TriggerDialogue();
                                }
                                break;
                            default:
                                break;
                        }
                    }
                }
            }
        }
	}

    public void StartDropItemCoroutine(Item item, Transform transform, float seconds)
    {
        StartCoroutine(DropItem(item, transform, seconds));
    }

    public IEnumerator DropItem(Item item, Transform transform, float seconds)
    {
        yield return new WaitForSecondsRealtime(seconds);

        GameObject droppedItem = Instantiate(item.modelPrefab);
        droppedItem.GetComponent<ItemInstance>().SetItem(item, transform);
    }
}
