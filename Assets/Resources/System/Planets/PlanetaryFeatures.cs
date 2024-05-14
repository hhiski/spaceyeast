using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
public class PlanetaryFeatureProfile
{
    public PlanetaryFeature HydrosphereFeature;
    public PlanetaryFeature PrimaryFeature;
    public PlanetaryFeature SecondaryFeature;
};*/

public class PlanetaryFeatures 
{

    public List<PlanetaryFeature> PlanetaryFeatureList = new List<PlanetaryFeature>();

    public PlanetaryFeature HydrosphereFeature;

    internal PlanetaryFeatures(Atmosphere atmosphere, PlanetType type, Biosphere biosphere, Hydrosphere hydrosphere)
    {
        RandomFeature randomFeature = new RandomFeature();


        PlanetaryFeatureList.Add(randomFeature.GetRockyFeature(atmosphere, biosphere, hydrosphere));
        PlanetaryFeatureList.Add(randomFeature.GetRockyFeature(atmosphere, biosphere, hydrosphere));
        PlanetaryFeatureList.Add(randomFeature.GetHydroFeature(atmosphere, biosphere, hydrosphere));

        if (biosphere.GetBiosphereLevel() == 2)
        {

            PlanetaryFeatureList.Add(randomFeature.GetBioFeature(atmosphere, biosphere, hydrosphere));
        }

        if (biosphere.GetBiosphereLevel() == 2)
        {

            PlanetaryFeatureList.Add(randomFeature.GetBioFeature(atmosphere, biosphere, hydrosphere));
        }
        
        // Add more features per planet?
        // Add a hydrosphere feature

    }

}

public class PlanetaryFeature
{
    public string Name;
    public string Desc;
    public int Rarity;
    public string LocationType;
    public bool LocationSet;

    public float[] TemperatureRange = new float[1];
    public float[] PressureRange = new float[1];
    public float[] EnvRadioactivityRange = new float[1];
    public int[] BiosphereLevelRange = new int[1];
    public string WaterSource;

    public PlanetaryFeature()
    {
        Name = "None";
        Desc = "";
        Rarity = 0;
        LocationSet = false;
        LocationType = "Any";
        TemperatureRange = new float[] { -274f, 99999f };
        PressureRange = new float[] { -1, 99999f };
        EnvRadioactivityRange = new float[] { -1, 99999f };
        BiosphereLevelRange = new int[] { 0, 4 };
        WaterSource = "Not Set";
    }


}

class PlanetaryFeatureFormation
{


    public readonly PlanetaryFeature WierdRocks = new PlanetaryFeature { Name = "Wierd rocks", Rarity = 1, LocationType = "Land" };
    public readonly PlanetaryFeature GraphiteDust = new PlanetaryFeature { Name = "Graphite Dust", Rarity = 1, LocationType = "Land" };
    public readonly PlanetaryFeature LavaTubes = new PlanetaryFeature { Name = "Lava Tubes", Rarity = 1, LocationType = "Land" };

    //Ore resource features
    public readonly PlanetaryFeature SulfosaltMinerals = new PlanetaryFeature { Name = "Sulfosalt Minerals", Rarity = 1, LocationType = "Land" };
    public readonly PlanetaryFeature SulfideMinerals  = new PlanetaryFeature { Name = "Sulfide Minerals", Rarity = 1 };
    public readonly PlanetaryFeature HalideMinerals = new PlanetaryFeature { Name = "Halide Minerals", Rarity = 1 };
    public readonly PlanetaryFeature SilicateMineral = new PlanetaryFeature { Name = "Silicate Mineral", Rarity = 1 };
    public readonly PlanetaryFeature CarbonateMinerals = new PlanetaryFeature { Name = "Carbonate Minerals", Rarity = 1 };
    public readonly PlanetaryFeature Tantalite = new PlanetaryFeature { Name = "Tantalite", Rarity = 1 };
    public readonly PlanetaryFeature Niobitetantalite = new PlanetaryFeature { Name = "Niobite-tantalite", Rarity = 1 };
    public readonly PlanetaryFeature Yttrocolumbite = new PlanetaryFeature { Name = "Yttrocolumbite", Rarity = 1 };
    public readonly PlanetaryFeature Uranophane = new PlanetaryFeature { Name = "Uranophane", Rarity = 1 };
    public readonly PlanetaryFeature BismuthOxides = new PlanetaryFeature { Name = "BismuthOxides", Rarity = 1 };
    public readonly PlanetaryFeature SilicateDust = new PlanetaryFeature { Name = "Silicate Dust", Rarity = 1 };
    public readonly PlanetaryFeature Cobaltite = new PlanetaryFeature { Name = "Cobaltite", Rarity = 1 };
    public readonly PlanetaryFeature ZinciteMinerals = new PlanetaryFeature { Name = "Zincite Minerals", Rarity = 1 };
    public readonly PlanetaryFeature PhosphateRocks = new PlanetaryFeature { Name = "Phosphate Rocks", Rarity = 1 };
    public readonly PlanetaryFeature Lanthanides = new PlanetaryFeature { Name = "Lanthanides", Rarity = 1 };
    public readonly PlanetaryFeature MetalHalides = new PlanetaryFeature { Name = "Metal-Halides", Rarity = 1 };


