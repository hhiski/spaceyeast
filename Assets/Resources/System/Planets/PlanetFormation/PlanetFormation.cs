using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MathSpace;


#region PlanetTypes
public class PlanetType
{
    public string Name { get; set; } = "EmptyType";
    public float Albedo { get; set; } = 0;
    public float SurfaceVariation { get; set; } = 0.3f;
    public float LifeProbability { get; set; } = 0;
    public float[] TemperatureRange { get; set; } = new float[1]; //min and max values
    public  float[] GreenhouseEffectRange { get; set; } = new float[1]; //not used?
    public  float[] PressureRange { get; set; } = new float[1]; //min and max values
    public  float[] EnvRadioactivityRange { get; set; } = new float[1]; //terrestrial radiation (not cosmic rays!), min and max values 
    public  int[] WaterLevelRange { get; set; } = new int[] { 0, 2 }; //0 = completely waterless, 1 = arid, 2 = really wet,earth hydrosphere equivalent, 3 no solid surface

    public SurfaceType SurfaceType = new SurfaceType();
    public Gas[] GasList = new Gas[] { }; // list of possible gas mixes

    public bool RandomAtmosphereAvailable = false;
    public string CustomSubType = "";

    public Atmosphere RandomAtmosphere = new Atmosphere(); //for storing random the atmosphere within the type frames

    public bool UsePolarCaps = true;
    public bool UseAtmosphericClouds = true;

    void SetTypeValues(PlanetType type)
    {
        Name = type.Name;
        GasList = type.GasList;
        Albedo = type.Albedo;
        TemperatureRange = type.TemperatureRange;
        GreenhouseEffectRange = type.GreenhouseEffectRange;
        PressureRange = type.PressureRange;
        EnvRadioactivityRange = type.EnvRadioactivityRange;
        WaterLevelRange = type.WaterLevelRange;
        SurfaceVariation = type.SurfaceVariation;
        LifeProbability = type.LifeProbability;
        SurfaceType = type.SurfaceType;
    }

    public PlanetType() { }

    public string GetName() { return Name; }
    public Gas[] GetGasList() { return GasList; }
    public float GetAlbedo() { return Albedo; }
    public float[] GetTemperatureRange() { return TemperatureRange; }
    public float[] GetGreenhouseEffectRange() { return GreenhouseEffectRange; }
    public float[] GetPressureRange() { return PressureRange; }
    public float[] GetEnvRadioactivityRange() { return EnvRadioactivityRange; }
    public int[] GetWaterLevelRange() { return WaterLevelRange; }
    public float GetLifeProbability() { return LifeProbability; }
    public SurfaceType GetSurfaceType() { return SurfaceType; }
    public bool GetAtmosphericCloudUsage() { return UseAtmosphericClouds; }

    public void SetName(string name) { Name = name; }
    public void SetGasList(Gas[] gases) { GasList = gases; }
    public void SetAlbedo(float albedo) { Albedo = albedo; }
    public void SetTemperatureRange(float[] range) { TemperatureRange = range; }
    public void SetGreenhouseEffectRange(float[] range) { GreenhouseEffectRange = range; }
    public void SetPressureRange(float[] range) { PressureRange = range; }
    public void SetEnvRadioactivityRange(float[] range) { EnvRadioactivityRange = range; }
    public void SetWaterLevelRange(int[] range) { WaterLevelRange = range; }
    public void SetLifeProbability(float probability) { LifeProbability = probability; }
    public void SetSurfaceType(SurfaceType surfaceType) { SurfaceType = surfaceType; }



    public PlanetType(PlanetType type)
    {
        Name = type.GetName();
        GasList = type.GetGasList();
        Albedo = type.GetAlbedo();
        TemperatureRange = type.GetTemperatureRange();
        GreenhouseEffectRange = type.GetGreenhouseEffectRange();
        PressureRange = type.GetEnvRadioactivityRange();
        EnvRadioactivityRange = type.GetPressureRange();
        WaterLevelRange = type.GetWaterLevelRange();
        LifeProbability = type.GetLifeProbability();
        SurfaceType = type.GetSurfaceType();
    }



