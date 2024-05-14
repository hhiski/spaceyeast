using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetRing : MonoBehaviour
{

    public GameObject SimpleRing;
    public GameObject SimpleRingOuter;
    public GameObject WideRing;
    public GameObject DoubleRingInner;
    public GameObject DoubleRingOuter;
    public GameObject MicroMoonRing;


    public Color colorPrimary = new Color(1f, 0.0f, 1, 1);
    public Color colorSecondary = new Color(1f, 1.0f, 1, 1);

    public void CreateRing(int ringType)
    {

        colorPrimary = this.gameObject.transform.parent.GetComponent<PlanetSurface>().colorMidShifted;
        colorSecondary = this.gameObject.transform.parent.GetComponent<PlanetSurface>().colorLowShifted;

        colorPrimary = LightenDarkColors(colorPrimary);
        colorSecondary = LightenDarkColors(colorSecondary);

        if (ringType == 0)
        {
           //No rings
        }

        else if (ringType == 1) {
            GameObject PlanetRingA = Instantiate(SimpleRing, transform, false) as GameObject;
            PlanetRingA.GetComponent<Renderer>().material.SetColor("_Color", colorPrimary);
            PlanetRingA.transform.Rotate(92.0f, 0.0f, 0.0f, Space.Self);
        }
        else if (ringType == 2) {
            GameObject PlanetRingA = Instantiate(WideRing, transform, false) as GameObject;
            PlanetRingA.GetComponent<Renderer>().material.SetColor("_Color", colorPrimary);
            PlanetRingA.transform.Rotate(92.0f, 0.0f, 0.0f, Space.Self);
        }
        else if (ringType == 3) {
            GameObject PlanetRingA = Instantiate(DoubleRingInner, transform, false) as GameObject;
            PlanetRingA.GetComponent<Renderer>().material.SetColor("_Color", colorPrimary);
            PlanetRingA.transform.Rotate(92.0f, 0.0f, 0.0f, Space.Self);

            GameObject PlanetRingB = Instantiate(DoubleRingOuter, transform, false) as GameObject;
            PlanetRingB.GetComponent<Renderer>().material.SetColor("_Color", colorSecondary);
            PlanetRingB.transform.Rotate(92.0f, 0.0f, 0.0f, Space.Self);
        }

        else if (ringType == 4)
        {
            GameObject PlanetRingA = Instantiate(SimpleRing, transform, false) as GameObject;
            PlanetRingA.GetComponent<Renderer>().material.SetColor("_Color", colorPrimary);
            PlanetRingA.transform.Rotate(98.0f, 0.0f, 0.0f, Space.Self);

            GameObject PlanetRingB = Instantiate(SimpleRingOuter, transform, false) as GameObject;
            PlanetRingB.GetComponent<Renderer>().material.SetColor("_Color", colorSecondary);
            PlanetRingB.transform.Rotate(115.0f, 0.0f, 0.0f, Space.Self);

        }

        else if (ringType == 5)
        {
            GameObject PlanetRingA = Instantiate(MicroMoonRing, transform, false) as GameObject;
        }

    }

    Color LightenDarkColors(Color color)
    {

        float hue;
        float saturation;
        float colorValue;

        Color.RGBToHSV(color, out hue, out saturation, out colorValue);

        if (colorValue < 0.3f)
        {
            colorValue = colorValue + 0.3f;
        }

        color = Color.HSVToRGB(hue, saturation, colorValue);

        return color;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
