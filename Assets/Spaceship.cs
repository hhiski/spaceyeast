using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spaceship : MonoBehaviour
{

    public float TurnSpeed = 0.2f;
    public float ThrusterSpeed = 0f;
    public float ThrusterSpeedMax = 1f;
    public float ThrusterSpeedMin = 0f;


    GameObject MovementLine;
    Material LineMaterial;
    Camera SystemCamera;

    Vector3 m_MyFirstVector = Vector2.zero;
    Vector3 m_MySecondVector = Vector2.zero;
    float  m_Angle = 0.0f;
    // Start is called before the first frame update
    void Start()
    {

        SystemCamera = CameraOrbit.GetInstance().SystemCamera;
        //     line.GetComponent<LineRenderer>().useWorldSpace = false;
        LineMaterial = new Material(Shader.Find("Legacy Shaders/Particles/Alpha Blended Premultiply"));
    }

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.W) == true)
        {
            Debug.Log("FORWARD!");
            ThrusterSpeed = ThrusterSpeed + 0.01f;
        }

        if (Input.GetKey(KeyCode.S) == true)
        {
            ThrusterSpeed = Mathf.Clamp(ThrusterSpeed - 0.01f, ThrusterSpeedMin, ThrusterSpeedMax);
        }

        if (Input.GetKey(KeyCode.D) == true)
        {
            this.transform.Rotate(0, TurnSpeed, 0.0f, Space.Self);
        }
        if (Input.GetKey(KeyCode.A) == true)
        {
            this.transform.Rotate(0, -TurnSpeed, 0.0f, Space.Self);
        }


        if (Input.GetMouseButton(0))
        {

            Plane plane = new Plane(Vector3.up, 0);
            Vector3 mousePlanePos = new Vector3(0, 0, 0);

            float distance;
            Ray ray = SystemCamera.ScreenPointToRay(Input.mousePosition);
            if (plane.Raycast(ray, out distance))
            {
                mousePlanePos = ray.GetPoint(distance);
            }





            float shipDirection = transform.localEulerAngles.y;
            Vector3 relativeMousePos = mousePlanePos - transform.position;
            float angle = Vector3.Angle(transform.forward, relativeMousePos);
            Vector3[] trajectory = new Vector3[50];
            Vector3 trajectorySegment;

            foreach (Vector3 segment in trajectory)
            {
              

            }

            Vector3 previousSegment = transform.position;
            Vector3 Velocity = transform.forward* 2;

            for (int segmentIndex = 0; segmentIndex < trajectory.Length; segmentIndex++)
            {

                trajectory[segmentIndex] = previousSegment + Velocity;
                Velocity = Vector3.RotateTowards(Velocity, relativeMousePos, TurnSpeed, 0.0f);
                previousSegment = trajectory[segmentIndex];
         

                float TrajectoryDot = Vector3.Dot(Vector3.Normalize(relativeMousePos), Vector3.Normalize(Velocity));

                if (TrajectoryDot >= 1 )
                {
                    break;
                }

            };

            
            for (int segmentIndex = 1; segmentIndex < trajectory.Length; segmentIndex++)
            {
                Color dd = new Color((float)segmentIndex/50f, (float)segmentIndex /20f, 0.4f);
                Debug.DrawLine(trajectory[segmentIndex - 1], trajectory[segmentIndex], dd, 0.1f);

            }


            //Fetch the first GameObject's position
            m_MyFirstVector = new Vector2(transform.position.x, transform.position.z);
            //Fetch the second GameObject's position
            m_MySecondVector = new Vector2(relativeMousePos.x, relativeMousePos.z);
            //Find the angle for the two Vectors
            m_Angle = Vector2.Angle(m_MyFirstVector, mousePlanePos);

            float r_Angle = Vector2.Angle(transform.position, mousePlanePos);

            //Draw lines from origin point to Vectors
            Debug.DrawLine(transform.position, transform.position + transform.forward * 100f, Color.magenta);
            Debug.DrawLine(transform.position, mousePlanePos, Color.blue);

            //Log values of Vectors and angle in Console
            /*Debug.Log("MyFirstVector: " + m_MyFirstVector);
            Debug.Log("MySecondVector: " + m_MySecondVector);
            Debug.Log("Angle Between Objects: " + m_Angle);
            Debug.Log("Mouse Angle: " + angle);
            Debug.Log("Trajectory Angle: " + r_Angle);*/
            // MovementLine = LineFunctions.CreateLineObject(this.transform, new Vector3(0, 0, 0), "Orbit Line", trajectory, LineMaterial, 0.3f, false);


        }

        transform.position = transform.position +   transform.forward * ThrusterSpeed;
    }
}
