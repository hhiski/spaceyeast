using LineSpace;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Reflection;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UIElements;
using static CelestialBody;
using static UnityEngine.GraphicsBuffer;

 class Trajectory
{
    float Acceleration = 0.1f;

    GameObject Start { get; set; }
    GameObject Target { get; set; }
    GameObject Line { get; set; }

    public Trajectory(GameObject start, GameObject target, GameObject line)
    {
        Start = start;
        Target = target;
        Line = line;
    }
    public GameObject GetTarget()
    {
        return Target;
    }

    public void DrawStraightLine()
    {

        Vector3 startPos = Start.transform.position;
        Vector3 endPos = Target.transform.position;
        Vector3[] linePoints = new Vector3[] { startPos, endPos };


        LineRenderer lineRenderer = Line.GetComponent<LineRenderer>();

        lineRenderer.SetPosition(0, startPos);
        lineRenderer.SetPosition(1, endPos);

    }
    Vector3 GetBezierPoint(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
    {
        // Cubic Bezier curve equation
        float u = 1 - t;
        float tt = t * t;
        float uu = u * u;
        float uuu = uu * u;
        float ttt = tt * t;

        Vector3 p = uuu * p0; //first term
        p += 3 * uu * t * p1; //second term
        p += 3 * u * tt * p2; //third term
        p += ttt * p3; //fourth term

        return p;
    }
    Vector3 CalculateBezierPoint(float t, Vector3 start, Vector3 control, Vector3 end)
    {
        float u = 1 - t;
        float tt = t * t;
        float uu = u * u;

        Vector3 point = uu * start; // (1-t)^2 * P0
        point += 2 * u * t * control; // 2(1-t)t * P1
        point += tt * end;        // t^2 * P2

        return point;
    }
    Vector3 CalculateHohmannPoint(Vector3 start, Vector3 end, float r1, float r2, float t)
    {
        // Semi-major axis of the transfer ellipse
        float a = (r1 + r2) / 2.0f;

        // Eccentricity of the transfer ellipse
        float e = (r2 - r1) / (r1 + r2);

        // Angle of the current point along the transfer ellipse
        float theta = Mathf.PI * t;

        // Radius at angle theta
        float r = (a * (1 - e * e)) / (1 + e * Mathf.Cos(theta));

        // Position in polar coordinates
        Vector3 point = new Vector3(r * Mathf.Cos(theta), 0, r * Mathf.Sin(theta));

        // Rotate to align with the starting and ending points
        Quaternion rotation = Quaternion.FromToRotation(Vector3.right, (end - start).normalized);
        point = rotation * point;


        return point;
    }
    Vector3 RotatePointAroundInclinedPivot(Vector3 point, Vector3 pivot, Vector3 axis, float angle)
    {
        axis.Normalize(); // Ensure the axis is normalized
        Quaternion rotation = Quaternion.AngleAxis(angle, axis); // Create rotation quaternion
        return RotatePointAroundPivot(point, pivot, rotation); // Use the original method
    }
    Vector3 RotatePointAroundPivot(Vector3 point, Vector3 pivot, Quaternion rotation)
    {
        Vector3 direction = point - pivot; // Get point direction relative to pivot
        direction = rotation * direction; // Rotate it
        point = direction + pivot; // Calculate rotated point
        return point; // Return it
    }
    public void DrawBrachistochroneLine(float EngineForce, float StartingVelocity, float RotationSpeed)
    {
   

        Vector3 startPos = Start.transform.position;
        Vector3 endPos = Target.transform.position;
        Vector3 startOrbitVector = Start.GetComponent<SystemPlanet>().GetOrbitVector();
        Vector3 startOrbitAxis = Start.GetComponent<SystemPlanet>().GetOrbitAxis();
        Vector3 startInitialPos = Start.GetComponent<SystemPlanet>().InitialPosition;
        Vector3 endOrbitVector = Target.GetComponent<SystemPlanet>().GetOrbitVector();
        Vector3 endOrbitAxis = Target.GetComponent<SystemPlanet>().GetOrbitAxis();
        Vector3 endInitialPos = Target.GetComponent<SystemPlanet>().InitialPosition;

        float startOrbitalDistance = startPos.magnitude;
        float endOrbitalDistance =endPos.magnitude;

        Vector3 midPos = (startPos + endPos) / 2;
        Vector3[] linePoints = new Vector3[] { startPos, endPos };


        LineRenderer lineRenderer = Line.GetComponent<LineRenderer>();
        const int linePointCount = 500;
        lineRenderer.positionCount = linePointCount;
        
        float totalDistance = Vector3.Distance(startPos, endPos);
        float halfDistance = totalDistance / 2;

        if (totalDistance == 0)
        {
            Debug.Log("TOTAL DISTANCE ZERO");
            return;
        }

        float angle = Vector3.SignedAngle(Vector3.right, startPos, Vector3.up);
        if (angle < 0) angle += 360f;

        float angleB = Vector3.SignedAngle(Vector3.right, endPos, Vector3.up);
        if (angleB < 0) angleB += 360f;


        float orbitPhaseA = angle;
        float orbitPhaseWithOmega = angle  * (1 / (1 + startOrbitAxis.y)); //The longitude of the ascending node rotates phase with the planet. 

        float normalizedorbitPhaseWithOmega = Mathf.InverseLerp(0, (1 / (1 + startOrbitAxis.y)), orbitPhaseWithOmega);

        float orbitPhaseB = Target.GetComponent<SystemPlanet>().GetOrbitPhase() * (1 / (1+startOrbitAxis.y));
        float difference = angleB - angle;
        if (difference < 0f) difference += 360f;
        float angleFactor = 360f - difference;

        if (angleFactor < 0f) angleFactor += 360f;


        //Debug.Log("A " + angle + "  -- B " + angleB + "   sep:" + difference + " omega: " + angleFactor);



        // Debug.Log("A " + angle + "  -- B "+ angleB + "   sep:"+ angle + angleB);


        Vector3[] points = new Vector3[100];
        Vector3 accelerationVector = startPos + new Vector3(0, 0, 0);
        Vector3 decelerationVector = endPos + new Vector3(0, 0, 0);


        Vector3[] accelerations = new Vector3[500];
        Vector3[] decelerations = new Vector3[500];


     //   accelerationVector += thrust;
        int accelerationIndex, decelerationIndex, fullIndex;
        float phase;


        difference /= 360f;
        Debug.Log("diffuse: " + difference + " ");
        float radiusFactor = difference * 2f;

        for (accelerationIndex = 0; accelerationIndex < accelerations.Length; accelerationIndex++)
        {
            phase = (float)(accelerationIndex) / (float)linePointCount;
          //  accelerationVector =  RotatePointAroundPivot(  accelerationVector, Vector3.zero, Quaternion.Euler(startOrbitAxis * radiusFactor * RotationSpeed ));

            accelerationVector = RotatePointAroundInclinedPivot(accelerationVector, Vector3.zero, startOrbitAxis, radiusFactor * RotationSpeed);
            accelerations[accelerationIndex] = accelerationVector;
        }
        for (decelerationIndex = 0; decelerationIndex < decelerations.Length; decelerationIndex++)
        {
            phase = (float)(decelerationIndex) / (float)linePointCount;
            //   decelerationVector = RotatePointAroundPivot(decelerationVector , Vector3.zero, Quaternion.Euler(endOrbitAxis *radiusFactor * EngineForce));
            decelerationVector = RotatePointAroundInclinedPivot(decelerationVector, Vector3.zero, endOrbitAxis, radiusFactor * EngineForce);
            decelerations[decelerationIndex] = decelerationVector;
        }

        System.Array.Reverse(decelerations);

        for (accelerationIndex = 0; accelerationIndex < linePointCount; accelerationIndex++)
        {
            phase = (float)(accelerationIndex) / (float)linePointCount;
            //  Vector3 point = Vector3.LerpUnclamped(accelerations[accelerationIndex], decelerations[accelerationIndex], phase);
            float smoothPhase = (1f - Mathf.Cos(Mathf.PI * phase)) / 2f;
            Vector3 point = Vector3.LerpUnclamped(accelerations[accelerationIndex], accelerations[accelerationIndex], smoothPhase );

            if (StartingVelocity >= 1)
                 point = Vector3.LerpUnclamped(decelerations[accelerationIndex], decelerations[accelerationIndex], smoothPhase );

            if (StartingVelocity >= 2)
                point = Vector3.LerpUnclamped(accelerations[accelerationIndex], decelerations[accelerationIndex], smoothPhase);

            lineRenderer.SetPosition(accelerationIndex, point);
        }





    }

}
public class TrajectoryManager : MonoBehaviour
{
    List<Trajectory> Trajectories = new List<Trajectory>();

