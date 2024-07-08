using System.Collections;
using System.Collections.Generic;
using UnityEngine;


#region atmosphere classes
public class Gas
{
    string Name { get; set; }
    string Reaction { get; set; }
    float Rarity { get; set; }
    bool Breathable { get; set; }
    bool Toxic { get; set; }

    public string GetGasName()
    {
        return Name;
    }
    public string GetGasReaction()
    {
        return Reaction;
    }
    public Gas(string name = "Undefined", string reaction = "Undefined", int rarity = 0, bool breathable = false, bool toxic = false)
    {
        Name = name;
        Reaction = reaction;
        Rarity = rarity;
        Breathable = breathable;
        Toxic = toxic;
    }

}




public static class AtmosphereGasFormation
{

    public static Gas None = new Gas(name: "None", reaction: "", breathable: false, toxic: false, rarity: 1);
    public static Gas Trace = new Gas(name: "Trace gases", reaction: "Vacuumlike", breathable: false, toxic: false, rarity: 1);
    public static Gas NitrogenCarbonDioxide = new Gas(name: "Nitrogen-Carbon Dioxide", reaction: "Asphyxiant", breathable: false, toxic: false, rarity: 1);
    public static Gas NitrogenMethane = new Gas(name: "Nitrogen-Methane", reaction: "Asphyxiant", breathable: false, toxic: false, rarity: 1);
    public static Gas NitrogenOxygen = new Gas(name: "Nitrogen-Oxygen", reaction: "Normoxic", breathable: true, toxic: false, rarity: 1);
    public static Gas NitrogenSulfurDioxide = new Gas(name: "Nitrogen-Sulfur Dioxide", reaction: "Corrosive", breathable: false, toxic: true, rarity: 1);
    public static Gas NitrogenChlorine = new Gas(name: "Nitrogen-Chlorine", reaction: "Corrosive", breathable: false, toxic: true, rarity: 1);
    public static Gas CarbonDioxideNitrogen = new Gas(name: "Carbon Dioxide-Nitrogen", reaction: "Asphyxiant", breathable: false, toxic: false, rarity: 1);
    public static Gas CarbonDioxideOxygen = new Gas(name: "Carbon Dioxide-Oxygen", reaction: "Hypoxic", breathable: true, toxic: false, rarity: 1);
    public static Gas CarbonDioxideMethane = new Gas(name: "Carbon Dioxide-Methane", reaction: "Asphyxiant", breathable: false, toxic: false, rarity: 1);
    public static Gas HeliumHydrogen = new Gas(name: "Helium-Hydrogen", reaction: "Asphyxiant", breathable: false, toxic: false, rarity: 1);
    public static Gas HydrogenHelium = new Gas(name: "Hydrogen-Helium", reaction: "Asphyxiant", breathable: false, toxic: false, rarity: 1);
    public static Gas HydrogenNitrogen = new Gas(name: "Hydrogen-Nitrogen", reaction: "Asphyxiant", breathable: false, toxic: false, rarity: 1);
    public static Gas HydrogenWater = new Gas(name: "Hydrogen-Water", reaction: "Asphyxiant", breathable: false, toxic: false, rarity: 1);
    public static Gas OxygenNitrogen = new Gas(name: "Oxygen-Nitrogen", reaction: "Hyperoxic", breathable: true, toxic: false, rarity: 1);
    public static Gas SiliconMonoxide = new Gas(name: "Silicon Monoxide", reaction: "Hot", breathable: false, toxic: false, rarity: 1);

   

    public static Gas[] GiantMix = new Gas[] { HydrogenNitrogen, HeliumHydrogen };
    public static Gas[] WaterMix = new Gas[] { NitrogenOxygen, OxygenNitrogen };
    public static Gas[] GreenMix = new Gas[] { CarbonDioxideNitrogen, CarbonDioxideOxygen, CarbonDioxideMethane };
    public static Gas[] HyceanMix = new Gas[] { HydrogenNitrogen, HydrogenHelium, HydrogenWater };
    public static Gas[] CommonMix = new Gas[] { NitrogenOxygen, NitrogenCarbonDioxide, NitrogenMethane, NitrogenSulfurDioxide, CarbonDioxideNitrogen };


}

public class Atmosphere
{

    public float Radiation { get; set; }
    public float Pressure { get; set; }
    public float TemperatureB { get; set; } //Black-body temperature
    public float TemperatureG { get; set; } //Black-body temperature + Greenhouse-effect
    public float Temperature { get; set; } // Simplified

    public Gas PrimaryGas;


};

#endregion 
