using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Math;

using static CelestialBody;
using Unity.VisualScripting;
using System;
using OpenCover.Framework.Model;
using System.Linq;
using System.Collections.Generic;

public class Premaid
{
    StarFormation starFormation = new();
    MathFunctions MathFunctions = new();

    Names nameList = new();

    System.Random random = new System.Random();

    int TotalPlanetCount = 0;
    int TotalStarCount = 0;

    float maxObjectDistanceToSun = 450f;
    float minObjectDistanceToSun = 25f;
    float meanPlanetDistance = 30f;



    public Universe GenericGalaxy()
    {
        Universe galaxy = new(0); 
        galaxy.Name = "Milkyway";
        // int galacticZoneNumber = ConfigManager.GetInstance().gameConfig.GalacticMapZoneNumber; 
        int galacticZoneNumber = 33;

     


        for (int id = 0; id <= galacticZoneNumber; id++)
        {
            if (id == 3)
            {
                Cluster LocalBubble = LocalBubbleCluster(3);
                galaxy.Clusters.Add(LocalBubble);
            }
         else
            {
                Cluster GenericCluster =  GenericClusterd(id);
                galaxy.Clusters.Add(GenericCluster);
            }
        }
          
        return galaxy;
    }

    public Cluster GenericClusterd(int clusterId )
    {
        Cluster GenericCluster = new Cluster();

        GenericCluster.Name = nameList.getRandomZoneName();

        int meanNumberOfStars, randomNumber, numberOfStars, meanNumberOfPlanets, numberOfPlanets;


         meanNumberOfStars = ConfigManager.GetInstance().gameConfig.AvgStarNumPeCluster;
         randomNumber = UnityEngine.Random.Range(0, 122);
        numberOfStars = meanNumberOfStars;
        numberOfStars = Mathf.Clamp(numberOfStars, 1, 100);




        for (int starIndex = 0; starIndex < numberOfStars; starIndex++)
        {
                
            Star Star = new(clusterId);

            Star.Name = nameList.getRandomStarName();
            Star.Type = starFormation.GetRandomTypeStar();
            Star.SolarTemperature = starFormation.getStarTypeRandomTemperature(Star.Type);



            int beltType = 1;
            int numOfBelts = 0;

            //  GenerateSolarSystemObjects(Star, systemPlanetNum, numOfBelts);
            //    GenerateBelts(Star, numOfBelts, beltType);
           // int maxSolarRadius = ConfigManager.GetInstance().gameConfig.MaxSolarRadius;
            meanNumberOfPlanets = ConfigManager.GetInstance().gameConfig.AvgPlanetNumPerStar;
            numberOfPlanets = (int)Game.Math.MathFunctions.StandardDeviation(meanNumberOfPlanets, meanNumberOfPlanets, randomNumber);
            numberOfPlanets = Mathf.Clamp(numberOfPlanets, 1, 10);

           // GeneratePlanets(Star, numberOfPlanets);
            Star = GenerateSolarSystemObjects(Star);

            GenericCluster.Stars.Add(Star);


        }

        return GenericCluster;
    }

    enum SystemSlot
    {
        EmptySpace,
        Planet,
        AsteroidBelt
        //more?
    };


    void ShuffleSystemSlots(SystemSlot[] array)
    {
        int n = array.Length;
        while (n > 1)
        {
            n--;
            int k = random.Next(n + 1);
            SystemSlot value = array[k];
            array[k] = array[n];
            array[n] = value;
        }

    }

