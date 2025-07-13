using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Game.Math;
using Game.Noise;
using ColorSpace;
using VoronoiSpace;
using static CelestialBody;
using System.Linq;
using Unity.Collections;
using Unity.Jobs;
using Unity.VisualScripting;
using TreeEditor;
using System.Net;
using Game.ID;
using static UnityEditor.Searcher.SearcherWindow.Alignment;

[System.Serializable]
public class PlanetSurface : MonoBehaviour
{
    MathFunctions MathFunctions = new MathFunctions();
    readonly VoronoiFunctions VoronoiFunctions = new VoronoiFunctions();

    Mesh planetMesh;
    Mesh seaMesh;

    Color[] planetColors;
    Vector3[] planetVertices;
    Vector3[] planetVerticesBackup;


    Color[] voronoiColors;
    Vector3[] voronoiVertices;

    Noise NoiseLayer = new Noise(8);


    ColorFunctions ColorFunctions = new ColorFunctions();



    System.Random Random = new System.Random();

    [Range(0.0f, 1.0f)]
    public float Amplitude = 0.03f;
    [Range(1, 10)]
    public int levels = 1;
    [Range(0f, 10f)]
    public float Frequency = 1.8f;
    [Range(0f, 1f)]
    public float Roughness = 0.5f;
    [Range(0f, 2f)]
    public float Persistence = 1;


    public AnimationCurve heightCurve = AnimationCurve.Linear(0, 0, 1f, 1f);


    [SerializeField] AnimationCurve cellularCurve = AnimationCurve.Linear(0f, 0f, 1f, 1f);


    public float negativePeakClip = 0f;
    public float positivePeakClip = 2f;
    //public float craterAmplitude = 0;
    //public float craterSize = 0;
    public int craterAmount = 0;

    public int cellAmount = 0;
    public int cellPattern = 1;
    public float cellPower = 1f;


    enum CellularPattern
    {
        None,
        CurveA,
        CurveB,
        CurveAB,
        Crack
    }

    [SerializeField]
    CellularPattern selectedCellPattern;

    public float Flatness = 0;
    public float fluidicity = 0;
    public float weird = 0;
    [SerializeField]
    [Range(0f, 1f)]
    float modulo = 0;


    public float warpForce = 0f;
    public Vector3 warpVector = new Vector3(0, 0, 0);

    [SerializeField] Gradient colorGradient = new Gradient();
    float highestHeight = 0;
    float LowestHeight = 120;

    [SerializeField] Gradient colorSeaGradient = new Gradient();
    [SerializeField] float seaLevel = 0.0f;
    public Color colorClouds = new Color(1f, 1f, 1f, 1);

    public float colorStripify = 1f;
    public float colorVariation = 0f;
    float hueShift = 0f;

    public float atmosphereThickness = 0f;

    public float testpolarCoverage = 0f;
    public float iceCapsSize = 0f;
    public float mass = 1;
    public float rotationSpeed = 1;

    public Planet Planet;


    Material surfaceMaterial;
    Material shadowMaterial;

    public int seed = 1;
    string type = "empty";
    float temperature = -273;

    void LateUpdate()
    {
        //Debug.Log("Rotating data: planet " + Planet.Name + " parent "+ transform.parent.position + " speed " + Planet.OrbitSpeed);
        // transform.RotateAround(transform.parent.position, Vector3.up, Planet.OrbitSpeed * Time.deltaTime);

        if (Input.GetKeyDown(KeyCode.B) == true)
        {
            StartCoroutine(ShapePlanetSurface());
            planetMesh.RecalculateNormals();
            planetMesh.RecalculateTangents();

        }
        if (Input.GetKeyDown(KeyCode.J) == true)
        {
            Debug.Log("Pressing J");
            UpdateClouds();


        }

        if (Input.GetKeyDown(KeyCode.K) == true)
        {
            StartCoroutine(ShapePlanetSurface());
            planetMesh.RecalculateNormals();
        }




    }




    public void UpdateRing()
    {
        PlanetRing ring = GetComponentInChildren<PlanetRing>(true);



        if (ring != null)
        {
            ring.ColorRings(colorGradient);
        }
    }

    public void UpdateAtmosphere()
    {
        PlanetAtmosphere atm = GetComponentInChildren<PlanetAtmosphere>();
        Color atmosphereColor = colorGradient.Evaluate(0.5f);
        if (atm != null)
        {
            atm.SetAtmosphereColor(atmosphereColor);
        }
    }

    public void UpdateClouds()
    {
        PlanetClouds clouds = GetComponentInChildren<PlanetClouds>();

        if (clouds != null)
        {
            Color gasCloudColor = Planet.Atm.PrimaryGas.GetGasColor();
            float gasCloudPressure = Planet.Atm.Pressure;
            bool useClouds = Planet.Type.UseAtmosphericClouds;

            if (useClouds)
            {
                clouds.SetCloudColor(gasCloudColor);
                clouds.SetCloudCoverage(gasCloudPressure);
            }
            else
            {
                clouds.SetCloudCoverage(0);
            }


        }
    }