    public PlanetType(string name, float albedo, Gas[] gasList)
    {
        Name = name;
        Albedo = albedo;
        GasList = gasList;
    }

    public PlanetType(string typeName)
    {

        bool viable = false;
        PlanetFormation planetFormation = new PlanetFormation();
        List<PlanetType> EveryPlanetTypeList = new List<PlanetType>();
        EveryPlanetTypeList = planetFormation.PopulatePlanetList();

        foreach (PlanetType type in EveryPlanetTypeList)
        {
            if (type.Name == typeName)
            {
                Name = typeName;
                SetTypeValues(type);
                viable = true;
                break;
            }
        }

        if (viable == false)
        {
            Debug.Log("CANT FIND PLANET TYPE :" + typeName);

        }

    }

    public PlanetType(float orbitalDistance, float solarTemperature) {

        bool viable = false;
        PlanetFormation planetFormation = new PlanetFormation();
        PlanetAtmosphereConditions planetAtmosphereConditions = new PlanetAtmosphereConditions();

        List<PlanetType> EveryPlanetTypeList = new List<PlanetType>();

        EveryPlanetTypeList = planetFormation.PopulatePlanetList();

        while (!viable)
            {
                int random = Random.Range(0, EveryPlanetTypeList.Count);
                PlanetType randomType = EveryPlanetTypeList[random];
                float minTemp = randomType.TemperatureRange[0];
                float maxTemp = randomType.TemperatureRange[1];

                RandomAtmosphere = planetAtmosphereConditions.AtmosphereProbability(orbitalDistance, solarTemperature, randomType);
                float temp = RandomAtmosphere.Temperature;
                
                // Checks if the planets atmosphere temperature is okey for its type.
                if (temp < maxTemp)
                {
                    if (temp > minTemp)
                    {
                        SetTypeValues(randomType);
                        RandomAtmosphereAvailable = true;
                        viable = true;

                    }
                }
            }
    }
}


 class PlanetFormation
{