    Star GenerateSolarSystemObjects(Star star)
    {


       

        Star planetHostingStar = star;

        int numOfBelts = UnityEngine.Random.Range(0, 3);
        int randomInteger, meanNumberOfPlanets, numberOfPlanets, maxSolarRadius, NumberOfSolarObjects, NumberOfSolarSlots, planetsLeft, beltsLeft;
        float orbitDistance;

        maxSolarRadius = ConfigManager.GetInstance().gameConfig.MaxSolarRadius;
        meanNumberOfPlanets = ConfigManager.GetInstance().gameConfig.AvgPlanetNumPerStar;
        numberOfPlanets = meanNumberOfPlanets;
        numberOfPlanets = Mathf.Clamp(numberOfPlanets, 1, 100);
        planetsLeft = numberOfPlanets;
        beltsLeft = numOfBelts;
        NumberOfSolarObjects = numOfBelts + numberOfPlanets;
        NumberOfSolarSlots = NumberOfSolarObjects + 5;

        SystemSlot[] orbitSlots = new SystemSlot[NumberOfSolarSlots];

        for (int orbitSlotIndex = 0; orbitSlotIndex < NumberOfSolarSlots; orbitSlotIndex++)
         {
             if (planetsLeft > 0)
             {
                 orbitSlots[orbitSlotIndex] = (SystemSlot.Planet);
                 planetsLeft--;
             }
             else if (beltsLeft > 0)
             {
                orbitSlots[orbitSlotIndex] = SystemSlot.AsteroidBelt;
                beltsLeft--;
             }
            else 
            {
               orbitSlots[orbitSlotIndex] = SystemSlot.EmptySpace;

            }
        }

       ShuffleSystemSlots(orbitSlots);

        orbitDistance = 22f + MathFunctions.StandardDeviation(40f, 15f, planetHostingStar.Id);


        int planetOrderIndex = 1;


        /*
        for (int orbitSlotIndex = 0; orbitSlotIndex < orbitSlots.Length; orbitSlotIndex++)
        {
            SystemSlot slot = orbitSlots[orbitSlotIndex];
            if (slot == SystemSlot.Planet)
            {
                Planet planet = new(orbitDistance, planetHostingStar.SolarTemperature);
                planet.Name = nameList.getPlanetName(planetHostingStar.Name, planetOrderIndex);
                planetOrderIndex++;
                orbitDistance += Mathf.Abs(MathFunctions.StandardDeviation(30f, 15f, planetHostingStar.Id * planet.Id * 500));
                planet.Moons = GenerateMoons(planet, star);
                planetHostingStar.Planets.Add(planet);
            }
             if (slot == SystemSlot.AsteroidBelt)
            {
                AsteroidBelt belt = new(1, orbitDistance);
                orbitDistance += 33;

                planetHostingStar.AsteroidBelts.Add(belt);
            }
             if (slot == SystemSlot.EmptySpace)
            {

                orbitDistance += 50;

            }
        }*/


        
        foreach (SystemSlot slot in orbitSlots)
        {
            if (slot == SystemSlot.Planet)
            {
                Planet planet = new(orbitDistance, planetHostingStar.SolarTemperature);
                planet.Name = nameList.getPlanetName(planetHostingStar.Name, planetOrderIndex);
                planetOrderIndex++;
                orbitDistance += Mathf.Abs(MathFunctions.StandardDeviation(30f, 15f, planetHostingStar.Id * planet.Id * 500));
                planet.Moons = GenerateMoons(planet, star);
                planetHostingStar.Planets.Add(planet);
            }
            if (slot == SystemSlot.AsteroidBelt)
            {
                int beltType = UnityEngine.Random.Range(1, 3);
                AsteroidBelt belt = new(beltType, orbitDistance);
                planetHostingStar.AsteroidBelts.Add(belt);
                orbitDistance += 33;
            }
            if (slot == SystemSlot.EmptySpace)
            {

                orbitDistance += 5;

            }

        }


        return planetHostingStar;

    }


    void GeneratePlanets(Star star, int numberOfPlanets)
    {
         float orbitDistance = 30f + MathFunctions.StandardDeviation(40f, 15f, star.Id);

        for (int planetIndex = 0; planetIndex < numberOfPlanets; planetIndex++)
        {

            Planet planet = new(orbitDistance, star.SolarTemperature);
            planet.Name = nameList.getPlanetName(star.Name, planetIndex);
            orbitDistance += Mathf.Abs(MathFunctions.StandardDeviation(30f, 15f, star.Id * planet.Id * 500));
            planet.Moons = GenerateMoons(planet, star);
            star.Planets.Add(planet);

        }
    }