    void UpdateSea()
    {


        if (surfaceMaterial != null)
        {
            surfaceMaterial.SetFloat("LiquidLevel", seaLevel);
        }
        else
        {
            Debug.Log(surfaceMaterial + "not set!");
        }




        Vector3[] vertices = planetVertices;

        Vector3 point;


        for (int i = 0; i < vertices.Length; i++)
        {
            point = vertices[i];


            if (point.magnitude <= seaLevel)
            {
                point = point.normalized * seaLevel * 0.998f;
                vertices[i] = point;
            }



        }


    }

    void UpdateShadow()
    {
        float distanceFromSun = Planet.Pos.magnitude;

        if (shadowMaterial != null)
        {
            shadowMaterial.SetFloat("_PlanetDistance", distanceFromSun);
        }
        else
        {
            Debug.Log(shadowMaterial + "not set!");
        }

    }

    public void SetValues()
    {
        seed = Planet.Seed;
        type = Planet.Type.Name;
        temperature = Planet.Atm.Temperature;
        rotationSpeed = Planet.RotationSpeed;
        atmosphereThickness = Planet.Atm.Pressure;
        mass = Planet.Mass;
        iceCapsSize = Planet.Hydrosphere.GetIceCapSize();
        hueShift = ((float)Random.NextDouble() - 0.5f) * 2 * colorVariation; //hue variation is between -colorVariation and +colorVariation
        NoiseLayer = new Noise(seed);

        PlanetClouds clouds = GetComponentInChildren<PlanetClouds>();

        float planetCloudPressure = Planet.Atm.Pressure;
        bool planetTypeUsesClouds = Planet.Type.GetAtmosphericCloudUsage();


        if (clouds != null)
        {
            clouds.Seed = seed;
            clouds.SetHueShift(hueShift);
            clouds.SetCloudPressureThickness(planetCloudPressure, planetTypeUsesClouds);
        }

    }


    internal void CopyVertices()
    {
        Mesh planetSharedMesh = GetComponent<MeshFilter>().sharedMesh;
        planetMesh = Instantiate(planetSharedMesh);
        GetComponent<MeshFilter>().sharedMesh = planetMesh;
        planetVertices = planetMesh.vertices;
        planetVerticesBackup = planetMesh.vertices;
        voronoiVertices = planetMesh.vertices;

        planetColors = new Color[planetVertices.Length];
        voronoiColors = planetColors;
        voronoiColors = Enumerable.Repeat(Color.white, voronoiColors.Length).ToArray();

        Renderer surfaceRenderer = this.transform.gameObject.GetComponent<Renderer>();
        if (surfaceRenderer != null)
        {
            foreach (Material material in surfaceRenderer.sharedMaterials) // First harmonic
            {

                Debug.Log("shaders:" + material.shader.name);

                if (material.shader.name == "Shader Graphs/PlanetSurfaceShader")
                    surfaceMaterial = new Material(material);

                if (material.shader.name == "Shader Graphs/PlanetaryShadowSG")
                    shadowMaterial = new Material(material);

                //Replace with references and centralize shader management maybe 
            }

        }



    }


    public IEnumerator ShapePlanetSurface()
    {

        // vertices back to their orginal shape. 
        System.Array.Copy(planetVerticesBackup, planetVertices, planetVerticesBackup.Length);
        System.Array.Copy(planetVerticesBackup, voronoiVertices, planetVerticesBackup.Length);

        // voronoiColors back to white.
        voronoiColors = Enumerable.Repeat(Color.white, voronoiColors.Length).ToArray();

        Random = new System.Random(seed);

        UpdateVoronoiPattern(); //modifies voronoiColor and voronoiVertices (Pass by reference)   
        UpdateSurfaceNonParallel(); //modifies planetVertices
        UpdateSurfaceColors();  //modifies planetColors
        UpdateCraters();        //modifies planetVertices and planetColors, adds craters
        UpdateSea();
        // UpdateLatitudeColoring(); //modifies planetColors
        // UpdatePolarIceCaps();     //modifies planetVertices and planetColors,
        UpdateFlatness();         //modifies planetVertice,
        UpdatePlanetVertices();   //planetVertices to the mesh shape
        UpdateClouds();
        UpdateAtmosphere();
        UpdateRing();
        UpdateShadow();
        yield return null;
    }
    void UpdateVoronoiPattern()
    {
        string patternType = "";
        int cellNum = cellAmount;
        float cellAmplitude = cellPower;
        if (selectedCellPattern != CellularPattern.None)
        {
            switch (selectedCellPattern)
            {
                case CellularPattern.Crack:
                    VoronoiFunctions.UpdateCells(ref voronoiVertices, ref voronoiColors, seed, 20, 0.3f, cellularCurve, "crack");
                    break;

                case CellularPattern.CurveA:
                    patternType = "2";
                    VoronoiFunctions.UpdateCells(ref voronoiVertices, ref voronoiColors, seed, cellNum, cellAmplitude, cellularCurve, patternType);
                    break;
                case CellularPattern.CurveB:
                    patternType = "3";
                    VoronoiFunctions.UpdateCells(ref voronoiVertices, ref voronoiColors, seed, cellNum, cellAmplitude, cellularCurve, patternType);
                    break;
                case CellularPattern.CurveAB:
                    patternType = "d";
                    VoronoiFunctions.UpdateCells(ref voronoiVertices, ref voronoiColors, seed, cellNum, cellAmplitude, cellularCurve, patternType);
                    break;

                default:

                    break;
            }


            for (int vertexIndex = 0; vertexIndex < planetVertices.Length; vertexIndex++)
            {
                planetVertices[vertexIndex] = voronoiVertices[vertexIndex];
            }

        }

    }



