using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Lines;
using Game.Math;

using static CelestialBody;
public class SystemMoon : MonoBehaviour
{

    public Moon Moon;

    int Seed;

    public Vector3 Pos;

    public Material LineMaterial;

   // LineFunctions LineFunctions = new LineFunctions();

    System.Random Random = new System.Random();
    MathFunctions MathFunctions = new MathFunctions();
    float OrbitSpeed = 0;

    void Start()
    {
        Seed = Moon.Seed;
        OrbitSpeed = Moon.OrbitSpeed;
        Random = new System.Random(Moon.Seed);






    }



    public void Visualize()
    {
        GameObject PlanetSurfaceObject;

        PlanetSurfaceObject = CreatePlanetSurface();


        LineManager.Instance.CreateCircleObject(this.transform, "Orbital Circle", transform.parent.position, 360, LineType.Orbital);

        float inclinationFactor = Mathf.Abs((MathFunctions.StandardDeviation(0, 40, Seed + 50)));
        float inclinationX = (float)(MathFunctions.StandardDeviation(0, inclinationFactor, Seed + 10));
        float inclinationZ = (float)(MathFunctions.StandardDeviation(0, inclinationFactor, Seed + 20));


        transform.RotateAround(transform.parent.position, new Vector3(1, 0, 0), inclinationX);
        transform.RotateAround(transform.parent.position, new Vector3(0, 0, 1), inclinationZ);

        Moon.Pos = transform.position; // new position
    }

    GameObject CreatePlanetSurface()
    {

        Random = new System.Random(Moon.Seed);

        string planetTypename = Moon.Type.Name;
        string customSubType = Moon.Type.CustomSubType;

        string moonFolderPath = "";
        
        if (customSubType == "")
        {
            moonFolderPath = "System/Planets/" + planetTypename + "Planet/";
        }
        else
        {
            moonFolderPath = "System/Planets/" + "CustomPlanet/" + customSubType;
        }


        GameObject MoonVisualPrefab = Resources.Load<GameObject>("System/Planets/MinorPlanet/MinorPlanet") as GameObject;

        GameObject[] AvailableSelection = Resources.LoadAll<GameObject>(moonFolderPath) as GameObject[]; // Selects all 

        int randomObject = Random.Next(0, AvailableSelection.Length);

        GameObject PlanetVisualPrefab = AvailableSelection[randomObject];

        if (PlanetVisualPrefab == null)
        {
            Debug.Log("PLANET:" + moonFolderPath + " NOT FOUND!");
            MoonVisualPrefab = Resources.Load<GameObject>("System/Planets/MinorPlanet/MinorPlanet") as GameObject;
        }

        GameObject MoonSurfaceObject = Instantiate(PlanetVisualPrefab, this.transform, false) as GameObject;
        MoonSurfaceObject.GetComponent<PlanetSurface>().Planet = Moon;
        MoonSurfaceObject.GetComponent<PlanetSurface>().SetValues();
        MoonSurfaceObject.GetComponent<PlanetSurface>().CopyVertices();
     //   MoonSurfaceObject.GetComponent<PlanetSurface>().ShapePlanetSurface();

        return MoonSurfaceObject;
    }








    void Update()
    {

        transform.RotateAround(transform.parent.position, Vector3.up, OrbitSpeed * Time.deltaTime);

    }




}
