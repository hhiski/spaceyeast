using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlanetRing : MonoBehaviour
{

    public GameObject SimpleRing;
    public GameObject SimpleRingOuter;
    public GameObject WideRing;
    public GameObject DoubleRingInner;
    public GameObject DoubleRingOuter;
    public GameObject MicroMoonRing;

    GameObject PlanetRingA;
    GameObject PlanetRingB;

    Color ColorPrimary = new Color(1f, 0.0f, 1, 1);
    Color ColorSecondary = new Color(1f, 1.0f, 1, 1);

    Color EvaluatePrimaryColor(Gradient colorGradient)
    {
        return colorGradient.Evaluate(0.33f);
    }
    Color EvaluateSecondaryColor(Gradient colorGradient)
    {
        return colorGradient.Evaluate(0.66f);
    }

    public void ColorRings(Gradient gradient)
    {
      
        if (gradient != null) { 

            ColorPrimary = EvaluatePrimaryColor(gradient); ;
            ColorSecondary = EvaluateSecondaryColor(gradient); ;

            ColorPrimary = LightenDarkColors(ColorPrimary);
            ColorSecondary = LightenDarkColors(ColorSecondary);

            if (PlanetRingA != null && PlanetRingA.TryGetComponent<Renderer>(out Renderer rendererA))
            {
                rendererA.material.SetColor("_Color", ColorPrimary);
            }


            if (PlanetRingB != null &&  PlanetRingB.TryGetComponent<Renderer>(out Renderer rendererB))
            {
                rendererB.material.SetColor("_Color", ColorSecondary);
            }
        }
        else
        {
            Debug.Log("Planet colorGradient null on PlanetRing");
        }

    }

    public void CreateRingWithColor(int ringType, Gradient colorGradient)
    {


        CreateRing(ringType);
        ColorRings(colorGradient);

    }

    public void CreateRing(int ringType)
    {

        if (ringType == 0)
        {
           //No rings
        }

        else if (ringType == 1) {
            PlanetRingA = Instantiate(SimpleRing, transform, false) as GameObject;
            PlanetRingA.transform.Rotate(92.0f, 0.0f, 0.0f, Space.Self);
        }
        else if (ringType == 2) {
            PlanetRingA = Instantiate(WideRing, transform, false) as GameObject;
            PlanetRingA.transform.Rotate(92.0f, 0.0f, 0.0f, Space.Self);
        }
        else if (ringType == 3) {
            PlanetRingA = Instantiate(DoubleRingInner, transform, false) as GameObject;
            PlanetRingA.transform.Rotate(92.0f, 0.0f, 0.0f, Space.Self);

            PlanetRingB = Instantiate(DoubleRingOuter, transform, false) as GameObject;
            PlanetRingB.transform.Rotate(92.0f, 0.0f, 0.0f, Space.Self);
        }

        else if (ringType == 4)
        {
            PlanetRingA = Instantiate(SimpleRing, transform, false) as GameObject;
            PlanetRingA.transform.Rotate(98.0f, 0.0f, 0.0f, Space.Self);

            PlanetRingB = Instantiate(SimpleRingOuter, transform, false) as GameObject;
            PlanetRingB.transform.Rotate(115.0f, 0.0f, 0.0f, Space.Self);

        }

        else if (ringType == 5)
        {
            PlanetRingA = Instantiate(MicroMoonRing, transform, false) as GameObject;
        }

    }

    Color LightenDarkColors(Color color)
    {

        float hue;
        float saturation;
        float colorValue;

        Color.RGBToHSV(color, out hue, out saturation, out colorValue);

        if (colorValue < 0.5f)
        {
            colorValue = colorValue + 0.5f;
        }

        color = Color.HSVToRGB(hue, saturation, colorValue);

        return color;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
