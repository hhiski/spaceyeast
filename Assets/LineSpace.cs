/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LineSpace
{
    static class LineFunctions
    {




        public static void CreateOrbitCircle(Transform parent, Vector3 orbitingPoint, Material material)
        {
            GameObject lineObject = new GameObject();

            lineObject.transform.parent = parent.transform;
            lineObject.name = "Orbit Line";
            lineObject.transform.position = new Vector3(0, 0, 0);
            lineObject.AddComponent<LineRenderer>();
            LineRenderer lineRenderer = lineObject.GetComponent<LineRenderer>();
            lineRenderer.material = material;
            lineRenderer.widthMultiplier = 0.5f;
            int numberOfSegments = 180;
            lineRenderer.positionCount = numberOfSegments;
            lineRenderer.loop = true;

            Vector3[] segments = new Vector3[numberOfSegments];
            Vector3 orbiterPos = parent.transform.position;
            float segmentX;
            float segmentZ;
            float angle = 42f;
            float orbitalDistance = Vector3.Distance(orbiterPos, orbitingPoint);

            for (int segmentIndex = 0; segmentIndex < numberOfSegments; segmentIndex++)
            {
                segmentX = orbitingPoint.x + Mathf.Sin(Mathf.Deg2Rad * angle) * orbitalDistance;
                segmentZ = orbitingPoint.z + Mathf.Cos(Mathf.Deg2Rad * angle) * orbitalDistance;
                segments[segmentIndex] = new Vector3(segmentX, 0, segmentZ);

                angle += (360f / numberOfSegments);

                lineRenderer.SetPosition(segmentIndex, segments[segmentIndex]);
            }



            lineObject.GetComponent<LineRenderer>().useWorldSpace = false;
            lineObject.tag = "UIElement";

        }

        public static void CreateLineObject(Transform parent, Vector3 offset, string name, Vector3[] segments, Material material, Gradient lineGradient, float widthMultiplier)
        {


            GameObject lineObject = new GameObject();

            lineObject.transform.parent = parent.transform;
            lineObject.name = name;
            lineObject.transform.position = new Vector3(0, 0, 0);
            lineObject.AddComponent<LineRenderer>();
            LineRenderer lineRenderer = lineObject.GetComponent<LineRenderer>();
            lineRenderer.material = material;
            lineRenderer.widthMultiplier = widthMultiplier;
            int numberOfSegments = segments.Length;
            lineRenderer.positionCount = numberOfSegments;


            for (int segmentIndex = 0; segmentIndex < numberOfSegments; segmentIndex++)
            {
 
                Vector3 segmentPosition = segments[segmentIndex];
                lineRenderer.SetPosition(segmentIndex, segmentPosition);
            }
            lineRenderer.colorGradient = lineGradient;
        }



        public static GameObject CreateLineObject(Transform parent, Vector3 offset, string name, Vector3[] segments, Material material, float widthMultiplier, bool loop)
        {

            GameObject lineObject = new GameObject();

            lineObject.transform.parent = parent.transform;
            lineObject.name = name;
            lineObject.transform.position = new Vector3(0, 0, 0);
            lineObject.AddComponent<LineRenderer>();
            LineRenderer lineRenderer = lineObject.GetComponent<LineRenderer>();
            lineRenderer.material = material;
            lineRenderer.widthMultiplier = widthMultiplier;
            lineRenderer.loop = loop;
            int numberOfSegments = segments.Length;
            lineRenderer.positionCount = numberOfSegments;

            for (int segmentIndex = 0; segmentIndex < numberOfSegments; segmentIndex++)
            {
                Vector3 segmentPosition = segments[segmentIndex];
                lineRenderer.SetPosition(segmentIndex, segmentPosition);
            }

            return lineObject;
        }

        public static GameObject CreateLineObject(Transform parent, Vector3 offset, string name, Vector3[] segments, Material material, float widthMultiplier)
        {

            GameObject lineObject = new GameObject();

            lineObject.transform.parent = parent.transform;
            lineObject.name = name;
            lineObject.transform.position = new Vector3(0, 0, 0);
            lineObject.AddComponent<LineRenderer>();
            LineRenderer lineRenderer = lineObject.GetComponent<LineRenderer>();
            lineRenderer.material = material;
            lineRenderer.widthMultiplier = widthMultiplier;
            int numberOfSegments = segments.Length;
            lineRenderer.positionCount = numberOfSegments;

            for (int segmentIndex = 0; segmentIndex < numberOfSegments; segmentIndex++)
            {
                Vector3 segmentPosition = segments[segmentIndex];
                lineRenderer.SetPosition(segmentIndex, segmentPosition);
            }

            return lineObject;
        }

        public static GameObject CreateLineObject(Transform parent, Vector3 offset, string name, Vector3[] segments, Material material, bool loop, Color color, float widthMultiplier)
        {

            GameObject lineObject = new GameObject();

            lineObject.transform.parent = parent.transform;
            lineObject.name = name;
            lineObject.transform.position = new Vector3(0, 0, 0);
            lineObject.AddComponent<LineRenderer>();
            LineRenderer lineRenderer = lineObject.GetComponent<LineRenderer>();
            lineRenderer.material = material;
            lineRenderer.startColor = color;
            lineRenderer.loop = loop;
            lineRenderer.endColor = color;
            lineRenderer.widthMultiplier = widthMultiplier;
            int numberOfSegments = segments.Length;
            lineRenderer.positionCount = numberOfSegments;

            for (int segmentIndex = 0; segmentIndex < numberOfSegments; segmentIndex++)
            {
                Vector3 segmentPosition = segments[segmentIndex];
                lineRenderer.SetPosition(segmentIndex, segmentPosition);
            }

            return lineObject;
        }

        public static void CreateLineObject(Transform parent, Vector3 offset, string name, Vector3[] segments, Material material, AnimationCurve lineCurve, float widthMultiplier, LineAlignment alignment)
        {

            GameObject lineObject = new GameObject();

            if (alignment == LineAlignment.TransformZ)
            {
                lineObject.transform.Rotate(90.0f, 0.0f, 0.0f, Space.World);
            }

            lineObject.transform.parent = parent;
            lineObject.name = name;
            lineObject.transform.position = new Vector3(0, 0, 0);
            lineObject.AddComponent<LineRenderer>();
            LineRenderer lineRenderer = lineObject.GetComponent<LineRenderer>();
            lineRenderer.material = material;
            lineRenderer.alignment = alignment;
            lineRenderer.widthMultiplier = widthMultiplier;
            lineRenderer.widthCurve = lineCurve;
            int numberOfSegments = segments.Length;
            lineRenderer.positionCount = numberOfSegments;

            for (int segmentIndex = 0; segmentIndex < numberOfSegments; segmentIndex++)
            {
                Vector3 segmentPosition = segments[segmentIndex] + offset;
                lineRenderer.SetPosition(segmentIndex, segmentPosition);
            }

        }

        

    }
}
*/