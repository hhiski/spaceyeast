using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ColorSpace
{
    class ColorFunctions
    {

        public Color ThreeColorLerp(Color colorA, Color colorB, Color colorC, float lerpT)
        {
            Color colorMix;
            float t = lerpT;

            if (lerpT <= 0.5f)
            {
                colorMix = Color.Lerp(colorA,colorB, t * 2);
            }
            else
            {
                t = (lerpT - 0.5f) * 2f;
                colorMix = Color.Lerp(colorB, colorC, t);
            }

            return colorMix;
        }

        public Color HueShiftColor(Color color, float colorHueShift, float colorHueMultiply)
        {
            float hue;
            float saturation;
            float colorValue;

            Color.RGBToHSV(color, out hue, out saturation, out colorValue);
            colorHueShift %= 1;

            hue = hue += colorHueShift;
            hue = hue *= colorHueMultiply;

            if (hue > 1) { hue -= 1; };
            if (hue < 0) { hue += 1; };

            color = Color.HSVToRGB(hue, saturation, colorValue);

            return color;
        }

        public Color SaturationShiftColor(Color color, float colorSaturationShift, float colorSaturationMultiply)
        {
            float hue;
            float saturation;
            float colorValue;

            Color.RGBToHSV(color, out hue, out saturation, out colorValue);

            saturation += colorSaturationShift;
            saturation *= colorSaturationMultiply;

            if (saturation > 1) { saturation = 1; };
            if (saturation < 0) { saturation = 0; };

            color = Color.HSVToRGB(hue, saturation, colorValue);

            return color;
        }

        public Color ValueShiftColor(Color color, float colorValueShift, float colorValueMultiply)
        {
            float hue;
            float saturation;
            float colorValue;

            Color.RGBToHSV(color, out hue, out saturation, out colorValue);

            colorValue += colorValueShift;
            colorValue *= colorValueMultiply;

            if (colorValue > 1) { colorValue = 1; };
            if (colorValue < 0) { colorValue = 0; };

            color = Color.HSVToRGB(hue, saturation, colorValue);

            return color;
        }

        /* public Color LatitudeBasedColoring(Color inputColor, float noise, float y, float polarCoverage)
         {

             Color polarZone = SaturationShiftColor(inputColor, 0, 0.8f);
             polarZone = ValueShiftColor(polarZone, 0.04f, 1);
             polarZone = HueShiftColor(polarZone, 0.46f, 1);
             Color taigaZone = SaturationShiftColor(inputColor, 0, 0.6f);
             taigaZone = HueShiftColor(taigaZone, 0.06f, 1);
             Color tundraZone = SaturationShiftColor(inputColor, 0, 0.3f);
             tundraZone = ValueShiftColor(tundraZone, 0, 1.3f);


             //  float noise = (PerlinFilter(point, NoiseLayer, 3.0f, 1, 0.15f)); ;
             //float snowNoise = noise * point.y;

             Color rightColor = inputColor;
             float latitude = (Mathf.Rad2Deg * (Mathf.Acos(y))) / 360f; ;


             //  float noise = (PerlinFilter(point, NoiseLayer, 3.0f, 1, 0.15f)); ;
             //float snowNoise = noise * point.y;
             // point = vertices[vertexIndex];
             //   noise = NoiseFunctions.PerlinFilter(point, NoiseLayer, 1.4f, 0, 1, 45);
             //  noise = noise + NoiseFunctions.PerlinFilter(point, NoiseLayer, 2f, 0, 0.5f, 899);


             float polarLevel = 1 - (polarCoverage);


             float tundraLevel = polarLevel + 0.1f;
             float taigaLevel = tundraLevel + 0.2f;

             if (latitude >= polarLevel || latitude <= -polarLevel)
             {
                 rightColor = polarZone;
             }
             else if (latitude >= tundraLevel || latitude <= -tundraLevel)
             {
                 rightColor = tundraZone;
             }
             else if (latitude >= taigaLevel || latitude <= -taigaLevel)
             {
                 rightColor = taigaZone;
             }
             else
             {
                 rightColor = inputColor;
             };


             return rightColor;
         }*/

        public Color LatitudeBasedColoring(Color inputColor, Vector3 vertex, float colorNoise, float iceCapsSize)
        {

            Vector3[] poles = new[] { new Vector3(0f, 1f, 0f), new Vector3(0f, -1f, 0f) }; //north pole and south pole;
            Vector3 point = vertex;

            float taigaRadius = iceCapsSize * 3.9f;
            float tundraRadius = iceCapsSize * 2.6f;

            float noise;

            Color taigaColor = SaturationShiftColor(inputColor, 0, 0.6f);
            taigaColor = HueShiftColor(taigaColor, 0.06f, 1);
            Color tundraColor = SaturationShiftColor(inputColor, 0, 0.3f);
            tundraColor = ValueShiftColor(tundraColor, 0, 1.3f);
            Color latitudeBasedColor = inputColor;

            foreach (Vector3 pole in poles)
            {
                float distance = Vector3.Distance(point, pole);

                if (distance <= taigaRadius)
                    {

                        float rDistance = distance / taigaRadius;
                        noise = colorNoise;
                        noise += 0.5f;
                        noise -= 0.6f * (rDistance);

                        if (noise > 0)
                        {
                        latitudeBasedColor = taigaColor;
                        }
                        else if (noise > 0.5f)
                        {
                        latitudeBasedColor = tundraColor;
                        }
                    }
                }

                 return latitudeBasedColor;
            }











        

    }
}