    public readonly PlanetaryFeature TidalFlexing = new PlanetaryFeature { Name = "Tidal Flexing", Rarity = 1 };
    public readonly PlanetaryFeature UnstableTectonics = new PlanetaryFeature { Name = "Unstable Tectonics", Rarity = 1 };
    public readonly PlanetaryFeature StrongMagneticField = new PlanetaryFeature { Name = "Strong Magnetic Field", Rarity = 1 };
    public readonly PlanetaryFeature AcidRains = new PlanetaryFeature { Name = "Acid Rains", Rarity = 1, TemperatureRange = new float[] { -30f, 100f }, PressureRange = new float[] { 0.1f, 999f } };
    public readonly PlanetaryFeature ExtremeStorms = new PlanetaryFeature { Name = "Extreme Storms", Rarity = 1, TemperatureRange = new float[] { -50f, 10000f }, PressureRange = new float[] { 0.2f, 999f } };
    public readonly PlanetaryFeature GeomagneticStorms = new PlanetaryFeature { Name = "Geomagnetic Storms", Rarity = 1 };
    public readonly PlanetaryFeature HeavyWater = new PlanetaryFeature { Name = "Heavy Water", Rarity = 1, TemperatureRange = new float[] { -50f, 120f } };
    public readonly PlanetaryFeature HydrochloricLake = new PlanetaryFeature { Name = "Hydrochloric  Lake", Rarity = 1, LocationType = "Land", TemperatureRange = new float[] { -50f, 120f } };
    public readonly PlanetaryFeature NitricLake = new PlanetaryFeature { Name = "Nitric Lake", Rarity = 1, LocationType = "Land", TemperatureRange = new float[] { -50f, 120f } };
    public readonly PlanetaryFeature LiquidQuicksilver = new PlanetaryFeature { Name = "Liquid Quicksilver", Rarity = 2, TemperatureRange = new float[] { 200f, 800f } };
    public readonly PlanetaryFeature GeothermalVents = new PlanetaryFeature { Name = "Geothermal Vents", Rarity = 1, LocationType = "Land" };
    public readonly PlanetaryFeature ActiveVolcano = new PlanetaryFeature { Name = "Active Volcano", Rarity = 1,  LocationType = "Land",TemperatureRange = new float[] { -150f, 720f } };
    public readonly PlanetaryFeature DeepSinkhole = new PlanetaryFeature { Name = "Deep Sinkhole", Rarity = 1, LocationType = "Land" };
    public readonly PlanetaryFeature MethaneIce = new PlanetaryFeature { Name = "Methane Ice", Rarity = 1, TemperatureRange = new float[] { -300f, 5f }, LocationType = "Polar" };
    public readonly PlanetaryFeature HydrothermalVents = new PlanetaryFeature { Name = "Hydrothermal Vents", Rarity = 1, LocationType = "Sea" };