    public List<Moon> GenerateMoons(Planet planet, Star star)
    {
        List<Moon> moons = new();
        float orbitDistance = 12f;
        int ringType = planet.RingType;
        int moonNumber = 0;

        if (UnityEngine.Random.Range(1, 11) <= 6)  //80% planets have no moon
        {
            moonNumber = 0;
        }
        else
        {
            if (ringType == 1) { moonNumber = UnityEngine.Random.Range(1, 3); }
            else if (ringType == 2) { moonNumber = 1; }
            else if (ringType == 3) { moonNumber = UnityEngine.Random.Range(1, 2); }
            else { moonNumber = UnityEngine.Random.Range(1, 4); }
        }

        if (ringType == 1) { orbitDistance += 1f; }
        else if (ringType == 2) { orbitDistance += 4f; }
        else if (ringType == 3) { orbitDistance += 5f; }
        else if (ringType == 4) { orbitDistance += 2.3f; }

        float parentsOrbitalDistance = planet.OrbitDistance;
        float solarTemperature = star.SolarTemperature;

        for (int i = 0; i < moonNumber; i++)
        {
            Moon moon = new(parentsOrbitalDistance, solarTemperature);
            moon.Pos = MathFunctions.RandomPositionOnCircle(orbitDistance, planet.Pos);
            moon.OrbitInclination = planet.OrbitInclination;
            moon.OrbitOmega = planet.OrbitOmega;
            moon.Mass = UnityEngine.Random.Range(0.10f, 0.25f); ;
            orbitDistance += 4;
            moons.Add(moon);
        }

        return moons;
    }

    public void GenerateBelts(Star star, int numberOfBelts, int type)
    {
        for (int beltIndex = 0; beltIndex < numberOfBelts; beltIndex++)
        {

            AsteroidBelt belt = new();
            float beltDistance = (beltIndex + 1) * UnityEngine.Random.Range(45f, 185f);
            belt.Distance = beltDistance;
            belt.Type = type;
            star.AsteroidBelts.Add(belt);
        }
    }

