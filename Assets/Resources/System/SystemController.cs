using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static CelestialBody;

public class SystemController : GalaxyCatalog
{
    public GameObject MoonSystemPrefab;
    public GameObject PlanetSystemPrefab;
    public GameObject BeltPrefab;
    public GameObject SystemStarPrefab;

    Star selectedVisibleSystem;

    GameObject PlanetSystem;
    GameObject PlanetVisual;
    GameObject StarVisual;



    private static SystemController _instance;
    private static SystemController Instance
    {
        get
        {
            if (_instance == null)
            {
                // If the instance is null, try to find it in the scene
                _instance = FindObjectOfType<SystemController>();

                if (_instance == null)
                {
                    Debug.LogError("SystemController NULL, CANT BE FOUND");
                }
            }

            return _instance;
        }
    }
    public static SystemController GetInstance()
    {
        return Instance;
    }

    void OnDisable()
    {
 

        foreach (Transform child in gameObject.transform)
        {
            if (child.gameObject.tag == "System")
            {
                Destroy(child.gameObject);
            }
        }
    }


    //only for currently excisting gameObjects 
    public GameObject FindPlanetGameObject(int planetId)
    {
        GameObject planet = null;
        foreach (Transform child in gameObject.transform)
        {
            if (child.GetComponent<SystemPlanet>())
            {
                int id = child.GetComponent<SystemPlanet>().Planet.Id;

                if (id == planetId)
                {
                    planet = child.gameObject;
                    break;
                }

            }
        }

        return planet;
    }

    public void VisualizeSystem(Star visibleSystem)
    {

        selectedVisibleSystem = visibleSystem;

        //Creates a main star for the system
        GameObject StarSystem = Instantiate(SystemStarPrefab, new Vector3(0f, 0f, 0f), transform.rotation) as GameObject;
        StarSystem.GetComponent<SystemStar>().Star = selectedVisibleSystem;
        /* string starPath = "System/Star/MainSequenceStar";
         GameObject StarVisualPrefab = Resources.Load<GameObject>(starPath) as GameObject;

         if (StarVisualPrefab == null)
         {
             Debug.Log("STAR:" + starPath + " NOT FOUND!");
             StarVisualPrefab = Resources.Load<GameObject>("System/Planets/MinorPlanet/MinorPlanet") as GameObject;
         }

         StarVisual = Instantiate(StarVisualPrefab, StarSystem.transform, false) as GameObject;
         */

        StarSystem.GetComponent<SystemStar>().Visualize();
        StarSystem.transform.parent = this.transform;


        //Creates asteroid belts for the system
        foreach (AsteroidBelt asteroidBelt in visibleSystem.AsteroidBelts)
        {

            if (BeltPrefab != null)
            {
                GameObject BeltInstance = Instantiate(BeltPrefab, new Vector3(0f, 0f, 0f), transform.rotation) as GameObject;

                BeltInstance.GetComponent<SystemAsteroidBelt>().Type = asteroidBelt.Type;
                BeltInstance.GetComponent<SystemAsteroidBelt>().Distance = asteroidBelt.Distance;
                BeltInstance.transform.parent = this.transform;
            }
            else
            {
                Debug.Log("null BeltPrefab!");
            }
        }



        //Creates planets for the system
        foreach (Planet planet in visibleSystem.Planets)
        {
            PlanetSystem = Instantiate(PlanetSystemPrefab, planet.Pos, transform.rotation) as GameObject;
            PlanetSystem.GetComponent<SystemPlanet>().Planet = planet;
            PlanetSystem.transform.localScale = new Vector3(planet.Mass, planet.Mass, planet.Mass);
            PlanetSystem.transform.parent = this.transform;

            foreach (Moon moon in planet.Moons)
            {
                GameObject MoonSystem = Instantiate(MoonSystemPrefab, moon.Pos, PlanetSystem.transform.rotation) as GameObject;
                MoonSystem.GetComponent<SystemPlanet>().Planet = moon;
                MoonSystem.transform.localScale = new Vector3(moon.Mass, moon.Mass, moon.Mass);
                MoonSystem.transform.parent = PlanetSystem.transform;
                MoonSystem.GetComponent<SystemPlanet>().Visualize();
            }

            PlanetSystem.GetComponent<SystemPlanet>().Visualize();
        }


        UI.StarDataView(StarSystem.transform, visibleSystem);

    }



}