    void UpdatePlanetVertices()
    {

        planetMesh.vertices = planetVertices;
        planetMesh.RecalculateNormals();

    }

    /*
    public Vector3 RandomVertexPosition()
    {
        Vector3[] vertices = planetVertices;
        int randomVertex = Random.Next(0, vertices.Length);
        return vertices[randomVertex] + Vector3.Normalize(vertices[randomVertex]) * 0.02f; ;
    }

    public Vector3 SuitableFeaturePosition(string locationType)
    {
        Vector3[] vertices = planetVertices;
        Vector3 suitableVertex = new Vector3(0, 0, 0);
        int randomVertexId;
        if (locationType == "Sea")
        {
            List<Vector3> seaVertices = new List<Vector3>();
            foreach (Vector3 vertex in vertices) // First harmonic
            {
                if (vertex.magnitude < seaLevel)
                    seaVertices.Add(vertex);
            }
            if (seaVertices.Count == 0)
            {
                suitableVertex = RandomVertexPosition();
            }
            else
            {
                randomVertexId = Random.Next(0, seaVertices.Count);
                suitableVertex = Vector3.Normalize(seaVertices[randomVertexId]) * seaLevel * 1.1f;
            }
        }
        if (locationType == "Land")
        {
            List<Vector3> landVertices = new List<Vector3>();
            foreach (Vector3 vertex in vertices) // First harmonic
            {
                if (vertex.magnitude > seaLevel)
                    landVertices.Add(vertex);
            }

            if (landVertices.Count == 0)
            {
                suitableVertex = RandomVertexPosition();
            }
            else
            {
                randomVertexId = Random.Next(0, landVertices.Count);
                suitableVertex = landVertices[randomVertexId];
            }
        }
        if (locationType == "Polar")
        {
            List<Vector3> polarVertices = new List<Vector3>();
            foreach (Vector3 vertex in vertices) // First harmonic
            {
                if (Mathf.Abs(vertex.y) > 0.9f)
                    polarVertices.Add(vertex);
            }

            if (polarVertices.Count == 0)
            {
                suitableVertex = RandomVertexPosition();
            }
            else
            {
                randomVertexId = Random.Next(0, polarVertices.Count);
                suitableVertex = polarVertices[randomVertexId];
            }
        }


        else
        {
            int randomVertex = Random.Next(0, vertices.Length);
            suitableVertex = vertices[randomVertex];
        }

        return suitableVertex;
    }
    */

    Vector3 SurfacePointPatterns(Vector3 point)
    {
        if (colorStripify != 1)
        {
            point.x = colorStripify * 1000 + point.x * colorStripify;
            point.z = colorStripify * 1000 + point.z * colorStripify;
        };
        return point;
    }

    float SurfaceNoisePatterns(float noise, Vector3 point, Noise noiseFilter, int level)
    {

        float patternNoise = heightCurve.Evaluate(noise);


        if (modulo != 0)
        {


            patternNoise = patternNoise - patternNoise % modulo;



        };

        if (fluidicity != 0)
        {

            float fluidX = fluidicity + (level - 1);
            float fluidY = fluidicity + 2 * (level - 1);
            point = new Vector3(point.x * patternNoise + seed, point.y * patternNoise + (seed + 10.5f), point.z * patternNoise + (seed));
            patternNoise = patternNoise + fluidicity * noiseFilter.Evaluate(point);
        };



        if (warpForce > 0)
        {
            float xDistortion = noiseFilter.Evaluate((new Vector3(point.x, point.y, point.z)));
            float yDistortion = noiseFilter.Evaluate((new Vector3(point.x + warpVector.x, point.y + warpVector.y, point.z + warpVector.z)));
            float zDistortion = noiseFilter.Evaluate((new Vector3(point.x + 0.75f * warpVector.x, point.y + 1.1f * warpVector.y, point.z + 0.79f * warpVector.z)));
            patternNoise = patternNoise * noiseFilter.Evaluate((new Vector3(point.x * xDistortion, point.y * yDistortion, point.z * zDistortion)));
        };




        return patternNoise;
    }


