using UnityEngine;
using System.Collections;

public class UIPlanetTrackerCircle : MonoBehaviour
{
    Camera SystemCamera;

    void Start()
    {
        SystemCamera = GameObject.Find("/Camera/ClusterCameraSystem/CameraTarget/System Camera").GetComponent<Camera>();
        CreateCirclePoints();
    }

    void Update()
    {

    }

    void CreateCirclePoints()
    {
        int Segments = 120;

        LineRenderer OrbitCircle;
        OrbitCircle = this.GetComponent<LineRenderer>();
        if (OrbitCircle == null) { Debug.Log("lineRenderer null"); }

        OrbitCircle.positionCount = Segments;

        float segmentX;
        float segmentY;
        float angle = 42f;
        OrbitCircle.positionCount = Segments + 1;
        OrbitCircle.widthMultiplier = 2.5f;
        OrbitCircle.startColor = new Color(0.8f, 0.83f, 0.9f, 0.2f);
        OrbitCircle.endColor = new Color(0.8f, 0.83f, 0.9f, 0.2f);


        for (int i = 0; i <= Segments; i++)
        {
            segmentX = Mathf.Sin(Mathf.Deg2Rad * angle) * 25;
            segmentY = Mathf.Cos(Mathf.Deg2Rad * angle) * 25;

            OrbitCircle.SetPosition(i, SystemCamera.WorldToScreenPoint(new Vector3(segmentX, segmentY, 0)));
            angle += (360f / Segments);
        }

    }
}