    public List<PlanetType> PopulatePlanetList()
    {
      
   
        List<PlanetType> EveryPlanetTypeList = new List<PlanetType>();


        PlanetType Minor = new PlanetType
        {
            Name = "Minor",
            GasList = AtmosphereGasFormation.WaterMix,
            SurfaceType = SurfaceType.Silicate,
            Albedo = 0.1f,
            TemperatureRange = new float[] { -1000, -1000 },
            GreenhouseEffectRange = new float[] { 0, 0 },
            PressureRange = new float[] { 0, 0f },
            EnvRadioactivityRange = new float[] { 0.0001f, 0.001f },
            WaterLevelRange = new int[] { 0, 1 },
            LifeProbability = 0.0f,
            UsePolarCaps = false
        };
        EveryPlanetTypeList.Add(Minor);

        PlanetType Water = new PlanetType
        {
            Name = "Water",
            Albedo = 0.3f,
            GasList = AtmosphereGasFormation.WaterMix,
            PressureRange = new float[] { 0.2f, 3f },
            TemperatureRange = new float[] { -5, 60 },
            GreenhouseEffectRange = new float[] { 25, 36 },
            EnvRadioactivityRange = new float[] { 0.0005f, 0.01f },
            WaterLevelRange = new int[] { 2, 2 },
            LifeProbability = 0.8f,
            SurfaceType = SurfaceType.Silicate,
        };
        EveryPlanetTypeList.Add(Water);


        PlanetType Dust = new PlanetType
        {
            Name = "Dust",
            GasList = AtmosphereGasFormation.CommonMix,
            Albedo = 0.3f,
            TemperatureRange = new float[] { -170, 170 },
            GreenhouseEffectRange = new float[] { 5, 30 },
            PressureRange = new float[] { 0.2f, 3f },
            EnvRadioactivityRange = new float[] { 0.0001f, 0.01f },
            WaterLevelRange = new int[] { 0, 1 },
            SurfaceType = SurfaceType.Dust,
            LifeProbability = 0.2f,
        };
        EveryPlanetTypeList.Add(Dust);

        PlanetType Rocky = new PlanetType
        {
            Name = "Rocky",
            GasList = AtmosphereGasFormation.CommonMix,
            Albedo = 0.1f,
            TemperatureRange = new float[] { -1000, 1200 },
            GreenhouseEffectRange = new float[] { 0, 1 },
            PressureRange = new float[] { 0, 0.0001f },
            EnvRadioactivityRange = new float[] { 0.0001f, 0.001f },
            WaterLevelRange = new int[] { 0, 1 },
            LifeProbability = 1.0f,
            SurfaceType = SurfaceType.Silicate,

        };
        EveryPlanetTypeList.Add(Rocky);

        PlanetType Hycean = new PlanetType
        {
            Name = "Hycean",
            Albedo = 0.8f,
            GasList = AtmosphereGasFormation.HyceanMix,
            TemperatureRange = new float[] { -0, 200 },
            GreenhouseEffectRange = new float[] { 10, 40 },
            PressureRange = new float[] { 1f, 40.5f },
            EnvRadioactivityRange = new float[] { 0, 0.02f },
            WaterLevelRange = new int[] { 3, 3 },
            LifeProbability = 0.4f,
            UsePolarCaps = true,
            SurfaceType = SurfaceType.Water,
        };
        EveryPlanetTypeList.Add(Hycean);

        PlanetType Ice = new PlanetType
        {
            Name = "Ice",
            Albedo = 0.8f,
            GasList = AtmosphereGasFormation.CommonMix,
            TemperatureRange = new float[] { -1000, -5 },
            GreenhouseEffectRange = new float[] { 0, 3 },
            PressureRange = new float[] { 0f, 0.5f },
            EnvRadioactivityRange = new float[] { 0, 0.02f },
            WaterLevelRange = new int[] { 2, 2 },
            LifeProbability = 0.1f,
            UsePolarCaps = false,
            SurfaceType = SurfaceType.Ice,
        };
        EveryPlanetTypeList.Add(Ice);

        PlanetType Snow = new PlanetType
        {
            Name = "Snow",
            Albedo = 0.8f,
            GasList = AtmosphereGasFormation.CommonMix,
            TemperatureRange = new float[] { -150, 0 },
            GreenhouseEffectRange = new float[] { 0, 7 },
            PressureRange = new float[] { 0f, 0.3f },
            EnvRadioactivityRange = new float[] { 0.1f, 0.7f },
            WaterLevelRange = new int[] { 2, 2 },
            LifeProbability = 0.3f,
            UsePolarCaps = false,
            SurfaceType = SurfaceType.Ice,
        };
        EveryPlanetTypeList.Add(Snow);

        PlanetType Mud = new PlanetType
        {
            Name = "Mud",
            Albedo = 0.2f,
            GasList = AtmosphereGasFormation.CommonMix,
            TemperatureRange = new float[] { -100, 60 },
            GreenhouseEffectRange = new float[] { 25, 33 },
            PressureRange = new float[] { 0.4f, 5.7f },
            EnvRadioactivityRange = new float[] { 0.001f, 0.05f },
            WaterLevelRange = new int[] { 2, 2 },
            LifeProbability = 0.7f,
            SurfaceType = SurfaceType.Silicate,
        };
        EveryPlanetTypeList.Add(Mud);

        PlanetType Greenhouse = new PlanetType
        {
            Name = "Greenhouse",
            GasList = AtmosphereGasFormation.GreenMix,
            Albedo = 0.7f,
            TemperatureRange = new float[] { 100, 600 },
            GreenhouseEffectRange = new float[] { 100, 400 },
            PressureRange = new float[] { 30f, 100f },
            EnvRadioactivityRange = new float[] { 0.001f, 0.05f },
            WaterLevelRange = new int[] { 0, 2 },
            LifeProbability = 0.05f,
            UsePolarCaps = false,
            SurfaceType = SurfaceType.Silicate,
        };
        EveryPlanetTypeList.Add(Greenhouse);

        PlanetType Gas = new PlanetType
        {
            Name = "Gas",
            Albedo = 0.55f,
            GasList = AtmosphereGasFormation.GiantMix,
            TemperatureRange = new float[] { -1000, 20000 },
            GreenhouseEffectRange = new float[] { 5, 40 },
            PressureRange = new float[] { 20f, 300f },
            EnvRadioactivityRange = new float[] { 0.001f, 5f },
            WaterLevelRange = new int[] { 0, 1 },
            UsePolarCaps = false,
            UseAtmosphericClouds = false,
            SurfaceType = SurfaceType.Gas,

        };
        EveryPlanetTypeList.Add(Gas);

        PlanetType Metallic = new PlanetType
        {
            Name = "Metallic",
            GasList = AtmosphereGasFormation.CommonMix,
            Albedo = 0.1f,
            TemperatureRange = new float[] { -1000, 1300 },
            GreenhouseEffectRange = new float[] { 0, 2 },
            PressureRange = new float[] { 0f, 0.5f },
            EnvRadioactivityRange = new float[] { 0.005f, 0.1f },
            WaterLevelRange = new int[] { 0, 1 },
            SurfaceType = SurfaceType.Metallic
            
        };
        EveryPlanetTypeList.Add(Metallic);

        PlanetType Nitrate = new PlanetType
        {
            Name = "Nitrate",
            GasList = AtmosphereGasFormation.CommonMix,
            Albedo = 0.7f,
            TemperatureRange = new float[] { -1000, -5 },
            GreenhouseEffectRange = new float[] { 0, 4 },
            PressureRange = new float[] { 0f, 0.03f },
            EnvRadioactivityRange = new float[] { 0.0001f, 0.002f },
            WaterLevelRange = new int[] { 0, 1 },
            LifeProbability = 0.15f,
            SurfaceType = SurfaceType.Salt,

        };
        EveryPlanetTypeList.Add(Nitrate);

        PlanetType Lava = new PlanetType
        {
            Name = "Lava",
            Albedo = 0.05f,
            GasList = AtmosphereGasFormation.CommonMix,
            SurfaceType = SurfaceType.Lava,
            TemperatureRange = new float[] { 1200, 999999 },
            GreenhouseEffectRange = new float[] { 0, 5 },
            PressureRange = new float[] { 0f, 0.001f },
            EnvRadioactivityRange = new float[] { 0.02f, 0.2f },
            WaterLevelRange = new int[] { 0, 0 },
            UsePolarCaps = false,

        };
        EveryPlanetTypeList.Add(Lava);


        PlanetType Acid = new PlanetType
        {
            Name = "Acid",
            Albedo = 0.2f,
            GasList = AtmosphereGasFormation.CommonMix,
            SurfaceType = SurfaceType.Silicate,
            TemperatureRange = new float[] { -100, 340 },
            GreenhouseEffectRange = new float[] { 5, 60 },
            PressureRange = new float[] { 0.01f, 6.5f },
            EnvRadioactivityRange = new float[] { 0.0001f, 0.1f },
            WaterLevelRange = new int[] { 0, 2 },
            LifeProbability = 0.5f,


        };
        EveryPlanetTypeList.Add(Acid);

        PlanetType Radioactive = new PlanetType
        {
            Name = "Radioactive",
            Albedo = 0.1f,
            GasList = AtmosphereGasFormation.CommonMix,
            TemperatureRange = new float[] { -300, 840 },
            GreenhouseEffectRange = new float[] { 5, 60 },
            PressureRange = new float[] { 0.01f, 6.5f },
            EnvRadioactivityRange = new float[] { 0.5f, 1f },
            WaterLevelRange = new int[] { 0, 2 },
            LifeProbability = 0.05f,
            SurfaceType = SurfaceType.Transuranic,

        };
        EveryPlanetTypeList.Add(Radioactive);

        PlanetType Asphalt = new PlanetType
        {
            Name = "Asphalt",
            Albedo = 0.05f,
            GasList = AtmosphereGasFormation.CommonMix,
            TemperatureRange = new float[] { -1000, 130 },
            GreenhouseEffectRange = new float[] { 0, 15 },
            PressureRange = new float[] { 0.0f, 0.5f },
            EnvRadioactivityRange = new float[] { 0.0001f, 0.002f },
            WaterLevelRange = new int[] { 0, 1 },
            SurfaceType = SurfaceType.Hydrocarbon,
        };
        EveryPlanetTypeList.Add(Asphalt);

        PlanetType Tholin = new PlanetType
        {
            Name = "Tholin",
            Albedo = 0.5f,
            GasList = AtmosphereGasFormation.CommonMix,
            TemperatureRange = new float[] { -1000, -50 },
            GreenhouseEffectRange = new float[] { 0, 0 },
            PressureRange = new float[] { 0.0f, 1f },
            EnvRadioactivityRange = new float[] { 0.0001f, 0.002f },
            WaterLevelRange = new int[] { 0, 2 },
            LifeProbability = 0.05f,
            SurfaceType = SurfaceType.Hydrocarbon,

        };
        EveryPlanetTypeList.Add(Tholin);

        PlanetType Chromic = new PlanetType
        {
            Name = "Chromic",
            Albedo = 0.7f,
            GasList = AtmosphereGasFormation.CommonMix,
            TemperatureRange = new float[] { -1000, 1900 },
            GreenhouseEffectRange = new float[] { 0, 1 },
            PressureRange = new float[] { 0.0f, 0.001f },
            EnvRadioactivityRange = new float[] { 0.0001f, 0.002f },
            WaterLevelRange = new int[] { 0, 2 },
            SurfaceType = SurfaceType.Chromic,

        };
        EveryPlanetTypeList.Add(Chromic);

        PlanetType Carbon = new PlanetType
        {
            Name = "Carbon",
            Albedo = 0.05f,
            GasList = AtmosphereGasFormation.CommonMix,
            TemperatureRange = new float[] { -1000, -50 },
            GreenhouseEffectRange = new float[] { 0, 5 },
            PressureRange = new float[] { 0.0f, 2.000f },
            EnvRadioactivityRange = new float[] { 0.0001f, 0.1f },
            WaterLevelRange = new int[] { 0, 0 },
            LifeProbability = 0.2f,
            SurfaceType = SurfaceType.Carbide,

        };
        EveryPlanetTypeList.Add(Carbon);

        PlanetType Slime = new PlanetType
        {
            Name = "Slime",
            Albedo = 0.4f,
            GasList = AtmosphereGasFormation.CommonMix,
            TemperatureRange = new float[] { -20, 100 },
            GreenhouseEffectRange = new float[] { 0, 80 },
            PressureRange = new float[] { 0.1f, 20.000f },
            EnvRadioactivityRange = new float[] { 0.0f, 0.0001f },
            WaterLevelRange = new int[] { 1, 2 },
            LifeProbability = 0.01f,
            UsePolarCaps = true,
            SurfaceType = SurfaceType.Slime,
        };
        EveryPlanetTypeList.Add(Slime);

        PlanetType Chondrite = new PlanetType
        {
            Name = "Chondrite",
            Albedo = 0.4f,
            GasList = AtmosphereGasFormation.CommonMix,
            TemperatureRange = new float[] { -999, -60 },
            GreenhouseEffectRange = new float[] { 0, 4 },
            PressureRange = new float[] { 0.00f, 0.050f },
            EnvRadioactivityRange = new float[] { 0.00001f, 0.002f },
            WaterLevelRange = new int[] { 0, 2 },
            LifeProbability = 0.8f,
            UsePolarCaps = false,
            SurfaceType = SurfaceType.Silicate,
        };
        EveryPlanetTypeList.Add(Chondrite);

        PlanetType Pnictogen = new PlanetType
        {
            Name = "Pnictogen",
            GasList = AtmosphereGasFormation.CommonMix,
            Albedo = 0.5f,
            TemperatureRange = new float[] { -1000, 500 },
            GreenhouseEffectRange = new float[] { 0, 2 },
            PressureRange = new float[] { 0f, 0.01f },
            EnvRadioactivityRange = new float[] { 0.005f, 0.1f },
            WaterLevelRange = new int[] { 0, 2 },
            SurfaceType = SurfaceType.Pnictogen,

        };

        EveryPlanetTypeList.Add(Pnictogen);

        PlanetType Sulfide = new PlanetType
        {
            Name = "Sulfide",
            GasList = AtmosphereGasFormation.CommonMix,
            Albedo = 0.3f,
            SurfaceType = SurfaceType.Silicate,
            TemperatureRange = new float[] { -300, 500 },
            GreenhouseEffectRange = new float[] { 0, 2 },
            PressureRange = new float[] { 0f, 0.5f },
            EnvRadioactivityRange = new float[] { 0.000f, 0.01f },
            WaterLevelRange = new int[] { 0, 2 },


        
        };
        EveryPlanetTypeList.Add(Sulfide);

        PlanetType Colloid = new PlanetType
        {
            Name = "Colloid",
            GasList = AtmosphereGasFormation.CommonMix,
            Albedo = 0.3f,
            SurfaceType = SurfaceType.Dust,
            TemperatureRange = new float[] { -50, 200 },
            GreenhouseEffectRange = new float[] { 0, 4 },
            PressureRange = new float[] { 0f, 30f },
            EnvRadioactivityRange = new float[] { 0.000f, 0.01f },
            WaterLevelRange = new int[] { 1, 2 },
  


        };
        EveryPlanetTypeList.Add(Colloid);


        PlanetType Lanthanide = new PlanetType
        {
            Name = "Lanthanide",
            GasList = AtmosphereGasFormation.CommonMix,
            Albedo = 0.3f,
            TemperatureRange = new float[] { 200, 1000 },
            GreenhouseEffectRange = new float[] { 0, 2 },
            PressureRange = new float[] { 0f, 0.01f },
            EnvRadioactivityRange = new float[] { 0.05f, 2f },
            WaterLevelRange = new int[] { 0, 2 },
            SurfaceType = SurfaceType.Metallic,

        };
        EveryPlanetTypeList.Add(Lanthanide);

        PlanetType Sodium = new PlanetType
        {
            Name = "Sodium",
            GasList = AtmosphereGasFormation.CommonMix,
            Albedo = 0.8f,
            TemperatureRange = new float[] { -400, 300 },
            GreenhouseEffectRange = new float[] { 0, 4 },
            PressureRange = new float[] { 0f, 0.8f },
            EnvRadioactivityRange = new float[] { 0.000f, 0.01f },
            WaterLevelRange = new int[] { 0, 1 },
            SurfaceType = SurfaceType.Salt
        };

        PlanetType Superdense = new PlanetType
        {
            Name = "Superdense",
            GasList = AtmosphereGasFormation.CommonMix,
            Albedo = 0.05f,
            TemperatureRange = new float[] { -400, 900 },
            GreenhouseEffectRange = new float[] { 0, 4 },
            PressureRange = new float[] { 0f, 10.8f },
            EnvRadioactivityRange = new float[] { 0.000f, 0.1f },
            WaterLevelRange = new int[] { 0, 1 },
            SurfaceType = SurfaceType.Metallic
        };

        EveryPlanetTypeList.Add(Superdense);

        PlanetType Sulfur = new PlanetType
        {
            Name = "Sulfur",
            GasList = AtmosphereGasFormation.CommonMix,
            Albedo = 0.5f,
            TemperatureRange = new float[] { -400, 100 },
            GreenhouseEffectRange = new float[] { 0, 50 },
            PressureRange = new float[] { 0f, 30.0f },
            EnvRadioactivityRange = new float[] { 0.000f, 1f },
            WaterLevelRange = new int[] { 0, 2 },
            SurfaceType = SurfaceType.Silicate
        };

        EveryPlanetTypeList.Add(Sulfur);
        return EveryPlanetTypeList;
    }
}