    public readonly PlanetaryFeature PrimordialSoup = new PlanetaryFeature { Name = "Primordial Soup", Rarity = 1, TemperatureRange = new float[] { -5f, 150f }, BiosphereLevelRange = new int[] { 1, 1 }  };
    public readonly PlanetaryFeature MicrobialSlurry = new PlanetaryFeature { Name = "Microbial Slurry", Rarity = 1, BiosphereLevelRange = new int[] { 1, 1 } };
    public readonly PlanetaryFeature WigglyPolymers = new PlanetaryFeature { Name = "Wiggly Polymers", Rarity = 1, BiosphereLevelRange = new int[] { 1, 1 } };
    public readonly PlanetaryFeature GiantAmoeboids = new PlanetaryFeature { Name = "Giant Amoeboids", BiosphereLevelRange = new int[] { 1, 1 } };
    public readonly PlanetaryFeature StickyJelly= new PlanetaryFeature { Name = "Sticky Jelly", Rarity = 1, BiosphereLevelRange = new int[] { 1, 1 } };
    public readonly PlanetaryFeature ReplicatingProteins = new PlanetaryFeature { Name = "Replicating Proteins", Rarity = 1, BiosphereLevelRange = new int[] { 1, 1 } };
    public readonly PlanetaryFeature AminoAcids = new PlanetaryFeature { Name = "Amino Acids", Rarity = 1, BiosphereLevelRange = new int[] { 1, 1 } };
    public readonly PlanetaryFeature SeaSnot = new PlanetaryFeature { Name = "Sea Snot", Rarity = 1, BiosphereLevelRange = new int[] { 1, 1 } };
    public readonly PlanetaryFeature MicrobialMat = new PlanetaryFeature { Name = "Microbial Mat", Rarity = 1, BiosphereLevelRange = new int[] { 1, 1 } };
    public readonly PlanetaryFeature PredatorySponges = new PlanetaryFeature { Name = "Predatory Sponges", Rarity = 1, BiosphereLevelRange = new int[] { 1, 1 } };
    public readonly PlanetaryFeature ToxicSpores = new PlanetaryFeature { Name = "Toxic Spores", Rarity = 1, BiosphereLevelRange = new int[] { 1, 1 } };
    public readonly PlanetaryFeature RadiotrophicYeast = new PlanetaryFeature { Name = "Radiotrophic Yeast", Rarity = 1, BiosphereLevelRange = new int[] { 1, 1 } };
    public readonly PlanetaryFeature FermentingLiquids = new PlanetaryFeature { Name = "Fermenting Liquids", Rarity = 1, BiosphereLevelRange = new int[] { 1, 1 } };
    public readonly PlanetaryFeature ParasiticSpiralloid = new PlanetaryFeature { Name = "Parasitic Spiralloids", Rarity = 1, BiosphereLevelRange = new int[] { 1, 1 } };
    public readonly PlanetaryFeature PinkSlime = new PlanetaryFeature { Name = "Pink Slime", Rarity = 1, BiosphereLevelRange = new int[] { 1, 1 } };
    public readonly PlanetaryFeature BubblingSwamp = new PlanetaryFeature { Name = "Bubbling Swamp", Rarity = 1, BiosphereLevelRange = new int[] { 1, 1 }, LocationType = "Land" };
    public readonly PlanetaryFeature BlueGreenDiazotrophs = new PlanetaryFeature { Name = "Blue-Green Diazotrophs", Rarity = 1, BiosphereLevelRange = new int[] { 1, 1 } };
    public readonly PlanetaryFeature GiantViruses = new PlanetaryFeature { Name = "Giant Viruses", Rarity = 1, BiosphereLevelRange = new int[] { 1, 1 } };
    public readonly PlanetaryFeature GelNodules = new PlanetaryFeature { Name = "Gel Nodules", Rarity = 1, BiosphereLevelRange = new int[] { 1, 1 } };
    public readonly PlanetaryFeature SiliconMicrobes = new PlanetaryFeature { Name = "Silicon Microbes", Rarity = 1, BiosphereLevelRange = new int[] { 1, 1 } };

    //hydrosphere features
    public readonly PlanetaryFeature DrySurface = new PlanetaryFeature { Name = "Completely dry", Rarity = 1, WaterSource = "None" };
    public readonly PlanetaryFeature WaterVaporTraces = new PlanetaryFeature { Name = "Water Vapor Traces", Rarity = 1, WaterSource  = "Water Vapor"};
    public readonly PlanetaryFeature StratosphericIceClouds = new PlanetaryFeature { Name = "Stratospheric Ice Clouds", Rarity = 1, WaterSource = "Water Vapor" };
    public readonly PlanetaryFeature PolarIceCaps = new PlanetaryFeature { Name = "Polar Ice Caps", Rarity = 1, WaterSource = "Polar Ice Caps", LocationType = "Polar" };
    public readonly PlanetaryFeature Oceans = new PlanetaryFeature { Name = "Saline Oceans", Rarity = 1, WaterSource = "Sea Water", LocationType = "Sea" };
    public readonly PlanetaryFeature FreshWater = new PlanetaryFeature { Name = "Freshwater Sources", Rarity = 1, WaterSource = "Fresh Water" };




    internal PlanetaryFeature[] GetHydroFeatureList()
    {
        PlanetaryFeature[] features = new PlanetaryFeature[] {
            DrySurface,WaterVaporTraces,StratosphericIceClouds,PolarIceCaps,Oceans,FreshWater
        };
        return features;
    }

