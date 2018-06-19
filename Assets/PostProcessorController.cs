using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PostProcessorController : MonoBehaviour {

    public Button button;

    bool active = true;

    public void TogglePostProcessor()
    {
        active = !active;
        button.GetComponentInChildren<Text>().text = active ? "Disable" : "Enable";
        GetComponent<PostProcessor>().enabled = active;
    }
}
