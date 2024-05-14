using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static CelestialBody;

public class Controls : GalaxyCatalog
{
    bool TimeControlReady = true;

    GalaxyCatalog GalaxyCatalog;
    GameObject Galaxy;
    CameraOrbit CameraOrbit;

    void Start()
    {

        Galaxy = GameObject.Find("/Galaxy");
        GalaxyCatalog = Galaxy.GetComponent<GalaxyCatalog>();
        CameraOrbit = CameraOrbit.GetInstance();
    }


        
    void ReadyTimeControl()
    {
        if (Input.GetKeyDown(KeyCode.KeypadPlus) == false)
        {
            TimeControlReady = true;
        }
        else if (Input.GetKeyDown(KeyCode.KeypadMinus) == false)
        {
            TimeControlReady = true;
        }
        else
        {
            Invoke("ReadyTimeControl", 0.11f);
        }

    }

    public void SwitchToSocietyScope()
    {
        string scaleLevel = Globals.spaceScaleLevel;
        int clusterId = Globals.spaceAddress[0];
        int systemId = Globals.spaceAddress[1];
        int planetId = Globals.spaceAddress[2];


        GalaxyCatalog.CreateSociety(clusterId, systemId, planetId);


    }


    public void ZoomOut()
    {
        string scaleLevel = Globals.spaceScaleLevel;
        int clusterId = Globals.spaceAddress[0];
        int systemId = Globals.spaceAddress[1];
        int planetId = Globals.spaceAddress[2];

        if (scaleLevel == "cluster")
        {
            GalaxyCatalog.CreateGalaxy();
        }

        else if (scaleLevel == "system")
        {
            GalaxyCatalog.CreateCluster(clusterId);
        }

        else if (scaleLevel == "society")
        {/*
            GameObject systemScope =  GalaxyCatalog.CreateSystem(clusterId, systemId);

            if (systemScope.GetComponent<SystemController>() != null)
            {
                GameObject selectedPlanet = systemScope.GetComponent<SystemController>().FindPlanetGameObject(planetId);
                
                if (selectedPlanet  != null)
                {
                    if (selectedPlanet.GetComponent<SystemPlanet>() != null)
                    {
                        Planet planet = selectedPlanet.GetComponent<SystemPlanet>().Planet;
                        UI.PlanetDataView(selectedPlanet.transform, planet);
                    }
                }

            }

            **/
        }
        else
        {
            GalaxyCatalog.CreateGalaxy();
        }

    }

    public void LocateSol()
    {
        GalaxyCatalog.LocateSol();
    }

    void Update()
    {

        if (TimeControlReady && Input.GetKeyDown(KeyCode.U) == true)
        {
            string scaleLevel = Globals.spaceScaleLevel;
            int clusterId = Globals.spaceAddress[0];
            int systemId = Globals.spaceAddress[1];
            int planetId = Globals.spaceAddress[2];
            Debug.Log("scaleLevel:" + scaleLevel + " clusterID:" + clusterId + " systemID:" + systemId + " planetId:" + planetId);

            Invoke("ReadyTimeControl", 0.05f);

        }



        if (TimeControlReady && Input.GetKeyDown(KeyCode.O) == true)
        {
            foreach (GameObject orbitLine in GameObject.FindGameObjectsWithTag("UIElement"))
            {
                LineRenderer lineRenderer = orbitLine.GetComponent<LineRenderer>();

                if (lineRenderer.enabled == true)
                {
                    Debug.Log("Switching lines off");
                    lineRenderer.enabled = false;
                }
                else
                {
                    Debug.Log("Switching lines on");
                    lineRenderer.enabled = true;
                }
            }

        }


        if (TimeControlReady && Input.GetKeyDown(KeyCode.Space) == true)
        {

            ZoomOut();
            Invoke("ReadyTimeControl", 0.11f);
        }


        if (TimeControlReady && Input.GetKeyDown(KeyCode.I) == true)
        {
            GalaxyCatalog.ShowPlanetNumbers();
        }
        
        if (TimeControlReady && Input.GetKeyDown(KeyCode.KeypadPlus) == true)
        {
            Time.timeScale = Time.timeScale * 2;
            Debug.Log("Time Scale:" + Time.timeScale);
            Invoke("ReadyTimeControl", 0.11f);
        }


        if (TimeControlReady && Input.GetKeyDown(KeyCode.KeypadMinus) == true)
        {
            Time.timeScale = Time.timeScale / 2;
            Debug.Log("Time Scale:" + Time.timeScale);
            Invoke("ReadyTimeControl", 0.11f);
        }
    }
}
