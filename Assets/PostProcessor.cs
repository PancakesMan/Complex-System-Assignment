using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PostProcessor : MonoBehaviour {

    public List<Material> materials;
    public int materialIndex = 0;

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if (materials[materialIndex].HasProperty("_DeltaX"))
        {
            materials[materialIndex].SetFloat("_DeltaX", 1.0f / source.width);
            materials[materialIndex].SetFloat("_DeltaY", 1.0f / source.height);
        }

        Graphics.Blit(source, destination, materials[materialIndex]);
    }

    public void SetMaterialIndex(int index)
    {
        materialIndex = index;
    }
}
