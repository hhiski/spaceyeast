using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlanetAtmosphere : MonoBehaviour
{

    Color atmosphereColor = new Color(1f, 0.0f, 1, 0);

    public void SetAtmosphereColor(Color atmosphereColor)
    {
        Color color = atmosphereColor;

        if (this.transform.gameObject.TryGetComponent<Renderer>(out Renderer renderer))
        {
            Material atmosphereMaterial = new Material(renderer.sharedMaterial);
            renderer.material = atmosphereMaterial;
            atmosphereMaterial.SetColor("_Emission", atmosphereColor);
        }
        else
        {
            Debug.LogWarning("PlanetAtmosphere renderer not found");
        }

    }


}
