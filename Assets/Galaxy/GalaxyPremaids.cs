using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MathSpace;

using static CelestialBody;




public class Premaid
{
    StarFormation starFormation = new StarFormation();
    MathFunctions MathFunctions = new MathFunctions();

    Names nameList = new Names();





    int TotalPlanetCount = 0;
    int TotalStarCount = 0;




     int SetPlanetId()
    {
        int id = TotalPlanetCount;
        TotalPlanetCount++;
        return id;
    }

    int SetStarId()
    {
        int id = TotalStarCount;
        TotalStarCount++;
        return id;
    }

    public Cluster GenericCluster(int id)
    {
        Cluster GenericCluster = new Cluster(id);

        GenericCluster.Name = nameList.getRandomZoneName();

        int numOfStars = ConfigManager.Instance.gameConfig.AvgStarNumPeCluster;

        for (int starIndex = 0; starIndex < numOfStars; starIndex++)
        {
                
            Star Star = new Star();

            Star.Id = SetStarId();
            Star.Name = nameList.getRandomStarName();
            Star.Type = starFormation.GetRandomTypeStar();
            Star.SolarTemperature = starFormation.getStarTypeRandomTemperature(Star.Type);

            int systemPlanetNum = ConfigManager.Instance.gameConfig.AvgPlanetNumPerStar; ;

            int beltType = Random.Range(0, 3);
            int numOfBelts = Random.Range(0, 3);

            GenerateBelts(Star, numOfBelts, beltType);
            GeneratePlanets(Star, systemPlanetNum);

            GenericCluster.Stars.Add(Star);
        }

        return GenericCluster;
    }

    void GeneratePlanets(Star star, int numberOfPlanets)
    {
        float orbitDistance = 10f + MathFunctions.StandardDeviation(40f, 15f, star.Id);

        for (int planetIndex = 0; planetIndex < numberOfPlanets; planetIndex++)
        {

            Planet planet = new Planet(orbitDistance, star.SolarTemperature);
            planet.Id = SetPlanetId();
            planet.Name = nameList.getPlanetName(star.Name, planetIndex);
            orbitDistance += Mathf.Abs(MathFunctions.StandardDeviation(30f, 15f, star.Id * planet.Id * 500));
            planet.Moons = GenerateMoons(planet, star);
            star.Planets.Add(planet);

        }
    }

    public List<Moon> GenerateMoons(Planet planet, Star star)
    {
        List<Moon> moons = new List<Moon>();
        float orbitDistance = 12f;
        int ringType = planet.RingType;
        int moonNumber = 0;

        if (Random.Range(1, 11) <= 6)  //80% planets have no moon
        {
            moonNumber = 0;
        }
        else
        {
            if (ringType == 1) { moonNumber = Random.Range(1, 3); }
            else if (ringType == 2) { moonNumber = 1; }
            else if (ringType == 3) { moonNumber = Random.Range(1, 2); }
            else { moonNumber = Random.Range(1, 4); }
        }

        if (ringType == 1) { orbitDistance += 1f; }
        else if (ringType == 2) { orbitDistance += 4f; }
        else if (ringType == 3) { orbitDistance += 5f; }
        else if (ringType == 4) { orbitDistance += 2.3f; }

        float parentsOrbitalDistance = planet.OrbitDistance;
        float solarTemperature = star.SolarTemperature;

        for (int i = 0; i < moonNumber; i++)
        {
            Moon moon = new Moon(parentsOrbitalDistance, solarTemperature);
            moon.Pos = MathFunctions.RandomPositionOnCircle(orbitDistance, planet.Pos);
            moon.OrbitInclination = planet.OrbitInclination;
            moon.OrbitOmega = planet.OrbitOmega;
            moon.Mass = Random.Range(0.10f, 0.25f); ;
            orbitDistance += 4;
            moons.Add(moon);
        }

        return moons;
    }

    public void GenerateBelts(Star star, int numberOfBelts, int type)
    {
        for (int beltIndex = 0; beltIndex < numberOfBelts; beltIndex++)
        {

            AsteroidBelt belt = new AsteroidBelt();
            float beltDistance = (beltIndex + 1) * Random.Range(45f, 185f);
            belt.Distance = beltDistance;
            belt.Type = type;
            star.AsteroidBelts.Add(belt);
        }
    }

