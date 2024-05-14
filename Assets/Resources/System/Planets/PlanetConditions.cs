using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public static class PlanetConditions
{
    static readonly float barMaxFillValue = 0.5f;


    public static float GetRadIndicatorLevel(float radiation)
    {

        float interpolatedRad = (Mathf.Log(radiation, 10) + 5) / 10;
        float radLevel = (Mathf.Round(interpolatedRad * 10)) / 10;
        return radLevel;
    }
    public static float GetPresIndicatorLevel(float pressure)
    {
        float pressureLevel = 0;
        float interpolatedPressureLevel = 0;


        if (pressure >= 0.001f && pressure < 0.01f) { pressureLevel = 0.2f; }
        else if (pressure >= 0.01f && pressure < 0.25f) { pressureLevel = 0.4f; }
        else if (pressure >= 0.25f && pressure < 4f) { pressureLevel = 0.6f; }
        else if (pressure >= 4f && pressure < 50f) { pressureLevel = 0.8f; }
        else if (pressure >= 50f) { pressureLevel = 1f; }

        interpolatedPressureLevel = Mathf.Lerp(0, barMaxFillValue, pressureLevel);

        return interpolatedPressureLevel;
    }


    public static float GetTempIndicatorLevel(float temperature)
    {

        float temperatureLevel = 0;
        float interpolatedTemperatureLevel = 0;


        if (temperature < -100) { temperatureLevel = 0.2f; }
        else if (temperature >= -100 && temperature < 0f) { temperatureLevel = 0.4f; }
        else if (temperature >= 0 && temperature < 50f) { temperatureLevel = 0.6f; }
        else if (temperature >= 50f && temperature < 100f) { temperatureLevel = 0.8f; }
        else if (temperature >= 100f) { temperatureLevel = 1f; }

        interpolatedTemperatureLevel = Mathf.Lerp(0, barMaxFillValue, temperatureLevel);

        return interpolatedTemperatureLevel;
    }

    public static string GetTempLevelText(float temperature)
    {

        string tempLevelText = "";

        switch (temperature)
        {
            case float t when t <= -200.0f:
                tempLevelText = "Cryogenic environment";
                break;
            case float t when t >= -200 && t < -182f:
                tempLevelText = "Free oxygen liquifies";
                break;
            case float t when t >= -182f && t < -100f:
                tempLevelText = "Deep freezing";
                break;
            case float t when t >= -100 && t < -20f:
                tempLevelText = "Unplesantly cold";
                break;
            case float t when t >= -20 && t < 5f:
                tempLevelText = "Mildly chilly";
                break;
            case float t when t >= 5 && t < 50f:
                tempLevelText = "Nice and warm";
                break;
            case float t when t >= 50 && t < 100f:
                tempLevelText = "Hot and toasty";
                break;
            case float t when t >= 100 && t < 200f:
                tempLevelText = "Extra crispy";
                break;
            case float t when t >= 200 && t < 300f:
                tempLevelText = "Pizza oven-like";
                break;
            case float t when t >= 300 && t < 400:
                tempLevelText = "Lead melting";
                break;
            case float t when t >= 300 && t < 800:
                tempLevelText = "Everything burns";
                break;
            case float t when t >= 800:
                tempLevelText = "Rocks melt";
                break;
            default:
                break;
        }


        return tempLevelText;

    }

    public static string GetPressLevelText(float pressure)
    {


        string pressLevelText = "";

        switch (pressure)
        {
            case float p when p <= 0.001f:
                pressLevelText = "No atmosphere";
                break;
            case float p when p >= 0.001 && p <0.01f:
                pressLevelText = "Trace atmosphere";
                break;
            case float p when p >= 0.01 && p < 0.25:
                pressLevelText = "Low pressure";
                break;
            case float p when p >= 0.25 && p < 4f:
                pressLevelText = "Standard pressure";
                break;
            case float p when p >= 4 && p < 50f:
                pressLevelText = "Squeezing pressure";
                break;
            case float p when p >= 50:
                pressLevelText = "Crushing pressure";
                break;
            default:
                break;
        }

        return pressLevelText;

    }
}