    internal PlanetaryFeature[] GetRockyFeatureList()
    {
        PlanetaryFeature[] features = new PlanetaryFeature[] {
            WierdRocks, GraphiteDust, SulfosaltMinerals, LavaTubes
        , Niobitetantalite, Yttrocolumbite, Yttrocolumbite, Uranophane, BismuthOxides, SilicateDust,
            Cobaltite,ZinciteMinerals,PhosphateRocks,Lanthanides,MetalHalides,Cobaltite,TidalFlexing,UnstableTectonics,StrongMagneticField,
            AcidRains, ExtremeStorms,HeavyWater,HydrochloricLake,NitricLake,LiquidQuicksilver,GeothermalVents,ActiveVolcano,DeepSinkhole,
            MethaneIce,HydrothermalVents,StrongMagneticField,StrongMagneticField,StrongMagneticField,StrongMagneticField,StrongMagneticField,


        };
        return features;
    }

    internal PlanetaryFeature[] GetBioFeatureList()
    {
        PlanetaryFeature[] features = new PlanetaryFeature[] {
            PrimordialSoup, MicrobialSlurry, WigglyPolymers, GiantAmoeboids, StickyJelly, AminoAcids, AminoAcids,
            SeaSnot, MicrobialMat, PredatorySponges, ToxicSpores, RadiotrophicYeast, FermentingLiquids, ParasiticSpiralloid,
            PinkSlime,BubblingSwamp,BlueGreenDiazotrophs,GiantViruses,GelNodules

        };
        return features;
    }

}




public class RandomFeature
{
    PlanetaryFeatureFormation features = new PlanetaryFeatureFormation();

    public PlanetaryFeature GetHydroFeature(Atmosphere atmosphere, Biosphere biosphere, Hydrosphere hydrosphere)
    {
        PlanetaryFeature[] hydroFeatures = features.GetHydroFeatureList();
        PlanetaryFeature randomFeature = GetRandomFeature(atmosphere, biosphere, hydrosphere, hydroFeatures);
        return randomFeature;
    }

    public PlanetaryFeature GetBioFeature(Atmosphere atmosphere, Biosphere biosphere, Hydrosphere hydrosphere)
    {
        PlanetaryFeature[] bioFeatures = features.GetBioFeatureList();
        PlanetaryFeature randomFeature =  GetRandomFeature( atmosphere, biosphere, hydrosphere, bioFeatures);
        return randomFeature;
    }

    public PlanetaryFeature GetRockyFeature(Atmosphere atmosphere, Biosphere biosphere, Hydrosphere hydrosphere)
    {
        PlanetaryFeature[] rockyFeatures = features.GetRockyFeatureList();
        PlanetaryFeature randomFeature = GetRandomFeature(atmosphere, biosphere, hydrosphere, rockyFeatures);
        return randomFeature;
    }

    //gets random planetary feature filtered by atmosphere conditions and weighted by rarity
    PlanetaryFeature GetRandomFeature(Atmosphere atmosphere, Biosphere biosphere, Hydrosphere hydrosphere, PlanetaryFeature[] featureList)
    {


        List<PlanetaryFeature> filteredFeatureList = new List<PlanetaryFeature>();

        //checks if the feature can exist in the atmosphere
        foreach (PlanetaryFeature feature in featureList)
        {
            if (atmosphere.Temperature >= feature.TemperatureRange[0] && atmosphere.Temperature <= feature.TemperatureRange[1])
            {
                if (atmosphere.Pressure >= feature.PressureRange[0] && atmosphere.Temperature <= feature.PressureRange[1])
                {
                    if (atmosphere.Radiation >= feature.EnvRadioactivityRange[0] && atmosphere.Radiation <= feature.EnvRadioactivityRange[1])
                    {
                        if (biosphere.GetBiosphereLevel() >= feature.BiosphereLevelRange[0] && biosphere.GetBiosphereLevel() <= feature.BiosphereLevelRange[1])
                        {
                            if (hydrosphere.GetWaterSourceName() == feature.WaterSource || feature.WaterSource == "Not Set")
                            {
                                filteredFeatureList.Add(feature);
                            }
                        }
                    }
                }
            }
        }

        //high rarity -> more common
        PlanetaryFeature randomFeature = new PlanetaryFeature();
        int weightSum = 0;
        int accumulatedWeightSum = 0;

        foreach (PlanetaryFeature feature in filteredFeatureList)
        {
            weightSum += feature.Rarity;
        }

        int random = Random.Range(0, weightSum - 1);

        foreach (PlanetaryFeature feature in filteredFeatureList)
        {
            accumulatedWeightSum += feature.Rarity;
            if (accumulatedWeightSum > random)
            {

                randomFeature = feature;
                break;
            }
        }

        return randomFeature;
    }
}

    
