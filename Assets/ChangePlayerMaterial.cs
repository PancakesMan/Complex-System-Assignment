using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangePlayerMaterial : MonoBehaviour {

    public List<Material> materials;

    private Material original;

	// Use this for initialization
	void Start () {
        original = GetComponent<Renderer>().material;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void ResetShader()
    {
        GetComponent<Renderer>().material = original;
    }

    public void ApplyShader(int index)
    {
        GetComponent<Renderer>().material = materials[index];
    }
}
