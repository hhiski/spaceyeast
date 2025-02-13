
using Game.Lines;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Reflection;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UIElements;



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
    public GameObject GetLineObject()
    {
        return Line;
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

    Vector3 RotatePointAroundInclinedPivot(Vector3 point, Vector3 pivot, Vector3 axis, float angle)
    {
        axis.Normalize(); // Ensure the axis is normalized
        Quaternion rotation = Quaternion.AngleAxis(angle, axis); // Create rotation quaternion

        Vector3 direction = point - pivot; // Get point direction relative to pivot


        direction = rotation * direction; // Rotate it
        point = direction + pivot; // Calculate rotated point
        return point;
      
    }
    Vector3 RotatePointAroundPivot(Vector3 point, Vector3 pivot, Quaternion rotation)
    {
        Vector3 direction = point - pivot; // Get point direction relative to pivot



        direction = rotation * direction; // Rotate it
        point = direction + pivot; // Calculate rotated point
        return point;
    }
    public void DrawBrachistochroneLine(float EngineForce, float StartingVelocity, float RotationSpeed, float AngleDifferenceOffset, Vector3 vectorAbjuster, AnimationCurve flightCurve)
    {

        float angleDifferenceOffset = AngleDifferenceOffset;
        Vector3 startPos = Start.transform.position;
        Vector3 endPos = Target.transform.position;
        Vector3 startOrbitVector = Start.GetComponent<SystemPlanet>().GetOrbitVector();
        Vector3 startOrbitAxis = Start.GetComponent<SystemPlanet>().GetOrbitAxis();
        Vector3 endOrbitAxis = Target.GetComponent<SystemPlanet>().GetOrbitAxis();

        startOrbitVector.z /= 5f;
        endOrbitAxis.z /= 5f;

        Debug.DrawRay(startPos, startOrbitVector * 360, UnityEngine.Color.red);
        Debug.DrawRay(startPos, startOrbitAxis * 360, UnityEngine.Color.green);
        Debug.DrawRay(startPos, vectorAbjuster * 55, UnityEngine.Color.blue);
        Debug.DrawRay(endPos, vectorAbjuster * 55, UnityEngine.Color.blue);
        Debug.DrawRay(endPos, endOrbitAxis * 360, UnityEngine.Color.green);
        LineRenderer lineRenderer = Line.GetComponent<LineRenderer>();
        lineRenderer.useWorldSpace = true;
      //  Line.transform.rotation = Line.transform.parent.rotation;


        const int linePointCount = 500;
        lineRenderer.positionCount = linePointCount;

        float totalDistance = Vector3.Distance(startPos, endPos);
        if (totalDistance == 0)
        {
            Debug.LogWarning("TOTAL BrachistochroneLine DISTANCE ZERO");

        }

        float angle = Vector3.SignedAngle(Vector3.right, startPos, Vector3.up);
        if (angle < 0) angle += 360f;

        float angleB = Vector3.SignedAngle(Vector3.right, endPos, Vector3.up);
        if (angleB < 0) angleB += 360f;


        float orbitPhaseWithOmega = angle * (1 / (1 + startOrbitAxis.y)); //The longitude of the ascending node rotates phase with the planet. 


        float orbitPhaseB = Target.GetComponent<SystemPlanet>().GetOrbitPhase() * (1 / (1 + startOrbitAxis.y));

        //If the target planet's phase angle difference is small enough, trajactory lines rather goes around the solar midpoint. 
        float difference = angleB - angle - angleDifferenceOffset;

        difference = difference % 360;
        if (difference < 0f) difference += 360f;
        float angleFactor = 360f - difference;

        if (angleFactor < 0f) angleFactor += 360f;


        Vector3[] points = new Vector3[100];
        Vector3 accelerationVector = startPos + new Vector3(0, 0, 0);
        Vector3 decelerationVector = endPos + new Vector3(0, 0, 0);


        Vector3[] accelerations = new Vector3[500];
        Vector3[] decelerations = new Vector3[500];


        int accelerationIndex, decelerationIndex;
        float phase;


        difference /= 360f;
        float radiusFactor = difference * 2f;



        for (accelerationIndex = 0; accelerationIndex < accelerations.Length; accelerationIndex++)
        {
            Vector3 oldA = accelerationVector;
            accelerationVector = RotatePointAroundInclinedPivot(accelerationVector, Vector3.zero, startOrbitAxis, radiusFactor * RotationSpeed);
            accelerations[accelerationIndex] = accelerationVector;
        }
        for (decelerationIndex = 0; decelerationIndex < decelerations.Length; decelerationIndex++)
        {
            decelerationVector = RotatePointAroundInclinedPivot(decelerationVector, Vector3.zero, endOrbitAxis, radiusFactor * EngineForce);
            decelerations[decelerationIndex] = decelerationVector;
        }

        System.Array.Reverse(decelerations);
        Vector3 point;
        for (accelerationIndex = 0; accelerationIndex < linePointCount; accelerationIndex++)
        {
            phase = (float)(accelerationIndex) / (float)linePointCount;

            // float smoothPhase = (1f - Mathf.Cos(Mathf.PI * phase)) / 2f;

            phase = flightCurve.Evaluate(phase);
            point = Vector3.LerpUnclamped(accelerations[accelerationIndex], decelerations[accelerationIndex], phase);
            lineRenderer.SetPosition(accelerationIndex, point);
        }





    }


}
public class TrajectoryManager : MonoBehaviour
{
    List<Trajectory> Trajectories = new List<Trajectory>();

    [SerializeField] Material LineMaterial;
    public bool DrawingTrajectories = false;
    public float EngineForce = 1;
    public float StartingVelocity = 0.27f;
    public float RotationSpeed = 1;
    private static TrajectoryManager _instance;

    [SerializeField] AnimationCurve flightCurve = AnimationCurve.Linear(0f, 0f, 1f, 1f);

    [SerializeField] float AngleDifferenceOffset = 30f;
    [SerializeField] Vector3 vectorAbjuster = new Vector3(0,0,0);
    [SerializeField] Vector3 vectormid = new Vector3(0, 0, 0);


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

    void OnDisable()
    {
        ClearTrajectories();
    }


    public void ClearTrajectories()
    {

        DrawingTrajectories = false;
        
        foreach (Trajectory trajectory in Trajectories)
        {
            GameObject lineObject = trajectory.GetLineObject();
            if (lineObject != null)
            {
                Destroy(lineObject);
            }
        }
        Trajectories.Clear();
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


            GameObject line = LineManager.Instance.CreateLineObject(sourcePlanet.transform, "Trajectory Line", linePoints, LineType.Trajectory);

            line.tag = "System";

            Trajectory trajectory = new  Trajectory(sourcePlanet, otherPlanet, line);
            Trajectories.Add(trajectory);
                Debug.Log("planet " + sourcePlanet.GetComponent<SystemPlanet>().name + " --- orbitAxis " + sourcePlanet.GetComponent<SystemPlanet>().GetOrbitAxis() + " orbitVector" + sourcePlanet.GetComponent<SystemPlanet>().GetOrbitVector());
                


                index++;

            }


        }
        DrawingTrajectories = true;
    }


    void FixedUpdate()
    {
        if (DrawingTrajectories)
        {
            foreach (Trajectory trajectory in Trajectories)
            {
                trajectory.DrawBrachistochroneLine(EngineForce, StartingVelocity, RotationSpeed, AngleDifferenceOffset, vectorAbjuster, flightCurve);
            }
        }
        
    }
}
