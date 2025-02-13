using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using ColorSpace;

public class StarSurface : MonoBehaviour
{
    public CelestialBody.Star Star;

    ColorFunctions ColorFunctions = new ColorFunctions();

    Mesh starMesh;

    Material surfaceMaterial;




    internal void CopyVertices()
    {
        Mesh starSharedMesh = GetComponent<MeshFilter>().sharedMesh;
        starMesh = Instantiate(starSharedMesh);
        GetComponent<MeshFilter>().sharedMesh = starMesh;

        Renderer surfaceRenderer = this.transform.gameObject.GetComponent<Renderer>();
        if (surfaceRenderer != null)
        {
            surfaceMaterial = new Material(surfaceRenderer.sharedMaterial);
            surfaceRenderer.material = surfaceMaterial;
        }
    }
}