#endregion

#region PlanetAtmosphereConditions
class PlanetAtmosphereConditions
{


    public Atmosphere AtmosphereProbability(float orbitDistance, float solarTemperature, PlanetType type)
    {
        int randomGasIndex = Random.Range(0, type.GasList.Length);
        Gas randomGas = type.GasList[randomGasIndex];
        float randomPressure = Random.Range(type.PressureRange[0], type.PressureRange[1]);
        float randomRadiation = Random.Range(type.EnvRadioactivityRange[0], type.EnvRadioactivityRange[1]);
        float blackBodyTemperature = GetEffectiveTemperature(orbitDistance, type.Albedo, solarTemperature);
        float greenhouseTemperature = GetGreenhouseTemperature(blackBodyTemperature, randomPressure, randomGas);

        if (randomPressure < 0.0099f)
        {
            randomGas = AtmosphereGasFormation.None;
        }
        else if (randomPressure < 0.099f)
        {
            randomGas = AtmosphereGasFormation.Trace;
        }
        

        Atmosphere atm = new Atmosphere
        {
            TemperatureB = blackBodyTemperature,
            Pressure = randomPressure,
            Radiation = randomRadiation,
            TemperatureG = greenhouseTemperature,
            Temperature = blackBodyTemperature + greenhouseTemperature,
            PrimaryGas = randomGas
        };


        return atm;
    }



