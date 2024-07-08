using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ColorSpace;

public class PlanetClouds : MonoBehaviour
{


    Mesh cloudMesh;
    Mesh cloudSharedMesh;

    Vector3[] vertices;
    Vector3[] verticesBackup;
    Noise NoiseLayer;

    ColorFunctions ColorFunctions = new ColorFunctions();

    [SerializeField] Color CloudColor = new Color(1, 1, 1, 1);
    [SerializeField] float HueShift = 0;
    [SerializeField] float CloudPower = 1.00f;
    [SerializeField] float CloudSwirl = 0.4f;
    [SerializeField] float Amplitude = 1;
    [SerializeField] float Frequency = 1;
    [SerializeField] float Stripify = 1f;

    bool CloudMeshInstantiated = false;

    public int Seed = 0;

    System.Random Random = new System.Random();

    void Start()
    {


    }

    void InstantiateCloudscape()
    {
        cloudSharedMesh = this.gameObject.GetComponent<MeshFilter>().sharedMesh;
        cloudMesh = Instantiate(cloudSharedMesh);
        this.GetComponent<MeshFilter>().sharedMesh = cloudMesh;
        CloudMeshInstantiated = true;
    }

    public void SetCloudPressureThickness(float pressure, bool useClouds)
    {
        Amplitude = 0;
        if (useClouds) {
            if (pressure < 0.2f)  { Amplitude = 0; }
            else if(pressure < 0.6f) { Amplitude = 0.7f; }
            else if(pressure < 1.2) { Amplitude = 1f; }
            else if (pressure < 1.7) { Amplitude = 1.2f; }
            else  { Amplitude = 1.5f; }
        };
    }


    void Update()
    {
        this.gameObject.transform.RotateAround(gameObject.transform.position, Vector3.up, 4f * Time.deltaTime);
    }

    public void SetHueShift(float hueShift)
    {
        HueShift = hueShift;
    }


    void LateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.C) == true)
        {
            ShapePlanetClouds();
        }

    }

    public void UpdateSeeds()
    {
        NoiseLayer = new Noise(Seed);
        Random = new System.Random(Seed);
    }

    float PerlinFilter(Vector3 point, Noise noiseFilter, float frequency, float amplitude)
    {

        point.x = point.x * Stripify;
        point.z = point.z * Stripify;
        point = new Vector3(point.x * frequency, point.y * frequency, point.z * frequency);
        float noise = noiseFilter.Evaluate(point);

        noise = noiseFilter.Evaluate(point * (1 + CloudSwirl * noise));
  
        noise = (((noise)+1)/2)* amplitude;
        noise = Mathf.Pow(noise, CloudPower);
        return noise;
    }



    void UpdateCloudVertices()
    {

        List<float> alphaList = new List<float>();
        vertices = cloudSharedMesh.vertices;
        Color[] colors = new Color[vertices.Length];
        Color cloudColorShifted = ColorFunctions.HueShiftColor(CloudColor, HueShift, 1); 

        float alphaNoise = 1;

        Vector3 point;
        for (int i = 0; i < vertices.Length; i++)
        {
            point = vertices[i];
            if (Amplitude > 0.01) {
            alphaNoise = PerlinFilter(point, NoiseLayer, Frequency, Amplitude);
            alphaNoise += PerlinFilter(point, NoiseLayer, 2*Frequency, 0.5f*Amplitude);
            alphaNoise = alphaNoise - 0.2f;
            colors[i] = cloudColorShifted;
            colors[i].a = alphaNoise;
            }
            else
            {
                colors[i].a = 0;
            }
        }

        cloudMesh.colors = colors;
    }



    public void ShapePlanetClouds()
    {
        if (!CloudMeshInstantiated)
        {
            InstantiateCloudscape();

        }

        UpdateSeeds();
        UpdateCloudVertices();


    }
}
