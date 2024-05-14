using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MathSpace;

#region hydrosphere classes
public class HydrosphereSource
{
    string Name;
    int Priority; //higher number -> easier accessability and a primary water source
    float TempMax = 9999f; // temperature range where the water source exists
    float TempMin = -9999;
    float PressMax = 99999; // pressure range where the water source exists
    float PressMin = -9999;
    int WaterMin = -99;  // wetness range where the water source exists
    int WaterMax = 99999;  // wetness range where the water source exists
                           //Water = 0 -> No water
                           //Water = 1 -> Dry
                           //Water = 2 -> Wet

    public string GetWaterSourceName()
    {
        return Name;
    }
    public int GetPriority()
    {
        return Priority;
    }
    public bool CheckValidity(float planetTemperature, float planetPressure, float planetWaterLevel) //checks if water source can exist on given conditions.
    {
        bool valid = true;

        if (planetTemperature > TempMax || planetTemperature < TempMin) valid = false;
        if (planetPressure > PressMax || planetPressure < PressMin) valid = false;
        if (planetWaterLevel > WaterMax || planetWaterLevel < WaterMin) valid = false;

        return valid;
    }


    public HydrosphereSource(string name = "Undefined", float tempMax = 9999, float tempMin = -9999, float pressMax = 9999, float pressMin = -9999, int waterMin = -1, int waterMax = 4, int priority = 0)
    {
        Name = name;
        TempMax = tempMax;
        TempMin = tempMin;
        PressMax = pressMax;
        PressMin = pressMin;
        WaterMin = waterMin;
        WaterMax = waterMax;
        Priority = priority;
    }

    public HydrosphereSource()
    {
        Name = "Undefined";
        TempMax = 9999f;
        TempMin = -9999;
        PressMax = 99999;
        PressMin = -9999;
        WaterMin = -1;
        WaterMax = 4;
        Priority = 0;
    }
}

public class Hydrosphere
{
    float IceCapsSize { get; set; }
    int WaterLevel { get; set; }
    //Water = 0 -> No water
    //Water = 1 -> Dry
    //Water = 2 -> Wet
    //Water = 3 -> No land
    HydrosphereSource PrimaryWaterSource { get; set; }
    List<HydrosphereSource> availableWaterSources = new List<HydrosphereSource>();

    public static readonly HydrosphereSource NoWater = new HydrosphereSource(name: "None", priority: 1);
    public static readonly HydrosphereSource WaterVapor = new HydrosphereSource(name: "Water Vapor", tempMax: 800, pressMin: 0.3f, waterMax: 0, priority: 2);
    public static readonly HydrosphereSource PolarIceCaps = new HydrosphereSource(name: "Polar Ice Caps", tempMax: 30, waterMin: 1, priority: 3);
    public static readonly HydrosphereSource SurfaceIce = new HydrosphereSource(name: "Surface Ice", tempMax: 0, waterMin: 2, priority: 4);
    public static readonly HydrosphereSource SeaWater = new HydrosphereSource(name: "Sea Water", tempMin: -4, tempMax: 100, waterMin: 3, pressMin: 0.1f, priority: 5);
    public static readonly HydrosphereSource FreshWater = new HydrosphereSource(name: "Fresh Water", tempMin: 0, tempMax: 100, waterMin: 2, pressMin: 0.2f, priority: 7);


    readonly HydrosphereSource[] AllWaterSources = new HydrosphereSource[] {
           NoWater,  WaterVapor, PolarIceCaps, SurfaceIce, SeaWater, FreshWater
    };

    public string GetWaterSourceName()
    {
        return PrimaryWaterSource.GetWaterSourceName();
    }

    public float GetIceCapSize()
    {
        return IceCapsSize;
    }

    public int GetWaterLevel()
    {
        return WaterLevel;
    }

    public Hydrosphere()
    {
        WaterLevel = 0;
        PrimaryWaterSource = NoWater;
        IceCapsSize = 0;
    }


    HydrosphereSource GetBestPossibleWaterSource(int WaterLevel, Atmosphere atm)
    {
        float planetTemp = atm.Temperature;
        float planetPress = atm.Pressure;
        int waterLevel = WaterLevel;

        HydrosphereSource[] allWaterSources = AllWaterSources;
        HydrosphereSource bestHydrosphereSource = NoWater;

        //checks if the feature can exist in the atmosphere and have higher priority (better for humanoids) than NoWater
        foreach (HydrosphereSource waterSource in allWaterSources)
        {
            if (!waterSource.CheckValidity(planetTemp, planetPress, waterLevel)) continue;
            if (bestHydrosphereSource.GetPriority() > waterSource.GetPriority()) continue; // better water source already found

            bestHydrosphereSource = waterSource;
        }
        return bestHydrosphereSource;
    }

    public Hydrosphere(int presetWaterLevel, Atmosphere atmosphere, bool usePolarCaps)
    {
        MathFunctions MathFunctions = new MathFunctions();

        int waterLevel = presetWaterLevel;
        bool polarCapsRelevant = usePolarCaps;


        PrimaryWaterSource = GetBestPossibleWaterSource(waterLevel, atmosphere);
        IceCapsSize = PolarCapsSize(atmosphere.Temperature, waterLevel, polarCapsRelevant);
    }

    public Hydrosphere(PlanetType type, Atmosphere atmosphere)
    {
        MathFunctions MathFunctions = new MathFunctions();

        int[] waterLevelRange = type.WaterLevelRange;
        bool polarCapsRelevant = type.UsePolarCaps;

        int waterLevel = Random.Range(waterLevelRange[0], waterLevelRange[1]);

        PrimaryWaterSource = GetBestPossibleWaterSource(waterLevel, atmosphere);
        IceCapsSize = PolarCapsSize(atmosphere.Temperature, waterLevel, polarCapsRelevant);
    }

    float PolarCapsSize(float temperature, int waterLevel, bool polarCapsRelevant)
    {
        float polarCapsSize = 0f;
        float relevantWetness = waterLevel * 0.35f;

        if (polarCapsRelevant)
        {
            if (relevantWetness > 0.2f && temperature < 30)
            {
                polarCapsSize = -0.0285f * temperature + 0.871f;
                polarCapsSize = Mathf.Clamp(polarCapsSize, 0.15f, 1);
                polarCapsSize *= relevantWetness;
                polarCapsSize = Mathf.Clamp(polarCapsSize, 0.15f, 1);

            }
        }

        return polarCapsSize;
    }

};
#endregion 