    void UpdatePolarIceCaps()
    {

        Color[] colors = planetMesh.colors;
        Vector3[] vertices = planetVertices;

        Vector3[] poles = new[] { new Vector3(0f, 1f, 0f), new Vector3(0f, -1f, 0f) }; //north pole and south pole;
        Vector3 point;

        float noise;
        float iceHeight = 1;
        float polarRadius = iceCapsSize * 1.8f;

        if (testpolarCoverage != 0f)
        {

            polarRadius = testpolarCoverage;
        }

        Color polarIceColor = new Color(1, 1, 1);

        foreach (Vector3 pole in poles)
        {
            for (int vertexIndex = 0; vertexIndex < vertices.Length; vertexIndex++)
            {

                float distance = Vector3.Distance(vertices[vertexIndex], pole);

                if (distance <= polarRadius)
                {

                    float rDistance = distance / polarRadius;
                    point = vertices[vertexIndex];
                    /*  noise = NoiseFunctions.PerlinFilter(point, NoiseLayer, 1.5f, 0, 1, 45);
                      noise += NoiseFunctions.PerlinFilter(point, NoiseLayer, 2f, 1, 0.8f, 899.1f);
                      noise += NoiseFunctions.PerlinFilter(point, NoiseLayer, 3f, 1, 0.5f, 2.1f);*/



                    noise = NoiseManager.Instance.SimplePerlinFilter(point, 1.5f);
                    noise += NoiseManager.Instance.SimplePerlinFilter(point, 2f);
                    noise += NoiseManager.Instance.SimplePerlinFilter(point, 3f);





                    noise += 2f;
                    noise -= 4f * (rDistance);

                    if (noise > 0)
                    {
                        iceHeight = 1;
                        if (point.magnitude < seaLevel) { iceHeight = (seaLevel / iceHeight) + 0.014f; }
                        point = new Vector3(vertices[vertexIndex].x * iceHeight, vertices[vertexIndex].y * iceHeight, vertices[vertexIndex].z * iceHeight);
                        vertices[vertexIndex] = point;
                        colors[vertexIndex] = polarIceColor;

                    }

                }
            }
        }
        planetVertices = vertices;
        planetMesh.colors = colors;


    }


    struct FirstPerlinNoiseLevel : IJobParallelFor
    {
        [ReadOnly] public float frequency;
        [ReadOnly] public float offset;
        [ReadOnly] public float seed;
        [ReadOnly] public NativeArray<Vector3> point;

        [ReadOnly] public float level;
        public NativeArray<float> resultNoise;
        Vector3 resultA;
        float noise;
        float perlin;
        public void Execute(int i)
        {
            resultA = new Vector3(point[i].x * frequency + offset, point[i].y * frequency + offset, point[i].z * frequency + offset);
            perlin = Mathf.PerlinNoise(resultA.x + 1.12f * seed, resultA.y - 4.3f * seed) * Mathf.PerlinNoise(resultA.y - 3.47f * seed, resultA.z + 1.77f * seed);
            noise = (0.25f - perlin) * 4f;
            resultNoise[i] = noise;
        }
    }

    struct DeepPerlinNoiseLevel : IJobParallelFor
    {
        [ReadOnly] public float frequency;
        [ReadOnly] public float offset;
        [ReadOnly] public float seed;
        [ReadOnly] public NativeArray<Vector3> point;

        [ReadOnly] public float level;
        public NativeArray<float> resultNoise;
        Vector3 resultA;
        float noise;
        float perlin;
        public void Execute(int i)
        {
            resultA = new Vector3(point[i].x * frequency + offset, point[i].y * frequency + offset, point[i].z * frequency + offset);
            perlin = Mathf.PerlinNoise(resultA.x + 1.12f * seed, resultA.y - 4.3f * seed) * Mathf.PerlinNoise(resultA.y - 3.47f * seed, resultA.z + 1.77f * seed);
            noise = (0.25f - perlin) * 4f;
            resultNoise[i] = noise;
        }
    }