    public Cluster LocalBubbleCluster(int id)
    {
           

        Cluster LocalBubbleCluster = new Cluster(id);
        LocalBubbleCluster.Name = "Local Bubble";

        float bubbleScale = 10;

        Hydrosphere hydrosphereSources = new Hydrosphere();

        Vector3 solPosition = new Vector3(0, 30, 0);


        //Firstly creates Sol to the local bubble
        Star Sol = new Star();
        Sol.Id = SetStarId();
        Sol.Name = "Sol";
        Sol.Pos = solPosition;
        Sol.Type = starFormation.GetSpecificTypeStar("G-type"); 
        Sol.SolarTemperature = 5778;



        AsteroidBelt Belt = new AsteroidBelt();
        Belt.Distance = 150;
        Belt.Type = 2;
        Sol.AsteroidBelts.Add(Belt);

        //Then creates planets of the solar system
        Planet Mercury = new Planet();
        Mercury.OrbitDistance = 42;
        Mercury.Name = "Mercury";
        Mercury.Id = SetPlanetId(); 
        Mercury.Pos = MathFunctions.RandomPositionOnCircle(Mercury.OrbitDistance, new Vector3(0, 0, 0));
        Mercury.OrbitSpeed = 0;
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


        Planet Venus = new Planet();
        Venus.OrbitDistance = 60;
        Venus.Name = "Venus";
        Venus.Id = SetPlanetId();
        Venus.Pos = MathFunctions.RandomPositionOnCircle(Venus.OrbitDistance, new Vector3(0, 0, 0));
        Venus.OrbitSpeed = 0;
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

        Planet Earth = new Planet();
        Earth.OrbitDistance = 80;
        Earth.Name = "Earth";
        Earth.Id = SetPlanetId();
        Earth.Pos = MathFunctions.RandomPositionOnCircle(Earth.OrbitDistance, new Vector3(0, 0, 0));
        Earth.OrbitSpeed = 0;
        Earth.RotationSpeed = 4;
        Earth.Type = new PlanetType("Water");
        Earth.Type.SurfaceType = SurfaceType.Silicate;
        Earth.Type.CustomSubType = "EarthPlanet";
        Earth.Mass = 1f;
        Earth.RingType = 0;
        Earth.PolarCoverage = 0f;
        Earth.Atm.PrimaryGas = AtmosphereGasFormation.NitrogenOxygen;
        Earth.Atm.Pressure = 1;;
        Earth.Atm.TemperatureB = MathFunctions.GetEffectiveTemperature(Earth.OrbitDistance, 0.3f, Sol.SolarTemperature);
        Earth.Atm.TemperatureG = 33;
        Earth.Atm.Radiation = 0.0024f;
        Earth.Biosphere = new Biosphere(2);
        Earth.Hydrosphere = new Hydrosphere(2, Earth.Atm, true);
        Earth.Society = new Society(33);

        Moon Luna = new Moon();
        Luna.Pos = MathFunctions.RandomPositionOnCircle(15, Earth.Pos);
        Earth.Moons.Add(Luna);
        Sol.Planets.Add(Earth);

        Planet Mars = new Planet();
        Mars.OrbitDistance = 105;
        Mars.Name = "Mars";
        Mars.Id = SetPlanetId();
        Mars.Pos = MathFunctions.RandomPositionOnCircle(Mars.OrbitDistance, new Vector3(0, 0, 0));
        Mars.OrbitSpeed = 0;
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

        Planet Jupiter = new Planet();
        Jupiter.OrbitDistance = 180;
        Jupiter.Name = "Jupiter";
        Jupiter.Id = SetPlanetId();
        Jupiter.Pos = MathFunctions.RandomPositionOnCircle(Jupiter.OrbitDistance, new Vector3(0, 0, 0));
        Jupiter.OrbitSpeed = 0;
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

        Planet Saturn = new Planet();
        Saturn.OrbitDistance = 230;
        Saturn.Id = SetPlanetId();
        Saturn.Name = "Saturn";
        Saturn.Pos = MathFunctions.RandomPositionOnCircle(Saturn.OrbitDistance, new Vector3(0, 0, 0));
        Saturn.OrbitSpeed = 0;
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

        Planet Uranus = new Planet();
        Uranus.OrbitDistance = 260;
        Uranus.Id = SetPlanetId();
        Uranus.Name = "Uranus";
        Uranus.Pos = MathFunctions.RandomPositionOnCircle(Uranus.OrbitDistance, new Vector3(0, 0, 0));
        Uranus.OrbitSpeed = 0;
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

        Planet Neptune = new Planet();
        Neptune.OrbitDistance = 285;
        Neptune.Id = SetPlanetId();
        Neptune.Name = "Neptune";
        Neptune.Pos = MathFunctions.RandomPositionOnCircle(Neptune.OrbitDistance, new Vector3(0, 0, 0));
        Neptune.OrbitSpeed = 0;
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
        Star alphaCentauri = new Star();
        alphaCentauri.Id = SetStarId();
        alphaCentauri.Name = "Alpha Centauri";
        alphaCentauri.Pos = solPosition + bubbleScale * new Vector3(3.126f, -0.052f, -3.047f);
        alphaCentauri.Type = starFormation.GetSpecificTypeStar("G-type");
        alphaCentauri.SolarTemperature = 5790;
        GeneratePlanets(alphaCentauri, 2);

        AsteroidBelt alphaCentauriBeltOne = new AsteroidBelt();
        alphaCentauriBeltOne.Distance = 45;
        alphaCentauriBeltOne.Type = 1;
        alphaCentauri.AsteroidBelts.Add(alphaCentauriBeltOne);
        LocalBubbleCluster.Stars.Add(alphaCentauri);

        //Proxima Centauri
        Star proximaCentauri = new Star();
        proximaCentauri.Id = SetStarId();
        proximaCentauri.Name = "Proxima Centauri";
        proximaCentauri.Pos = solPosition + bubbleScale * new Vector3(2.545f, -0.243f, -3.256f);
        proximaCentauri.Type = starFormation.GetSpecificTypeStar("M-type");
        proximaCentauri.SolarTemperature = 3420;
        LocalBubbleCluster.Stars.Add(proximaCentauri);

        Planet proximaCentauriF = new Planet();
        proximaCentauriF.OrbitDistance = 55;
        proximaCentauriF.Id = SetPlanetId();
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
        Star Wolf359 = new Star();
        Wolf359.Id = SetStarId();
        Wolf359.Name = "Wolf 359";
        Wolf359.Pos = solPosition + bubbleScale * new Vector3(-1.916f, 6.522f, -3.938f);
        Wolf359.Type = starFormation.GetSpecificTypeStar("M-type");
        Wolf359.SolarTemperature = 2749;
        GeneratePlanets(Wolf359, 3);
        LocalBubbleCluster.Stars.Add(Wolf359);

        //Lalande 21185
        Star Lalande21185 = new Star();
        Lalande21185.Id = SetStarId();
        Lalande21185.Name = "Lalande 21185";
        Lalande21185.Pos = solPosition + bubbleScale * new Vector3(-3.439f, 7.553f, -0.308f);
        Lalande21185.Type = starFormation.GetSpecificTypeStar("M-type");
        Lalande21185.SolarTemperature = 3546;

        Planet hyceanA = new Planet();
        hyceanA.OrbitDistance = 55;
        hyceanA.Id = SetPlanetId();
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
        Star SiriusA = new Star();
        SiriusA.Id = SetStarId();
        SiriusA.Name = "Sirius A";
        SiriusA.Pos = solPosition + bubbleScale * new Vector3(-5.809f, -1.338f, -6.28f);
        SiriusA.Type = starFormation.GetSpecificTypeStar("A-type");
        SiriusA.SolarTemperature = 9950;
        LocalBubbleCluster.Stars.Add(SiriusA);

        //Epsilon Eridani
        Star EpsilonEridani = new Star();
        EpsilonEridani.Id = SetStarId();
        EpsilonEridani.Name = "Epsilon Eridani";
        EpsilonEridani.Pos = solPosition + bubbleScale * new Vector3(-6.753f, -7.811f, -1.917f);
        EpsilonEridani.Type = starFormation.GetSpecificTypeStar("K-type");
        EpsilonEridani.SolarTemperature = 5230;
        LocalBubbleCluster.Stars.Add(EpsilonEridani);

        //Procyon A
        Star ProcyonA = new Star();
        ProcyonA.Id = SetStarId();
        ProcyonA.Name = "Epsilon Eridani";
        ProcyonA.Pos = solPosition + bubbleScale * new Vector3(-9.27f, 2.577f, -6.183f);
        ProcyonA.Type = starFormation.GetSpecificTypeStar("F-type");
        ProcyonA.SolarTemperature = 6530;
        LocalBubbleCluster.Stars.Add(ProcyonA);

        return LocalBubbleCluster;
    }
}
