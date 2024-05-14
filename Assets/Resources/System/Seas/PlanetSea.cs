using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NoiseSpace;

public class PlanetSea : MonoBehaviour
{
    [SerializeField] GameObject sea;
    [SerializeField] GameObject ice;

    NoiseFunctions NoiseFunctions = new NoiseFunctions();

    Mesh seaMesh;
    Mesh seaSharedMesh;
    Mesh iceMesh;
    Mesh iceSharedMesh;

    int[] seaTris;

    Vector3[] vertices;
    Vector3[] verticesBackup;
    Noise NoiseLayer;


    public Color seaColor = new Color(0, 0, 1);

    [SerializeField] float seaLevel = 0.995f;
    [SerializeField] float iceLevel = 0.01f;
    [SerializeField] float iceCoverage = 0.0f;

    int seed = 22;

    void Start()
    {
        if (sea != null)
        {

            sea = Instantiate(sea, this.transform) as GameObject;
            seaSharedMesh = sea.GetComponent<MeshFilter>().sharedMesh;
            seaMesh = Instantiate(seaSharedMesh);
            sea.GetComponent<MeshFilter>().sharedMesh = seaMesh;
            int[] seaTris = seaMesh.triangles;

        }

        if (ice != null)
        {
            ice = Instantiate(ice, this.transform) as GameObject;
            iceSharedMesh = ice.GetComponent<MeshFilter>().sharedMesh;
            iceMesh = Instantiate(seaSharedMesh);
            ice.GetComponent<MeshFilter>().sharedMesh = iceMesh;
        }



        NoiseLayer = new Noise(seed);
        ShapePlanetSea();
    }

    void LateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.B) == true)
        {

            ShapePlanetSea();
        }

    }

    void UpdateSeaColors()
    {
        Renderer seaRender = sea.gameObject.GetComponent<Renderer>();
        seaRender.material.SetColor("_Color", seaColor);

        
        Transform[] transforms = this.GetComponentsInChildren<Transform>();

        foreach (Transform child in transforms)
        {
            if (child.gameObject.name == "Sea")
            {
                Renderer SeaGlow = child.gameObject.GetComponent<Renderer>();
                SeaGlow.material.SetColor("_Color", seaColor);
            }
            else if (child.gameObject.name == "Atmosphere")
            {
                Renderer AtmosphereGlow = child.gameObject.GetComponent<Renderer>();
                AtmosphereGlow.material.SetColor("_Color", seaColor);
            }
        }
        
    }

    /*
    float PerlinFilter(Vector3 point, Noise noiseFilter, float frequency, float amplitude)
    {



        point = new Vector3(point.x * frequency, point.y * frequency, point.z * frequency);
        float noise = noiseFilter.Evaluate(point);

        noise =1+(noise * amplitude);
        return noise;
    }*/
    void SplitMesh()
    {

        vertices = seaMesh.vertices;
        Vector3[] normals = seaMesh.normals;
        int[] triangles = seaMesh.triangles;
        var newVertices = new Vector3[triangles.Length];
        var newNormals = new Vector3[triangles.Length];
        var newTriangles = new int[triangles.Length];

        for (var i = 0; i < triangles.Length; i++)
        {
            newVertices[i] = vertices[triangles[i]];
            newNormals[i] = normals[triangles[i]];
            newTriangles[i] = i;
        }

        seaMesh.vertices = newVertices;
        seaMesh.normals = newNormals;
        seaMesh.triangles = newTriangles;

    }
    

    void UpdateIceLevel()
    {
        vertices = iceSharedMesh.vertices;
        Vector3 point;

        for (int i = 0; i < vertices.Length; i++)
        {
            point = vertices[i] * seaLevel;
            float polarLatitude = (1-iceCoverage);
            float articLatitude = polarLatitude - (0.55f * iceCoverage);

            if (point.y > polarLatitude || point.y < -polarLatitude) {
                point = point * iceLevel;

            }

            else if (point.y > articLatitude - 0.2f || point.y < -articLatitude + 0.2f)
            {
               
                float vLatitude = vertices[i].y;
                float snowAmplitude = (iceLevel - 1) * 0.55f;
                float snowNoise = NoiseFunctions.PerlinFilter(point, NoiseLayer, 2.6f, 1, snowAmplitude,0);
                snowNoise = 1 + (snowNoise * snowAmplitude);
                if (snowNoise > 1) { snowNoise = 1; } else { snowNoise = 0; };

                vertices[i] =  (vertices[i]  * snowNoise * iceLevel);
            }

            else
            {
                vertices[i] = vertices[i] * 0.95f; ;
            }

            vertices[i] = point;
        }

       iceMesh.vertices = vertices;
    }


    public void CopyVertices()
    {
        Mesh sharedMesh = GetComponent<MeshFilter>().sharedMesh;
        seaMesh = Instantiate(sharedMesh);
        GetComponent<MeshFilter>().sharedMesh = seaMesh;


        vertices = seaMesh.vertices;
        verticesBackup = seaMesh.vertices;
    }

    public void ShapePlanetSea()
    {
       // UpdateSeaLevel();


        // UpdateIceLevel();
    }
}