    public Material LineMaterial;
    public bool DrawingTrajectories = false;
    public float EngineForce = 1;
    public float StartingVelocity = 0.27f;
    public float RotationSpeed = 1;
    private static TrajectoryManager _instance;
    private static TrajectoryManager Instance
    {
        get
        {
            if (_instance == null)
            {
                // If the instance is null, try to find it in the scene
                _instance = FindObjectOfType<TrajectoryManager>();

                if (_instance == null)
                {
                    Debug.LogError("TrajectoryManager NULL, CANT BE FOUND");
                }
            }

            return _instance;
        }
    }
    public static TrajectoryManager GetInstance()
    {
        return Instance;
    }

    void Start()
    {
        


    }
    public void ClearTrajectoriesSystemWide()
    {
      /*List<GameObject> otherPlanets = SystemController.GetInstance().GetPlanetObjects();

        foreach (GameObject planet in otherPlanets)
        {
            Trajectories.Clear();
            ClearTrajectories();
            Debug.Log("Deleting : " + planet.name + "trejecotries");
        }*/

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
    public void CreateTrajectories(GameObject sourcePlanet)
    {
        ClearTrajectories();

        List<GameObject> otherPlanets = SystemController.GetInstance().GetPlanetObjects();



        int index = 0;
        Vector3[] linePoints = new Vector3[] { new Vector3(0, 0, 0), new Vector3(0, 0, 0) };
        foreach (GameObject otherPlanet in otherPlanets)
        {
            if (otherPlanet != sourcePlanet) {
    
            GameObject line = LineFunctions.CreateLineObject(this.transform, new Vector3(0, 0, 0), "Trajectory Line", linePoints, LineMaterial, 0.5f, false);
            Trajectory trajectory = new  Trajectory(sourcePlanet, otherPlanet, line);
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


            foreach (Trajectory trajectory in Trajectories)
            {
               // trajectory.DrawStraightLine();
                trajectory.DrawBrachistochroneLine(EngineForce, StartingVelocity, RotationSpeed);
            }

        }
        
    }
}