    float GetGreenhouseTemperature(float blackBodyTemperature, float pressure, Gas gas)
    {
        float greenhouseTemperatureEffect = Mathf.Pow(pressure, 0.5975f) * 34;

        float absorption = 1;
        if (blackBodyTemperature > 400) { absorption = 1.6f; }
        if (blackBodyTemperature <= 400 && blackBodyTemperature > 200) { absorption = 1.3f; }
        if (blackBodyTemperature <= 100 && blackBodyTemperature > -100) { absorption = 1f; }
        if (blackBodyTemperature <= -100 && blackBodyTemperature > -180) { absorption = 0.6f; }
        if (blackBodyTemperature <= -180) { absorption = 0.2f; }

        if (gas.GetGasName() == "Helium-Hydrogen" || gas.GetGasName() == "Hydrogen-Helium")
        {
            absorption /= 4.5f;
        }

        greenhouseTemperatureEffect *= absorption;

        return greenhouseTemperatureEffect;
    }

    float GetEffectiveTemperature(float distance, float albedo, float solarTemp)
    {
        float temperatureB = 0;


        temperatureB = -1.2f * distance + 365;

        float starTemperatureMultiplier = 0.000226f * solarTemp - 0.186f;

        temperatureB *= starTemperatureMultiplier;
        // distance = distance / 80.000f; // distance conversion to AUs




        /*
        if (distance <= 0.55f)
        {
            temperatureB = -560f * distance + 733f;
        }
        else if (distance > 0.55f && distance <= 0.819f)
        {
            temperatureB = -500f * distance + 700f;
        }
        else if (distance > 0.819f && distance <= 2.968f)
        {
            temperatureB = -107f * distance + 378f;
        }
        else
        {
            temperatureB = -13.7f * distance + 101;
        }


        if (temperatureB <= 3) { temperatureB = 3; }; // Planets cant get colder than space

        temperatureB = temperatureB * (0.000173f * solarTemp - 0.0008f); //Solar Heat Factor
        temperatureB = temperatureB - (temperatureB * (0.1667f * albedo)); //Bond albedo reflection
        */

        return temperatureB - 273;
    }

}
#endregion