    void UpdateSurface()
    {
        float startTime = Time.realtimeSinceStartup;
        Vector3[] vertices = planetVertices;
        float max = 0.1f;
        float min = 0.5f;
        float avg = 0f;
        NoiseLayer = new Noise(seed);

        float frequency = Frequency;
        float roughness = Roughness;
        float amplitude = Amplitude;
        float persistance = Persistence;
        float subAmplitude = Amplitude * Roughness;
        float subFrequency = Frequency;
        float noise = 0;

        Vector3 point;
        Vector3 modPoint;

        NativeArray<Vector3> surfacePoint = new NativeArray<Vector3>(vertices.Length, Allocator.TempJob);
        NativeArray<float> resultNoise = new NativeArray<float>(vertices.Length, Allocator.TempJob);

        for (int i = 0; i < vertices.Length; i++) // 1st 
        {
            surfacePoint[i] = vertices[i];
        }

        FirstPerlinNoiseLevel jobData = new FirstPerlinNoiseLevel();
        jobData.point = surfacePoint;
        jobData.frequency = frequency;
        jobData.offset = 0f;
        jobData.seed = 5.4f;
        jobData.resultNoise = resultNoise;

        // Schedule the job with one Execute per index in the results array and only 1 item per processing batch
        JobHandle handle = jobData.Schedule(vertices.Length, 1);

        // Wait for the job to complete
        handle.Complete();

        for (int i = 0; i < vertices.Length; i++) // 1st 
        {
            if (resultNoise[i] > max) { max = resultNoise[i]; };
            if (resultNoise[i] < min) { min = resultNoise[i]; }
            avg += resultNoise[i];


            noise = SurfaceNoisePatterns(resultNoise[i], surfacePoint[i], NoiseLayer, 1);
            noise = 1f + (noise * subAmplitude);
            surfacePoint[i] = surfacePoint[i] * noise;

        }

        avg = avg / resultNoise.Length;

        for (int currentLevel = 2; currentLevel <= levels; currentLevel++) // Higher harmonics
        {
            subAmplitude = (subAmplitude - (subAmplitude * roughness)) + ((Mathf.Pow(1 - roughness, levels) * Amplitude) / levels);
            subFrequency = Frequency * Mathf.Pow(currentLevel, Persistence);

            DeepPerlinNoiseLevel deepHarmonic = new DeepPerlinNoiseLevel();
            deepHarmonic.point = surfacePoint;
            deepHarmonic.frequency = subFrequency;
            deepHarmonic.level = currentLevel;
            deepHarmonic.seed = 2.5f;
            deepHarmonic.offset = 0f;
            deepHarmonic.resultNoise = resultNoise;

            JobHandle handle2 = deepHarmonic.Schedule(vertices.Length, 1);

            // Wait for the job to complete
            handle2.Complete();

            for (int i = 0; i < vertices.Length; i++) // 1st 
            {
                point = surfacePoint[i];
                noise = SurfaceNoisePatterns(noise, point, NoiseLayer, 1);
                noise = 1 + (noise * subAmplitude);
                vertices[i] = point * noise;

            }
        }


        for (int i = 0; i < vertices.Length; i++)
        {
            point = vertices[i];

            if (point.magnitude < negativePeakClip)
            {
                point = planetVerticesBackup[i];
            }

            vertices[i] = point;
        }

        surfacePoint.Dispose();
        resultNoise.Dispose();


        for (int i = 0; i < vertices.Length; i++)
        {
            planetVertices[i] = vertices[i];
        }

        float lastedTime = startTime - Time.realtimeSinceStartup;
        Debug.Log(" parallel: " + lastedTime);



    }


    Vector3 RandomVector()
    {
        float randomX = ((float)Random.NextDouble() - 0.5f) * 2f;
        float randomY = ((float)Random.NextDouble() - 0.5f) * 2f;
        float randomZ = ((float)Random.NextDouble() - 0.5f) * 2f;
        return new Vector3(randomX, randomY, randomZ);
    }
    void UpdateSurfaceNonParallel()
    {
        float startTime = Time.realtimeSinceStartup;
        Vector3[] vertices = planetVertices;

        NoiseLayer = new Noise(seed);

        float frequency = Frequency;
        float roughness = Roughness;
        float amplitude = Amplitude;
        float persistance = Persistence;
        float subAmplitude = Amplitude * Roughness;
        float subFrequency = Frequency;
        float noise = 0;

        highestHeight = 0;
        LowestHeight = 999;


        Vector3 point;
        Vector3 modPoint;

        float max = 0.5f;
        float min = 2.5f; ;
        float avg = 0.0f; ;


        Vector3 randomVector = RandomVector();
        for (int i = 0; i < vertices.Length; i++) // 1st 
        {

            point = vertices[i];
           modPoint = SurfacePointPatterns(point);

            noise = NoiseManager.Instance.SimplePerlinFilter(modPoint + randomVector, frequency);

            if (noise > max) { max = noise; };
            if (noise < min) { min = noise; }
            avg += noise;

            //  noise = noise * SurfaceNoisePatterns(noise, point, NoiseLayer, 1);
            //  noise =  ((noise-0.5f)) * subAmplitude;
            noise = noise * amplitude;
            point *= (1 + noise);
            vertices[i] = point;
        }


        avg = avg / vertices.Length;
        Debug.Log("Average: " + avg);
        for (int currentLevel = 2; currentLevel <= levels; currentLevel++) // Higher harmonics
        {
            randomVector = RandomVector();
            //subAmplitude = (subAmplitude - (subAmplitude * roughness)) + ((Mathf.Pow(1 - roughness, levels) * Amplitude) / levels);
            subAmplitude = (subAmplitude - (subAmplitude * roughness)) + ((Mathf.Pow(1 - roughness, levels) * Amplitude) / levels);
            subFrequency = Frequency * Mathf.Pow(currentLevel, Persistence);

            for (int i = 0; i < vertices.Length; i++)
            {
                point = vertices[i];

                modPoint = SurfacePointPatterns(point);

                noise = NoiseManager.Instance.SimplePerlinFilter(modPoint + randomVector, subFrequency);

                noise = SurfaceNoisePatterns(noise, modPoint + randomVector, NoiseLayer, currentLevel);
                noise = ((noise - 0.5f) * subAmplitude);
                point *= 1 + noise;


                vertices[i] = point;
            }
        }





        float surfaceHeight = 0;

        for (int i = 0; i < vertices.Length; i++)
        {
            point = vertices[i];
            surfaceHeight = point.magnitude;
            if (highestHeight < surfaceHeight)
            {
                highestHeight = surfaceHeight;
            }
            if (LowestHeight > surfaceHeight)
            {
                LowestHeight = surfaceHeight;
            }

            if (surfaceHeight < negativePeakClip)
            {
                point = planetVerticesBackup[i];
            }

            vertices[i] = point;
        }




        for (int i = 0; i < vertices.Length; i++)
        {
            //planetVertices[i] = vertices[i];
        }



    }

