using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Game.Math;

using static CelestialBody;
using System.Security.Claims;
using Unity.VisualScripting;
using System.Net;
using System;
using System.Security.Cryptography.X509Certificates;
using UnityEngine.UIElements;
using Game.Lines;
public class SystemPlanet : MonoBehaviour
{

    public Planet Planet;

    public GameObject PlanetaryFeatureLocation;

    public bool selectedPlanet = false;

    List<GameObject> FeatureLocations = new List<GameObject>();
    List<GameObject> InterplanetaryLines = new List<GameObject>();
    List<GameObject> BrachistochroneTrejectoryLines = new List<GameObject>();

    private Vector3 oldPosition;

    [SerializeField] Vector3 Pos;
    [SerializeField] float Omega; //longitude of the ascending node
    [SerializeField] float Inclination;
    [SerializeField] Vector3 OrbitAxis;

    public GameObject RingPrefab;

    public Material LineMaterial;
    Vector3 OrbitVector = new Vector3(0, 0, 0);
    public Vector3 InitialPosition = new Vector3(0, 0, 0);

    float rotations = 0;
    public float rotationspeed = 1;
    System.Random Random = new System.Random();
    MathFunctions MathFunctions = new MathFunctions();


    void Start()
    {
        int seed = Planet.Seed;
        Random = new System.Random(Planet.Seed);
        oldPosition = transform.position;
        InitialPosition = oldPosition;

    }



    public Vector3 GetOrbitVector()
    {
        return OrbitVector;
    }
    public Vector3 GetOrbitAxis()
    {
        return OrbitAxis;
    }
    public float GetOrbitPhase()
    {
        return Planet.OrbitPhase;
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

        GameObject circle = LineManager.Instance.CreateCircleObject(this.transform, "Orbital Circle", transform.parent.position, 360, LineType.Orbital);
        circle.GetComponent<LineRenderer>().loop = true;


    }

    public void ClearPlanetLinesSystemWide()
    {
        List<GameObject> otherPlanets = SystemController.GetInstance().GetPlanetObjects();

        foreach (GameObject planet in otherPlanets)
        {
            planet.GetComponent<SystemPlanet>().ClearPlanetLines();
        }

    }

    public void ClearPlanetLines()
    {
        InterplanetaryLines.Clear();
        BrachistochroneTrejectoryLines.Clear();
        foreach (Transform child in transform)
        {
            if (child.gameObject.name == "Interplanetary Line" || child.gameObject.name == "Interplanetary Line(Clone)")
            {
                Destroy(child.gameObject);
            }
            else if (child.gameObject.name == "Brachistochrone Trejectory" || child.gameObject.name == "Brachistochrone Trejectory(Clone)")
            {
                Destroy(child.gameObject);
            }
        }
    }
    void CreatePlanetDistanceLines()
    {
        ClearPlanetLinesSystemWide();

        List<GameObject> otherPlanets = SystemController.GetInstance().GetPlanetObjects();
        if (selectedPlanet)
            otherPlanets.Remove(this.transform.gameObject);


        int index = 0;
        Vector3[] linePoints = new Vector3[] { new Vector3(0, 0, 0), new Vector3(0, 0, 0) };
        foreach (GameObject child in otherPlanets)
        {

            GameObject interplanetaryLine =  LineManager.Instance.CreateLineObject(this.transform, "Interplanetary Line", linePoints, LineType.Trajectory);

                InterplanetaryLines.Add(interplanetaryLine);

            index++;
        }
        Debug.Log("num: " + index);
    }

    void InclinatePlanet()
    {
        transform.RotateAround(transform.parent.position, new Vector3(1, 0, 0), Planet.OrbitInclination * 360f);
        transform.RotateAround(transform.parent.position, new Vector3(0, 1, 0), Planet.OrbitOmega * 360f);

        OrbitAxis = new Vector3(Planet.OrbitInclination, Planet.OrbitOmega, 0);



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
        StartCoroutine(PlanetSurfaceObject.GetComponent<PlanetSurface>().ShapePlanetSurface());
        return PlanetSurfaceObject;
    }


    void CreatePlanetRing(GameObject surface)
    {
        if (surface == null) return;  // Early exit if surface is null
        GameObject PlanetRing = Instantiate(RingPrefab, surface.transform, false) as GameObject;

        int planetRingType = Planet.RingType;
        Gradient planetColorGradient = new Gradient();

        if (surface.TryGetComponent<PlanetSurface>(out PlanetSurface PlanetSurface))
        {
             planetColorGradient = PlanetSurface.GetColorGradient();
        }

        if (PlanetRing.TryGetComponent<PlanetRing>(out PlanetRing planetRing))
        {
            planetRing.CreateRingWithColor(planetRingType, planetColorGradient);
        }

    }

    


    void OnMouseDown()
    {


        TrajectoryManager.GetInstance().CreateTrajectories(this.gameObject);


        UiCanvas.GetInstance().PlanetDataView(transform, Planet);

        Globals.spaceAddress[2] = Planet.Id;




    }




    void FixedUpdate()
    {
     
 
        if (rotations > 1) { rotations = 0; };
        if (rotations < 0) { rotations = 0; };
        rotations += rotationspeed;
        Vector3 currentPosition = transform.position;

        transform.RotateAround(transform.parent.position, transform.up, Planet.OrbitSpeed *  rotations);
        Planet.SetOrbitPhase(Planet.OrbitSpeed * 360f * Time.deltaTime);

        OrbitVector = (transform.position - currentPosition).normalized;



    }
 



}