    public Cluster LocalBubbleCluster(int clusterId)
    {
           

        Cluster LocalBubbleCluster = new(clusterId);
        LocalBubbleCluster.Name = "Local Bubble";

        float bubbleScale = 10;

        Hydrosphere hydrosphereSources = new();

        Vector3 solPosition = new(0, 30, 0);


        //Firstly creates Sol to the local bubble
        Star Sol = new(clusterId);
        Sol.Name = "Sol";
        Sol.Pos = solPosition;
        Sol.Type = starFormation.GetSpecificTypeStar("G-type");
        Sol.SolarTemperature = 5778;



        AsteroidBelt Belt = new();
        Belt.Distance = 150;
        Belt.Type = 2;
        Sol.AsteroidBelts.Add(Belt);

        //Then creates planets of the solar system
        Planet Mercury = new();
        Mercury.OrbitDistance = 42;
        Mercury.Name = "Mercury";
        Mercury.Pos = MathFunctions.RandomPositionOnCircle(Mercury.OrbitDistance, new Vector3(0, 0, 0));
        Mercury.OrbitSpeed = 0.018f;
        Mercury.RotationSpeed = 0.08f;
        Mercury.Mass = 0.5f;
        Mercury.Type = new PlanetType("Metallic");
        Mercury.Atm.PrimaryGas = AtmosphereGasFormation.NitrogenMethane;
        Mercury.RingType = 0;
        Mercury.Atm.TemperatureB = MathFunctions.GetEffectiveTemperature(Mercury.OrbitDistance, Mercury.Type.Albedo, Sol.SolarTemperature);
        Mercury.Atm.Pressure = 0;
        Mercury.Atm.Radiation = 30.1f;
        Mercury.Hydrosphere = new Hydrosphere(1, Mercury.Atm, false);

        Sol.Planets.Add(Mercury);


        Planet Venus = new();
        Venus.OrbitDistance = 60;
        Venus.Name = "Venus";
        Venus.Pos = MathFunctions.RandomPositionOnCircle(Venus.OrbitDistance, new Vector3(0, 0, 0));
        Venus.OrbitSpeed = 0.006f;
        Venus.RotationSpeed = 0.02f;
        Venus.Type = new PlanetType("Greenhouse");
        Venus.Type.CustomSubType = "VenusPlanet";
        Venus.Mass = 0.9f;
        Venus.RingType = 0;
        Venus.Atm.TemperatureB = MathFunctions.GetEffectiveTemperature(Venus.OrbitDistance, 0.75f, Sol.SolarTemperature);
        Venus.Atm.TemperatureG = 421;
        Venus.Atm.Pressure = 93.2f;
        Venus.Atm.Radiation = 0.78f;
        Venus.Atm.PrimaryGas = AtmosphereGasFormation.CarbonDioxideNitrogen;
        Venus.Hydrosphere = new Hydrosphere(1, Venus.Atm, false);
        Sol.Planets.Add(Venus);

        Planet Earth = new();
        Earth.OrbitDistance = 80;
        Earth.Name = "Earth";
        Earth.Pos = MathFunctions.RandomPositionOnCircle(Earth.OrbitDistance, new Vector3(0, 0, 0));
        Earth.OrbitSpeed = 0.003f;
        Earth.RotationSpeed = 4;
        Earth.Type = new PlanetType("Water");
        Earth.Type.SurfaceType = SurfaceType.Silicate;
        Earth.Mass = 1f;
        Earth.RingType = 0;
        Earth.PolarCoverage = 0f;
        Earth.Atm.PrimaryGas = AtmosphereGasFormation.NitrogenOxygen;
        Earth.Atm.Pressure = 1; ;
        Earth.Atm.TemperatureB = MathFunctions.GetEffectiveTemperature(Earth.OrbitDistance, 0.3f, Sol.SolarTemperature);
        Earth.Atm.TemperatureG = 33;
        Earth.Atm.Radiation = 0.0024f;
        Earth.Biosphere = new Biosphere(2);
        Earth.Hydrosphere = new Hydrosphere(2, Earth.Atm, true);
        Earth.Society = new Society(33);

        Moon Luna = new();
        Luna.Pos = MathFunctions.RandomPositionOnCircle(15, Earth.Pos);
        Earth.Moons.Add(Luna);
        Luna.OrbitSpeed = Earth.OrbitSpeed * 13.7f;
        Sol.Planets.Add(Earth);

        Planet Mars = new();
        Mars.OrbitDistance = 105;
        Mars.Name = "Mars";
        Mars.Pos = MathFunctions.RandomPositionOnCircle(Mars.OrbitDistance, new Vector3(0, 0, 0));
        Mars.OrbitSpeed = 0.002f;
        Mars.RotationSpeed = 4;
        Mars.Type = new PlanetType("Dust");
        Mars.Type.CustomSubType = "MarsPlanet";
        Mars.Mass = 0.7f;
        Mars.PolarCoverage = 0.15f;
        Mars.RingType = 0;
        Mars.Atm.TemperatureB = MathFunctions.GetEffectiveTemperature(Mars.OrbitDistance, Mars.Type.Albedo, Sol.SolarTemperature);
        Mars.Atm.PrimaryGas = AtmosphereGasFormation.CarbonDioxideNitrogen;
        Mars.Atm.Pressure = 0.00658f;
        Mars.Atm.Radiation = 0.233f;
        Mars.Hydrosphere = new Hydrosphere(1, Mars.Atm, true);
        Sol.Planets.Add(Mars);

        Planet Jupiter = new();
        Jupiter.OrbitDistance = 180;
        Jupiter.Name = "Jupiter";
        Jupiter.Pos = MathFunctions.RandomPositionOnCircle(Jupiter.OrbitDistance, new Vector3(0, 0, 0));
        Jupiter.OrbitSpeed = 0.0003f;
        Jupiter.RotationSpeed = 2;
        Jupiter.Type = new PlanetType("Gas");
        Jupiter.Type.CustomSubType = "JupiterPlanet";
        Jupiter.Mass = 1.7f;
        Jupiter.RingType = 0;
        Jupiter.Seed = 1231;
        Jupiter.Atm.PrimaryGas = AtmosphereGasFormation.HydrogenHelium;
        Jupiter.Atm.TemperatureB = MathFunctions.GetEffectiveTemperature(Jupiter.OrbitDistance, Jupiter.Type.Albedo, Sol.SolarTemperature);
        Jupiter.Atm.Pressure = 5000;
        Jupiter.Atm.Radiation = 5000;
        Jupiter.Hydrosphere = new Hydrosphere(0, Jupiter.Atm, false);
        Sol.Planets.Add(Jupiter);

        Planet Saturn = new();
        Saturn.OrbitDistance = 230;
        Saturn.Name = "Saturn";
        Saturn.Pos = MathFunctions.RandomPositionOnCircle(Saturn.OrbitDistance, new Vector3(0, 0, 0));
        Saturn.OrbitSpeed = 0.0001f;
        Saturn.RotationSpeed = 2;
        Saturn.Type = new PlanetType("Gas");
        Saturn.Type.CustomSubType = "SaturnPlanet";
        Saturn.Mass = 1.7f;
        Saturn.RingType = 3;
        Saturn.Atm.PrimaryGas = AtmosphereGasFormation.HydrogenHelium;
        Saturn.Atm.TemperatureB = MathFunctions.GetEffectiveTemperature(Saturn.OrbitDistance, Saturn.Type.Albedo, Sol.SolarTemperature);
        Saturn.Atm.Pressure = 5000;
        Saturn.Atm.Radiation = 5000;
        Saturn.Hydrosphere = new Hydrosphere(0, Saturn.Atm, false);
        Sol.Planets.Add(Saturn);

        Planet Uranus = new();
        Uranus.OrbitDistance = 260;
        Uranus.Name = "Uranus";
        Uranus.Pos = MathFunctions.RandomPositionOnCircle(Uranus.OrbitDistance, new Vector3(0, 0, 0));
        Uranus.OrbitSpeed = 0.0005f;
        Uranus.RotationSpeed = 2;
        Uranus.Type = new PlanetType("Gas");
        Uranus.Type.CustomSubType = "UranusPlanet";
        Uranus.Mass = 1.5f;
        Uranus.RingType = 0;
        Uranus.Atm.PrimaryGas = AtmosphereGasFormation.HydrogenHelium;
        Uranus.Atm.TemperatureB = MathFunctions.GetEffectiveTemperature(Uranus.OrbitDistance, Uranus.Type.Albedo, Sol.SolarTemperature);
        Uranus.Atm.Pressure = 1000;
        Uranus.Hydrosphere = new Hydrosphere(0, Uranus.Atm, false);
        Uranus.Atm.Radiation = 3;
        Sol.Planets.Add(Uranus);

        Planet Neptune = new();
        Neptune.OrbitDistance = 285;
        Neptune.Name = "Neptune";
        Neptune.Pos = MathFunctions.RandomPositionOnCircle(Neptune.OrbitDistance, new Vector3(0, 0, 0));
        Neptune.OrbitSpeed = 0.0003f;
        Neptune.RotationSpeed = 2;
        Neptune.Type = new PlanetType("Gas");
        Neptune.Type.CustomSubType = "NeptunePlanet";
        Neptune.Mass = 1.6f;
        Neptune.RingType = 0;
        Neptune.Atm.PrimaryGas = AtmosphereGasFormation.HydrogenHelium;
        Neptune.Atm.TemperatureB = MathFunctions.GetEffectiveTemperature(Neptune.OrbitDistance, Neptune.Type.Albedo, Sol.SolarTemperature);
        Neptune.Hydrosphere = new Hydrosphere(0, Neptune.Atm, false);
        Neptune.Atm.Pressure = 1000;
        Neptune.Atm.Radiation = 3.43f;
        Sol.Planets.Add(Neptune);

        LocalBubbleCluster.Stars.Add(Sol);





        //Alpha Centauri
        Star alphaCentauri = new(clusterId);
        alphaCentauri.Name = "Alpha Centauri";
        alphaCentauri.Pos = solPosition + bubbleScale * new Vector3(3.126f, -0.052f, -3.047f);
        alphaCentauri.Type = starFormation.GetSpecificTypeStar("G-type");
        alphaCentauri.SolarTemperature = 5790;
        GeneratePlanets(alphaCentauri, 2);

        AsteroidBelt alphaCentauriBeltOne = new();
        alphaCentauriBeltOne.Distance = 45;
        alphaCentauriBeltOne.Type = 1;
        alphaCentauri.AsteroidBelts.Add(alphaCentauriBeltOne);
        LocalBubbleCluster.Stars.Add(alphaCentauri);

        //Proxima Centauri
        Star proximaCentauri = new(clusterId);
        proximaCentauri.Name = "Proxima Centauri";
        proximaCentauri.Pos = solPosition + bubbleScale * new Vector3(2.545f, -0.243f, -3.256f);
        proximaCentauri.Type = starFormation.GetSpecificTypeStar("M-type");
        proximaCentauri.SolarTemperature = 3420;
        LocalBubbleCluster.Stars.Add(proximaCentauri);

        Planet proximaCentauriF = new();
        proximaCentauriF.OrbitDistance = 55;
        proximaCentauriF.Name = "Proxima Centauri F";
        proximaCentauriF.Pos = MathFunctions.RandomPositionOnCircle(proximaCentauriF.OrbitDistance, new Vector3(0, 0, 0));
        proximaCentauriF.OrbitSpeed = 0;
        proximaCentauriF.RotationSpeed = 1;
        proximaCentauriF.Type = new PlanetType("Slime");
        proximaCentauriF.Mass = 1.1f;
        proximaCentauriF.RingType = 0;
        proximaCentauriF.Atm.PrimaryGas = AtmosphereGasFormation.HydrogenHelium;
        proximaCentauriF.Atm.TemperatureB = MathFunctions.GetEffectiveTemperature(proximaCentauriF.OrbitDistance, proximaCentauriF.Type.Albedo, proximaCentauri.SolarTemperature);
        proximaCentauriF.Atm.Pressure = 7;
        proximaCentauriF.Atm.Radiation = 0.13f;
        proximaCentauri.Planets.Add(proximaCentauriF);

        LocalBubbleCluster.Stars.Add(Sol);

        //Wolf 359
        Star Wolf359 = new(clusterId);
        Wolf359.Name = "Wolf 359";
        Wolf359.Pos = solPosition + bubbleScale * new Vector3(-1.916f, 6.522f, -3.938f);
        Wolf359.Type = starFormation.GetSpecificTypeStar("M-type");
        Wolf359.SolarTemperature = 2749;
        GeneratePlanets(Wolf359, 3);
        LocalBubbleCluster.Stars.Add(Wolf359);

        //Lalande 21185
        Star Lalande21185 = new(clusterId);
        Lalande21185.Name = "Lalande 21185";
        Lalande21185.Pos = solPosition + bubbleScale * new Vector3(-3.439f, 7.553f, -0.308f);
        Lalande21185.Type = starFormation.GetSpecificTypeStar("M-type");
        Lalande21185.SolarTemperature = 3546;

        Planet hyceanA = new();
        hyceanA.OrbitDistance = 55;
        hyceanA.Name = "Hattusili";
        hyceanA.Pos = MathFunctions.RandomPositionOnCircle(proximaCentauriF.OrbitDistance, new Vector3(0, 0, 0));
        hyceanA.OrbitSpeed = 0;
        hyceanA.RotationSpeed = 1;
        hyceanA.Type = new PlanetType("Hycean");
        hyceanA.Mass = 1.4f;
        hyceanA.RingType = 0;
        hyceanA.Atm.PrimaryGas = AtmosphereGasFormation.HydrogenHelium;
        hyceanA.Atm.TemperatureB = 46;
        hyceanA.Atm.Pressure = 26;
        hyceanA.Atm.Radiation = 0.03f;
        Lalande21185.Planets.Add(hyceanA);


        LocalBubbleCluster.Stars.Add(Lalande21185);

        //Sirius A
        Star SiriusA = new(clusterId);
        SiriusA.Name = "Sirius A";
        SiriusA.Pos = solPosition + bubbleScale * new Vector3(-5.809f, -1.338f, -6.28f);
        SiriusA.Type = starFormation.GetSpecificTypeStar("A-type");
        SiriusA.SolarTemperature = 9950;
        LocalBubbleCluster.Stars.Add(SiriusA);

        //Epsilon Eridani
        Star EpsilonEridani = new(clusterId);
        EpsilonEridani.Name = "Epsilon Eridani";
        EpsilonEridani.Pos = solPosition + bubbleScale * new Vector3(-6.753f, -7.811f, -1.917f);
        EpsilonEridani.Type = starFormation.GetSpecificTypeStar("K-type");
        EpsilonEridani.SolarTemperature = 5230;
        LocalBubbleCluster.Stars.Add(EpsilonEridani);

        //Procyon A
        Star ProcyonA = new(clusterId);
        ProcyonA.Name = "Epsilon Eridani";
        ProcyonA.Pos = solPosition + bubbleScale * new Vector3(-9.27f, 2.577f, -6.183f);
        ProcyonA.Type = starFormation.GetSpecificTypeStar("F-type");
        ProcyonA.SolarTemperature = 6530;
        LocalBubbleCluster.Stars.Add(ProcyonA);

        return LocalBubbleCluster;
    }
}