    void UpdateFlatness()
    {
        if (Flatness > 0)
        {
            float flatness = Flatness;

            for (int i = 0; i < planetVertices.Length; i++)
            {
                planetVertices[i] = planetVertices[i] * (1 - flatness) + flatness * planetVerticesBackup[i];
            }
        }

    }

    void Update()
    {
        transform.RotateAround(gameObject.transform.position, Vector3.up, rotationSpeed * Time.deltaTime);
    }

    void UpdateSurfaceColors()
    {
        Vector3[] vertices = planetVertices;

        Vector3 point;


        float lowestDryLandLevel = 000f;
        for (int i = 0; i < vertices.Length; i++)
        {
            point = vertices[i];

            lowestDryLandLevel = Mathf.Max(LowestHeight, seaLevel);

            float relativeDryLandHeight = Mathf.InverseLerp(lowestDryLandLevel, highestHeight, point.magnitude);

            float relativeSeaDepth = Mathf.InverseLerp(LowestHeight, seaLevel, point.magnitude);

            planetColors[i] = colorGradient.Evaluate(relativeDryLandHeight);

            if (point.magnitude <= seaLevel)
            {
                planetColors[i].a = relativeSeaDepth;
            }




        }


        planetMesh.colors = planetColors;

    }

    public Gradient GetColorGradient()
    {
        return colorGradient;
    }

