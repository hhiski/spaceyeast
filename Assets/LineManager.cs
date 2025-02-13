using UnityEngine;

namespace Game.Lines 
{
    public class LineManager : MonoBehaviour
    {
        public static LineManager Instance { get; private set; }

        public Material GalacticLineMaterial;
        public Material OrbitalLineMaterial;
        public Material TrajectoryLineMaterial;
        public Material ClusterLineMaterial;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject); // Optional: Keeps the manager persistent across scenes
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public GameObject CreateLineObject(Transform parent, string name, Vector3[] segments, LineType lineType)
        {

            GameObject lineObject = new();

            lineObject.transform.parent = parent.transform;
            lineObject.name = name;
            lineObject.transform.position = new Vector3(0, 0, 0);
            lineObject.AddComponent<LineRenderer>();
            LineRenderer lineRenderer = lineObject.GetComponent<LineRenderer>();
            lineRenderer.material = GetMaterial(lineType);
            int numberOfSegments = segments.Length;
            lineRenderer.positionCount = numberOfSegments;
         

            for (int segmentIndex = 0; segmentIndex < numberOfSegments; segmentIndex++)
            {

                Vector3 segmentPosition = segments[segmentIndex];
                lineRenderer.SetPosition(segmentIndex, segmentPosition);
            }

            return lineObject;

        }

        public GameObject CreateBObject(Transform parent, Transform target, string name, float degreeDifference, Vector3 barycenter,  LineType lineType)
        {

            GameObject lineObject = new();

            lineObject.transform.parent = parent.transform;
          
            lineObject.name = name;
            lineObject.transform.position = new Vector3(0, 0, 0);
            lineObject.AddComponent<LineRenderer>();
            LineRenderer lineRenderer = lineObject.GetComponent<LineRenderer>();
            lineRenderer.material = GetMaterial(lineType);
            lineRenderer.positionCount = 100;

            lineRenderer.loop = false;
            Vector3[] segments = new Vector3[100];
            Vector3 orbiterPos = parent.transform.position;
            float segmentX;
            float segmentZ;
            float angle = 0f;
            float orbitalDistance = Vector3.Distance(orbiterPos, barycenter);
            float distanceA = Vector3.Distance(parent.position, barycenter);
            float distanceB = Vector3.Distance(target.position, barycenter);
            float phase = 0;

            for (int segmentIndex = 0; segmentIndex < 100; segmentIndex++)
            {
                phase = segmentIndex / 100f;
                orbitalDistance = Mathf.Lerp(distanceA, distanceB, phase);
                segmentX = barycenter.x + Mathf.Sin(Mathf.Deg2Rad * angle) * orbitalDistance;
                segmentZ = barycenter.z + Mathf.Cos(Mathf.Deg2Rad * angle) * orbitalDistance;
                segments[segmentIndex] = new Vector3(segmentX, 0, segmentZ);

                angle = Mathf.Lerp(0, degreeDifference, phase); 

                lineRenderer.SetPosition(segmentIndex, segments[segmentIndex]);
            }


            lineObject.GetComponent<LineRenderer>().useWorldSpace = true;
            lineObject.tag = "UIElement";

            return lineObject;
        }

        public GameObject CreateCircleObject(Transform parent, string name, Vector3 barycenter, int numberOfSegments, LineType lineType)
        {

            GameObject lineObject = new();
          //  int UILayer = LayerMask.NameToLayer("UI");
          //  lineObject.layer = UILayer;

            lineObject.transform.parent = parent.transform;
            lineObject.transform.rotation = parent.transform.rotation;
            lineObject.name = name;
            lineObject.transform.position = new Vector3(0, 0, 0);
            lineObject.AddComponent<LineRenderer>();
            LineRenderer lineRenderer = lineObject.GetComponent<LineRenderer>();
            lineRenderer.material = GetMaterial(lineType);
            lineRenderer.positionCount = numberOfSegments;

            lineRenderer.loop = true;
            Vector3[]segments = new Vector3[numberOfSegments];
            Vector3 orbiterPos = parent.transform.position;
            float segmentX;
            float segmentZ;
            float angle = 0f;
            float orbitalDistance = Vector3.Distance(orbiterPos, barycenter);

            for (int segmentIndex = 0; segmentIndex < numberOfSegments; segmentIndex++)
            {
                segmentX = barycenter.x + Mathf.Sin(Mathf.Deg2Rad * angle) * orbitalDistance;
                segmentZ = barycenter.z + Mathf.Cos(Mathf.Deg2Rad * angle) * orbitalDistance;
                segments[segmentIndex] = new Vector3(segmentX, 0, segmentZ);

                angle += (360f / numberOfSegments);

                lineRenderer.SetPosition(segmentIndex, segments[segmentIndex]);
            }

            lineObject.GetComponent<LineRenderer>().useWorldSpace = false;
            lineObject.tag = "UIElement";

            return lineObject;
        }


        public  Material GetMaterial(LineType lineType)
        {
            switch (lineType)
            {
                case LineType.Galactic:
                    return GalacticLineMaterial;
                case LineType.Orbital:
                    return OrbitalLineMaterial;
                case LineType.Trajectory:
                    return TrajectoryLineMaterial;
                case LineType.Cluster:
                    return ClusterLineMaterial;
                default:
                    return null;
            }
        }
    }

    public enum LineType
    {
        Galactic,
        Orbital,
        Trajectory,
        Cluster
    }
}