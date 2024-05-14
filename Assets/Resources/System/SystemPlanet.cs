using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LineSpace;
using MathSpace;

using static CelestialBody;
public class SystemPlanet : MonoBehaviour
{

    public Planet Planet;

    public GameObject PlanetaryFeatureLocation;

    List<GameObject> FeatureLocations = new List<GameObject>();
    [SerializeField] Vector3 Pos;
    [SerializeField] float Omega; //longitude of the ascending node
    [SerializeField] float Inclination;

    public GameObject RingPrefab;

    public Material LineMaterial;
    

    System.Random Random = new System.Random();
    MathFunctions MathFunctions = new MathFunctions();


    void Start()
    {
        int seed = Planet.Seed;
        Random = new System.Random(Planet.Seed);



    }


    public void Visualize()
    {
        GameObject PlanetSurfaceObject;
        
        PlanetSurfaceObject = CreatePlanetSurface();
        CreatePlanetRing(PlanetSurfaceObject);
        CreatePlanetOrbitCircle();
        InclinatePlanet();
    }

    void CreatePlanetOrbitCircle()
    {
        LineFunctions.CreateOrbitCircle(this.transform, transform.parent.position, LineMaterial);

    }

    void InclinatePlanet()
    {
        transform.RotateAround(transform.parent.position, new Vector3(1, 0, 0), Planet.OrbitInclination);
        transform.RotateAround(transform.parent.position, new Vector3(0, 1, 0), Planet.OrbitOmega);
    }



    GameObject CreatePlanetSurface()
    {

        Random = new System.Random(Planet.Seed);

        string planetTypename = Planet.Type.Name;
        string customSubType = Planet.Type.CustomSubType;
        string planetFolderPath = "";

        string planetPath = "System/Planets/" + planetTypename + "Planet/" + planetTypename + "Planet";
        if (customSubType == "")
        {
            planetFolderPath = "System/Planets/" + planetTypename + "Planet/";
        } else
        {
            planetFolderPath = "System/Planets/" + "CustomPlanet/" + customSubType;
        }


        GameObject[] AvailableSelection = Resources.LoadAll<GameObject>(planetFolderPath) as GameObject[]; // Selects all 

        int randomObject = Random.Next(0, AvailableSelection.Length);


        GameObject PlanetVisualPrefab = AvailableSelection[randomObject];

        if (PlanetVisualPrefab == null)
        {
            Debug.Log("PLANET:" + planetPath + " NOT FOUND!");
            PlanetVisualPrefab = Resources.Load<GameObject>("System/Planets/MinorPlanet/MinorPlanet") as GameObject;
        }

        GameObject PlanetSurfaceObject = Instantiate(PlanetVisualPrefab, this.transform, false) as GameObject;
        PlanetSurfaceObject.GetComponent<PlanetSurface>().Planet = Planet;
        PlanetSurfaceObject.GetComponent<PlanetSurface>().SetValues();
        PlanetSurfaceObject.GetComponent<PlanetSurface>().CopyVertices();
        PlanetSurfaceObject.GetComponent<PlanetSurface>().ShapePlanetSurface();

        return PlanetSurfaceObject;
    }


    void CreatePlanetRing(GameObject surface)
    {
        GameObject PlanetRing = Instantiate(RingPrefab, surface.transform, false) as GameObject;
        PlanetRing.GetComponent<PlanetRing>().CreateRing(Planet.RingType);
    }



    void OnMouseDown()
    {
        UiCanvas.GetInstance().PlanetDataView(transform, Planet);
        Globals.spaceAddress[2] = Planet.Id;

    }

    float GetPlanetOrbitalDistance(Vector3 pos, Vector3 parentPos)
    {
        float distance;

        distance = Vector3.Distance(pos, parentPos);
        return distance;
    }


    void Update()
    {
        transform.RotateAround(transform.parent.position, transform.up, Planet.OrbitSpeed * Time.deltaTime);
    }



}