    /*
    void UpdateSurfaceColors()
    {
        Vector3[] vertices = planetVertices;

        Vector3 point;
        float colorNoise = 0;

        Noise noiseLayer = new Noise(seed + 4512);

        HueShiftColors();

        Color highGround = colorHighShifted;
        Color middleGround = colorMidShifted;
        Color lowGround = colorLowShifted;
        Color equatorialColor;
        Color weirdColor;

        for (int i = 0; i < vertices.Length; i++)
        {
            point = vertices[i];

           colorNoise = (NoiseFunctions.PerlinFilter(point, noiseLayer, 1.7f, 1, 1f, 22 + seed));

 
            float height = point.magnitude;
            float heighSpectrum = 0.5f;
            float y = point.y;


            //Elevation based coloring
            if (height > colorHeightHigh)
            {
                planetColors[i] = highGround;
            }
            else if (height <= colorHeightHigh && height > colorHeightMid)
            {
                heighSpectrum = (height - colorHeightMid) / (colorHeightHigh - colorHeightMid);
                planetColors[i] = Color.Lerp(middleGround, highGround, heighSpectrum);
                planetColors[i].a = 0;
            }
            else if (height <= colorHeightMid && height > colorHeightLow)
            {
                planetColors[i] = middleGround;
            }
            else if (height <= colorHeightLow && height > colorHeightDeep)
            {
                heighSpectrum = (height - colorHeightDeep) / (colorHeightLow - colorHeightDeep);
                planetColors[i] = Color.Lerp(lowGround, middleGround, heighSpectrum);
            }
            else if (height <= colorHeightDeep)
            {
                planetColors[i] = lowGround;
            }


            if (weird != 0 )
            {
                float wierdHue = NoiseFunctions.PerlinFilter(point, NoiseLayer, 0.4f, 1, 0.4f* weird, 4 * seed);
                wierdHue = wierdHue + NoiseFunctions.PerlinFilter(point, NoiseLayer, 0.8f, 1, 0.22f * weird, 55 * seed);


                float wierdSaturation =(NoiseFunctions.PerlinFilter(point, NoiseLayer, 0.5f, 1, 0.15f * weird, 4.1f * seed));
                wierdSaturation = wierdSaturation + (NoiseFunctions.PerlinFilter(point, NoiseLayer, 1f, 1, 0.15f * weird, 55.2f * seed));

                float wierdValue = (NoiseFunctions.PerlinFilter(point, NoiseLayer, 0.5f, 1, 0.15f * weird, 4.3f * seed));
                wierdValue = wierdValue + (NoiseFunctions.PerlinFilter(point, NoiseLayer, 1f, 1, 0.05f * weird, 55.3f * seed));
                wierdValue = Mathf.Clamp01(wierdValue);


                weirdColor = ColorFunctions.HueShiftColor(planetColors[i], wierdHue, 1);
                weirdColor = ColorFunctions.SaturationShiftColor(weirdColor, wierdSaturation, 1);
                weirdColor = ColorFunctions.SaturationShiftColor(weirdColor, wierdValue, 1);
                planetColors[i] = weirdColor;

            };



            if (equatorial != 0)
            {
                float equatorialGradient = Mathf.Cos(2f * point.y * point.y) ;
                equatorialColor = ColorFunctions.HueShiftColor(planetColors[i], -0.1f * equatorial, 1);
                equatorialColor = ColorFunctions.SaturationShiftColor(equatorialColor, -0.1f * equatorial, 1);
                equatorialColor = ColorFunctions.ValueShiftColor(equatorialColor, 0.1f * equatorial, 1);
                planetColors[i] = Color.Lerp(planetColors[i], equatorialColor, equatorialGradient);

            }

            if (desertification != 0)
            {

                float desertNoise;
                desertNoise = NoiseFunctions.PerlinFilter(point, NoiseLayer, 0.7f,1, 1,0);
                desertNoise += NoiseFunctions.PerlinFilter(point, NoiseLayer, 1.3f, 1, 0.5f , seed + 672);
                desertNoise += NoiseFunctions.PerlinFilter(point, NoiseLayer, 1.3f, 1, 0.5f, seed + 172);
                desertNoise = Mathf.Abs(desertNoise);

                desertNoise = desertNoise * Mathf.Cos(3 * point.y * point.y) * desertification  * desertNoise;
                desertNoise = Mathf.Clamp(desertNoise, 0, 1);

                planetColors[i] = Color.Lerp(planetColors[i], colorDesertShifted, desertNoise);
            }




            if (voronoiColors != null)
                if (pebbles != 0)
                {
                    float pebbleValue = NoiseFunctions.PerlinFilter(point, NoiseLayer, pebbles, 1, pebbles, seed + 5);
                     pebbleValue += NoiseFunctions.PerlinFilter(point, NoiseLayer, 0.75f * pebbles, 1, pebbles, seed + 15);
                     pebbleValue += NoiseFunctions.PerlinFilter(point, NoiseLayer, 2f * pebbles, 1, pebbles, seed + 25);

                    //    planetColors[i] = ColorFunctions.ValueShiftColor(planetColors[i], 0, 0.5f + 0.5f * pebbleValue);
                    //    planetColors[i] = ColorFunctions.HueShiftColor(planetColors[i], 0, 1 + 0.2f * (((float)Random.NextDouble() - 0.5f) * 2));
                    planetColors[i] = Color.Lerp(planetColors[i], voronoiColors[i], pebbleValue);


                } else
                {
                    planetColors[i] = planetColors[i] * voronoiColors[i]; // voronoiColors comes from the voronoi pattern namespaces
                }
        }


        planetMesh.colors = planetColors;

    }*/

