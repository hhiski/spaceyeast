/*using LineSpace;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.Pool;
using static CelestialBody;

public class Trajectory
{
    float Acceleration = 0.1f;

    GameObject Target { get; set; }
    GameObject Line { get; set; }
    public Trajectory(GameObject target, GameObject line)
    {
        Target = target;
        Line = line;
    }
    public GameObject GetTarget()
    {
        return Target;
    }

    public void DrawStraightLine(Vector3 StartPosition)
    {

        Vector3 startPos = StartPosition;
        Vector3 endPos = Target.transform.position;
        Vector3[] linePoints = new Vector3[] { startPos, endPos };


        LineRenderer lineRenderer = Line.GetComponent<LineRenderer>();

        lineRenderer.SetPosition(0, startPos);
        lineRenderer.SetPosition(1, endPos);

    }
    public void DrawBrachistochroneLine(Vector3 StartPosition)
    {

        Vector3 startPos = StartPosition;
        Vector3 endPos = Target.transform.position;
        Vector3 midPos = (startPos + endPos) / 2;
        Vector3[] linePoints = new Vector3[] { startPos, endPos };


        LineRenderer lineRenderer = Line.GetComponent<LineRenderer>();
        lineRenderer.positionCount = 100;

        float totalDistance = Vector3.Distance(startPos, endPos);
        float halfDistance = totalDistance / 2;

        if (totalDistance == 0)
        {
            Debug.Log("TOTAL DISTANCE ZERO");
            return;
        }

        float timeToMidpoint = Mathf.Sqrt(2 * halfDistance / Acceleration);
        float totalTime = 2 * timeToMidpoint;

        Vector3[] points = new Vector3[100];
        int numberOfPoints = points.Length;

        for (int i = 0; i < numberOfPoints; i++)
        {
            float t = (i / (float)(numberOfPoints - 1)) * totalTime;
            float currentDistance;

            if (t <= timeToMidpoint)
            {
                currentDistance = 0.5f * Acceleration * t * t;
                points[i] = Vector3.Lerp(startPos, midPos, currentDistance / halfDistance);
            }
            else
            {
                float timeFromMidpoint = t - timeToMidpoint;
                currentDistance = halfDistance + 0.5f * Acceleration * timeFromMidpoint * (2 * timeToMidpoint - timeFromMidpoint);
                points[i] = Vector3.Lerp(midPos, endPos, (currentDistance - halfDistance) / halfDistance);
            }

        }




        lineRenderer.SetPositions(points);

            
        

    }

}
public class SystemPlanetTrajectories : MonoBehaviour
{
    List<Trajectory> Trajectories = new List<Trajectory>();

    public Material LineMaterial;
    public bool DrawingTrajectories = false;

    void Start()
    {
        


    }
    public void ClearTrajectoriesSystemWide()
    {
        List<GameObject> otherPlanets = SystemController.GetInstance().GetPlanetObjects();

        foreach (GameObject planet in otherPlanets)
        {
            planet.GetComponent<SystemPlanetTrajectories>().Trajectories.Clear();
            planet.GetComponent<SystemPlanetTrajectories>().ClearTrajectories();
            Debug.Log("Deleting : " + planet.name + "trejecotries");
        }

    }

    public void ClearTrajectories()
    {
        Trajectories.Clear();
        DrawingTrajectories = false;
        
        foreach (Transform child in transform)
        {
            if (child.gameObject.name == "Trajectory Line" || child.gameObject.name == "Trajectory Line(Clone)")
            {
                Destroy(child.gameObject);
            }
            else if (child.gameObject.name == "Brachistochrone Trejectory" || child.gameObject.name == "Brachistochrone Trejectory(Clone)")
            {
                Destroy(child.gameObject);
            }
        }
    }
    public void DrawTrajectories()
    {
        ClearTrajectoriesSystemWide();

        List<GameObject> otherPlanets = SystemController.GetInstance().GetPlanetObjects();



        int index = 0;
        Vector3[] linePoints = new Vector3[] { new Vector3(0, 0, 0), new Vector3(0, 0, 0) };
        foreach (GameObject otherPlanet in otherPlanets)
        {
            if (otherPlanet != transform.gameObject) {
    
            GameObject line = LineFunctions.CreateLineObject(this.transform, new Vector3(0, 0, 0), "Trajectory Line", linePoints, LineMaterial, 0.5f, true);
            Trajectory trajectory = new  Trajectory(otherPlanet, line);
            Trajectories.Add(trajectory);

           
            index++;
            }
        }
        DrawingTrajectories = true;
    }


    // Update is called once per frame
    void Update()
    {
        if (DrawingTrajectories)
        {


            Vector3 startOrbitVector = gameObject.GetComponent<SystemPlanet>().OrbitVector;  

            foreach (Trajectory trajectory in Trajectories)
            {
                trajectory.DrawStraightLine(transform.position);
                trajectory.DrawBrachistochroneLine(transform.position);
            }

        }
        
    }
}
*/