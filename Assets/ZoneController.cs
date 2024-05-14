using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LineSpace;

public class ZoneController : MonoBehaviour
{
    public Material ZoneBorderMaterial;
    public GameObject ZoneStarParticleHolder;
    public GameObject ZoneDustParticleHolder;
    public bool drawZones = true;
    public bool drawStars = true;
    public bool drawBorders = true;
    public bool drawDust = true;

    void Start()
    {
        int zoneIndex = 0;
        foreach (Transform zoneTransform in transform)
        {
            GameObject zone = zoneTransform.gameObject;

            zone.AddComponent<MeshCollider>();

            if (drawZones)
            {
                zone.AddComponent<ZoneClick>();
                zone.GetComponent<ZoneClick>().ZoneId = zoneIndex;
            }

            if (drawBorders)
                CreateBorders(zone);
            if (drawStars)
                CreateZoneMeshParticles(zone, ZoneStarParticleHolder);
            if (drawDust)
                CreateZoneMeshParticles(zone, ZoneDustParticleHolder);

            zoneIndex++;


        }

    }

        public void CreateZoneMeshParticles(GameObject zone, GameObject particleHolder)
    {

        if (particleHolder == null)
        {
            Debug.Log("ZoneParticleSystem PREFAB NOT FOUND!");
        }


        GameObject systemHolder = Instantiate(particleHolder, new Vector3(0f, 0f, 0f), transform.rotation) as GameObject;


        systemHolder.transform.parent = zone.transform;
        systemHolder.name = "Zone Particle System Object";

        ParticleSystem particleSystem = systemHolder.GetComponent<ParticleSystem>();
        ParticleSystem.ShapeModule shape = particleSystem.shape;

        MeshRenderer meshRender = zone.GetComponent<MeshRenderer>();

        shape.shapeType = ParticleSystemShapeType.MeshRenderer;
        shape.meshRenderer = meshRender;

    }




    public void CreateBorders(GameObject zone)
    {

        if (ZoneBorderMaterial == null)
        {
            ZoneBorderMaterial =  new Material(Shader.Find("Sprites/Default"));
        }

        Mesh mesh = zone.GetComponent<MeshFilter>().mesh;

        List<Edge> boundaryPath = GetEdges(mesh.triangles);
        boundaryPath = FindBoundary(boundaryPath);
        boundaryPath = SortEdges(boundaryPath);

        Vector3[] vertices = mesh.vertices;
        Vector3[] border = new Vector3[boundaryPath.Count];
        Vector3 pos;

        for (int i = 0; i < boundaryPath.Count; i++)
        {
            pos = vertices[boundaryPath[i].VertexStart];
            border[i] = transform.TransformPoint(pos);
        }



        LineFunctions.CreateLineObject(zone.transform, new Vector3(0, 0, 0), "Border Line", border, ZoneBorderMaterial, 1f, true); 

    }



    public struct Edge
    {
        public int VertexStart;
        public int VertexEnd;
        public int triangleIndex;

        public Edge(int start, int end, int vertexIndex)
        {
            VertexStart = start;
            VertexEnd = end;
            triangleIndex = vertexIndex;
        }
    }

    public List<Edge> GetEdges(int[] indices)
    {
        List<Edge> edgeList = new List<Edge>();
        for (int i = 0; i < indices.Length; i += 3)
        {
            int v1 = indices[i];
            int v2 = indices[i + 1];
            int v3 = indices[i + 2];
            edgeList.Add(new Edge(v1, v2, i));
            edgeList.Add(new Edge(v2, v3, i));
            edgeList.Add(new Edge(v3, v1, i));
        }
        return edgeList;
    }

    public List<Edge> FindBoundary( List<Edge> aEdges)
    {
        List<Edge> result = new List<Edge>(aEdges);
        for (int i = result.Count - 1; i > 0; i--)
        {
            for (int n = i - 1; n >= 0; n--)
            {
                if (result[i].VertexStart == result[n].VertexEnd && result[i].VertexEnd == result[n].VertexStart)
                {
                    // shared edge so remove both
                    result.RemoveAt(i);
                    result.RemoveAt(n);
                    i--;
                    break;
                }
            }
        }
        return result;
    }
    public List<Edge> SortEdges( List<Edge> aEdges)
    {
        List<Edge> result = new List<Edge>(aEdges);
        for (int i = 0; i < result.Count - 2; i++)
        {
            Edge E = result[i];
            for (int n = i + 1; n < result.Count; n++)
            {
                Edge a = result[n];
                if (E.VertexEnd == a.VertexStart)
                {
                    // in this case they are already in order so just continoue with the next one
                    if (n == i + 1)
                        break;
                    // if we found a match, swap them with the next one after "i"
                    result[n] = result[i + 1];
                    result[i + 1] = a;
                    break;
                }
            }
        }
        return result;
    }




    // Update is called once per frame
    void Update()
    {
        
    }
}