    void UpdateLatitudeColoring()
    {

        Color[] colors = planetMesh.colors;
        Vector3[] vertices = planetVertices;

        Vector3[] poles = new[] { new Vector3(0f, 1f, 0f), new Vector3(0f, -1f, 0f) }; //north pole and south pole;
        Vector3 point;
        Color latitudeColor;

        float noise;
        float distance;
        float rDistance;
        float taigaRadius = iceCapsSize * 2.5f;


        foreach (Vector3 pole in poles)
        {
            for (int vertexIndex = 0; vertexIndex < vertices.Length; vertexIndex++)
            {

                distance = Vector3.Distance(vertices[vertexIndex], pole);
                rDistance = 0;
                latitudeColor = colors[vertexIndex];


                if (distance <= taigaRadius)
                {

                    rDistance = distance / taigaRadius;
                    point = vertices[vertexIndex];

                    noise = 0.5f;

                    /* noise += Mathf.Abs(NoiseFunctions.SimplePerlinFilter(point,  1.5f));
                     noise -= Mathf.Abs(NoiseFunctions.PerlinFilter(point, 1.5f));
                     noise += NoiseFunctions.PerlinFilter(point, 2.5f);*/


                    noise += Mathf.Abs(NoiseManager.Instance.SimplePerlinFilter(point, 1.5f));
                    noise -= Mathf.Abs(NoiseManager.Instance.SimplePerlinFilter(point, 1.5f));
                    noise += NoiseManager.Instance.SimplePerlinFilter(point, 2.5f);


                    noise -= 1 * (rDistance * rDistance * rDistance);

                    if (noise > 0.35f)
                    {
                        latitudeColor = ColorFunctions.SaturationShiftColor(latitudeColor, 0, 0.3f);
                        latitudeColor = ColorFunctions.ValueShiftColor(latitudeColor, 0, 1.3f);
                        colors[vertexIndex] = latitudeColor;
                    }

                    else if (noise > 0)
                    {
                        latitudeColor = ColorFunctions.SaturationShiftColor(latitudeColor, 0, 0.6f);
                        latitudeColor = ColorFunctions.HueShiftColor(latitudeColor, 0.06f, 1);
                        colors[vertexIndex] = latitudeColor;
                    }



                }

            }
        }
        planetMesh.colors = colors;
    }


    void UpdateCraters()
    {
        Random = new System.Random(seed);
        Vector3[] vertices = planetVertices;
        Vector3[] backupVertices = planetVerticesBackup;

        int craterNumber = 0;
        float craterAmplitude = 1;
        float craterSize = 0.15f;
        craterNumber = (int)(MathFunctions.StandardDeviation(craterAmount, 0.8f * craterAmount, seed));
        craterNumber = Mathf.Clamp(craterAmount, 0, craterAmount * 5);


        for (int i = 0; i < craterNumber; i++)
        {
            int craterId = Random.Next(0, vertices.Length);
            Vector3 craterPos = vertices[craterId];
            Vector3 smoothedVertex;


            List<Vector3> craterVertices = new List<Vector3>();


            float randomCraterSize = Mathf.Clamp(Mathf.Abs(MathFunctions.StandardDeviation(craterSize, 0.6f * craterSize, seed + i)), 0.3f * craterSize, 3f * craterSize);

            float rDistance = 0; //a relative distance from the crater midpoint
            float rDepth = 0;  //a relative depth of the crater midpoint
            for (int j = 0; j < vertices.Length; j++)
            {
                float distance = Vector3.Distance(vertices[j], craterPos); //a point distance from the crater midpoint

                if (distance <= randomCraterSize)
                {

                    rDistance = distance / randomCraterSize;

                    rDepth = -0.5f * Mathf.Pow(-Mathf.Pow(rDistance, 2) + 1.4f, 0.5f) + 0.32f;
                    rDepth *= randomCraterSize;
                    planetColors[j] = colorGradient.Evaluate(0.3f * rDepth);

                    smoothedVertex = Vector3.Lerp(backupVertices[j], vertices[j], rDistance);

                    vertices[j] = smoothedVertex * (1 + (rDepth * craterAmplitude));
                }
            }


        }


        planetVertices = vertices;
        planetMesh.colors = planetColors;

    }

    /*void UpdateCraters()

    {
        Random = new System.Random(seed);
        Vector3[] vertices = planetVertices;

        int craterNumber = 0;

        craterNumber = (int)(MathFunctions.StandardDeviation(craterAmount, 0.8f * craterAmount, seed));
        craterNumber = Mathf.Clamp(craterAmount, 0, craterAmount * 5);


        for (int i = 0; i < craterNumber; i++)
        {
            int craterId = Random.Next(0, vertices.Length);
            Vector3 craterPos = vertices[craterId];
            Color craterColor = ColorFunctions.SaturationShiftColor(colorLowShifted, 0, 0.85f);
            craterColor = ColorFunctions.ValueShiftColor(colorLowShifted, 0, 0.81f);

            List<Vector3> craterVertices = new List<Vector3>();


            float randomCraterSize = Mathf.Clamp(Mathf.Abs(MathFunctions.StandardDeviation(craterSize, 0.6f * craterSize, seed+i)), 0.3f * craterSize, 3f * craterSize);

            for (int j = 0; j < vertices.Length; j++)
            {

                float distance = Vector3.Distance(vertices[j], craterPos);

                if (distance <= randomCraterSize)
                {

                    float rDistance = distance / randomCraterSize;

                    float rDepth = 0;
                    planetColors[j] = 0.4f * planetColors[j] + 0.6f * craterColor;
                    rDepth = -0.5f * Mathf.Pow(-Mathf.Pow(rDistance, 2) + 1.4f, 0.5f) + 0.32f;
                    vertices[j] = vertices[j] * (1 + (rDepth * craterAmplitude));
                }
            }


        }


        planetVertices = vertices;
        planetMesh.colors = planetColors;

    }
    */





}

