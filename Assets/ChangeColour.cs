using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeColour : MonoBehaviour {

    new Light light;

	// Use this for initialization
	void Start () {
        light = GetComponent<Light>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Night()
    {
        light.color = new Color(0, 0, 0, 1);
    }

    public void Day()
    {
        light.color = new Color(1, 1, 1, 1);
    }
}
