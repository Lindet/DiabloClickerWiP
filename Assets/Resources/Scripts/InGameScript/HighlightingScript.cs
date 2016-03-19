using UnityEngine;
using System.Collections;

public class HighlightingScript : MonoBehaviour {
    
	// Use this for initialization
    private Material outlineMaterial;
    private Material transparentMaterial;

	void Start ()
    {
        var shader = Shader.Find("Outlined/Silhouette Only");
        outlineMaterial = new Material(shader);
        outlineMaterial.SetColor("_OutlineColor", new Color32(22, 112, 135, 255));
        outlineMaterial.SetFloat("_Outline", 0.0025f);

	    transparentMaterial = Resources.Load<Material>(@"Models/Materials/Transparent");

	    SetMaterial(true);
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnMouseEnter()
    {
        SetMaterial(false);
    }

    void OnMouseExit()
    {
        SetMaterial(true);
    }

    void SetMaterial(bool transparent)
    {
        if (!gameObject.GetComponent<MeshRenderer>())
        {
            foreach (Transform child in transform)
            {
                if (child.GetComponent<MeshRenderer>() && child.GetComponent<MeshRenderer>().sharedMaterials.Length > 1)
                {
                    var materials = child.GetComponent<MeshRenderer>().sharedMaterials;
                    materials[1] = transparent? transparentMaterial : outlineMaterial;
                    child.GetComponent<MeshRenderer>().sharedMaterials = materials;
                }
            }
        }
        else
        {
            var materials = gameObject.GetComponent<MeshRenderer>().sharedMaterials;
            materials[1] = transparent ? transparentMaterial : outlineMaterial;
            gameObject.GetComponent<MeshRenderer>().sharedMaterials = materials;
        } 
    }

    void OnDisable()
    {
        SetMaterial(true);
    }
}
