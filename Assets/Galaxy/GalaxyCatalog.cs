using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using System.Linq;
using MathSpace;

using static CelestialBody;

public static class Globals
{
    public static string spaceScaleLevel = "system";
    public static int[] spaceAddress = new int[] { 0, 0, 0 };
}

public class GalaxyCatalog : MonoBehaviour
{
    MathFunctions MathFunctions = new MathFunctions();

    public Premaid Premaid = new Premaid();
    public Universe Universe = new Universe(0);
    public UiCanvas UI;
    public SkyboxController SkyboxController;


    void Awake()
    {
        UI = GameObject.Find("/Canvas").GetComponent<UiCanvas>();
        SkyboxController = GetComponent<SkyboxController>();

        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60;
    }

    void Start()
    {



        for (int id = 0; id < 50; id++)
        {
            if (id == 3) //Sol System
            {
                Cluster LocalBubbleCluster = Premaid.LocalBubbleCluster(id);
                Universe.Clusters.Add(LocalBubbleCluster);
            }
            else
            {
                Cluster GenericCluster = Premaid.GenericCluster(id);
                Universe.Clusters.Add(GenericCluster);
            }
        }
        
        CreateCluster(1);
        SaveIntoJson();
    }


    public void LocateSol()
    {
        Debug.Log("Locating Sol...");
        foreach (Cluster cluster in Universe.Clusters)
        {
            foreach (Star star in cluster.Stars)
            {
                if (star.Name == "Sol")
                {
                    int clusterID = cluster.Id;
                    int systemID = star.Id;
                    Debug.Log("clusterID:" + clusterID + " systemID:" + star.Id);
                    CreateSystem(clusterID, systemID);
                    break;
                }
            }
        }
    }

    // Later okeyy
    public void SaveIntoJson()
    {
        var clusterList = Universe.Clusters;
        var outputString = JsonUtility.ToJson(clusterList);
        File.WriteAllText(Application.persistentDataPath  + "MyFile.json", outputString);
    }

    void InactiveScopes()
    {
        foreach (Transform child in gameObject.transform)
        {
            if (child.gameObject.tag == "Zone")
            { child.gameObject.SetActive(false); }
            if (child.gameObject.name == "SystemScope")
            { child.gameObject.SetActive(false); }
            if (child.gameObject.name == "SocietyScope")
            { child.gameObject.SetActive(false); }
            if (child.gameObject.name == "ClusterScope")
            { child.gameObject.SetActive(false); }

        }

    }

    public void ShowPlanetNumbers()
    {
        List<string> planetTypesAll = new List<string>();
        List<string> planetTypesUniques = new List<string>(); //containing one of the each types

        int planetCount = 0;
        int typeCount = 0;

        foreach (Cluster cluster in Universe.Clusters)
        {
            foreach (Star star in cluster.Stars)
            {
                foreach (Planet planet in star.Planets)
                {
                    string planetType = planet.Type.Name;
                    planetTypesAll.Add(planetType);

                    if (!(planetTypesUniques.Contains(planetType)))
                    {
                        planetTypesUniques.Add(planetType);
                    }
                }
            }
        }

        planetCount = planetTypesAll.Count;
        Debug.Log("Total planet count: " + planetCount);

        foreach (string type in planetTypesUniques)
        {
            typeCount = planetTypesAll.Count(n => n == type);
            Debug.Log(type + "-type planet count:" + typeCount);
        }
    }

    public void CreateCluster(int clusterId)
    {
        Globals.spaceScaleLevel = "cluster";
        Globals.spaceAddress[0] = clusterId;


        InactiveScopes();

        foreach (Transform child in gameObject.transform)
        {
            if (child.gameObject.name == "ClusterScope")
            { child.gameObject.SetActive(true); }

        }


        foreach (Cluster cluster in Universe.Clusters)
        {
            if (cluster.Id == clusterId)
            {
                ClusterController.GetInstance().VisualizeCluster(cluster);
                break;
            }
           
        }


    }

    public void CreateSystem(int clusterId, int systemId)
    {
        Globals.spaceScaleLevel = "system";
        Globals.spaceAddress[0] = clusterId;
        Globals.spaceAddress[1] = systemId;


        InactiveScopes();

        foreach (Transform child in gameObject.transform)
        {
            if (child.gameObject.name == "SystemScope")
            { child.gameObject.SetActive(true); }

        }

        SkyboxController.SetSystemSkybox();

        foreach (Cluster cluster in Universe.Clusters)
        {
            foreach (Star star in cluster.Stars)
            {
               if (star.Id == systemId)
                {
                    SystemController.GetInstance().VisualizeSystem(star);
                    break;
                }
            }
        }
    }



    public void CreateSociety(int clusterId, int systemId, int planetId)
    {
        Globals.spaceScaleLevel = "society";
        Globals.spaceAddress[0] = clusterId;
        Globals.spaceAddress[1] = systemId;
        Globals.spaceAddress[2] = planetId;


        InactiveScopes();

        foreach (Transform child in gameObject.transform)
        {
            if (child.gameObject.name == "SocietyScope")
            { child.gameObject.SetActive(true); }

        }


        GameObject societyScope = GameObject.Find("Galaxy/SocietyScope");
        SocietyController societyController = societyScope.GetComponent<SocietyController>();

        foreach (Cluster cluster in Universe.Clusters)
        {
            foreach (Star star in cluster.Stars)
            {
                if (star.Id == systemId)
                {
                    foreach (Planet planet in star.Planets)
                    {
                        if (planet.Id == planetId)
                        {
                            societyController.VisualizeSociety(planet);
                            break;
                        }
                    }
                }
            }
        }



        foreach (Transform child in gameObject.transform)
        {
            if (child.gameObject.name == "SocietyScope")
            { child.gameObject.SetActive(true); }

        }
        GameObject SocietyScope = GameObject.Find("Galaxy/SocietyScope");
        SocietyController SystemController = SocietyScope.GetComponent<SocietyController>();




    }


    public void CreateGalaxy()
    {

        Globals.spaceScaleLevel = "galaxy";
        InactiveScopes();

        foreach (Transform child in gameObject.transform)
        {

            if (child.gameObject.tag == "Zone")
            {
                child.gameObject.SetActive(true);
            }

        }

        UI.GalaxyDataView("Milky Way");

        StartCoroutine(SkyboxController.SetSkybox("galaxy"));
    }



}
