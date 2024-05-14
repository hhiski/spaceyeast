using UnityEngine;

public class QuadRandom : MonoBehaviour
{

    [Range(2, 512)]
    public int resolution = 256;

    public float frequency = 1f;
    public float erosion = 1f;

    public Noise NoiseLayerAs = new Noise(7);
    private Texture2D texture;

    private void OnEnable()
    {
        if (texture == null)
        {
            texture = new Texture2D(resolution, resolution, TextureFormat.RGB24, true);
            texture.name = "Procedural Texture";
            texture.wrapMode = TextureWrapMode.Clamp;
            texture.filterMode = FilterMode.Trilinear;
            texture.anisoLevel = 9;
            GetComponent<MeshRenderer>().material.mainTexture = texture;
        }
        FillTexture();
    }

    private void Update()
    {
        FillTexture();
    }

    private static int[] hash = {
        151,160,137,91,90,15,             
    131,13,201,95,96,53,194,233,7,225,140,36,103,30,69,142,8,99,37,240,21,10,23,    
    190, 6,148,247,120,234,75,0,26,197,62,94,252,219,203,117,35,11,32,57,177,33,
    88,237,149,56,87,174,20,125,136,171,168, 68,175,74,165,71,134,139,48,27,166,
    77,146,158,231,83,111,229,122,60,211,133,230,220,105,92,41,55,46,245,40,244,
    102,143,54, 65,25,63,161, 1,216,80,73,209,76,132,187,208, 89,18,169,200,196,
    135,130,116,188,159,86,164,100,109,198,173,186, 3,64,52,217,226,250,124,123,
    5,202,38,147,118,126,255,82,85,212,207,206,59,227,47,16,58,17,182,189,28,42,
    223,183,170,213,119,248,152, 2,44,154,163, 70,221,153,101,155,167, 43,172,9,
    129,22,39,253, 19,98,108,110,79,113,224,232,178,185, 112,104,218,246,97,228,
    251,34,242,193,238,210,144,12,191,179,162,241, 81,51,145,235,249,14,239,107,
    49,192,214, 31,181,199,106,157,184, 84,204,176,115,121,50,45,127, 4,150,254,
    138,236,205,93,222,114,67,29,24,72,243,141,128,195,78,66,215,61,156,180,

        151,160,137, 91, 90, 15,131, 13,201, 95, 96, 53,194,233,  7,225,
        140, 36,103, 30, 69,142,  8, 99, 37,240, 21, 10, 23,190,  6,148,
        247,120,234, 75,  0, 26,197, 62, 94,252,219,203,117, 35, 11, 32,
         57,177, 33, 88,237,149, 56, 87,174, 20,125,136,171,168, 68,175,
         74,165, 71,134,139, 48, 27,166, 77,146,158,231, 83,111,229,122,
         60,211,133,230,220,105, 92, 41, 55, 46,245, 40,244,102,143, 54,
         65, 25, 63,161,  1,216, 80, 73,209, 76,132,187,208, 89, 18,169,
        200,196,135,130,116,188,159, 86,164,100,109,198,173,186,  3, 64,
         52,217,226,250,124,123,  5,202, 38,147,118,126,255, 82, 85,212,
        207,206, 59,227, 47, 16, 58, 17,182,189, 28, 42,223,183,170,213,
        119,248,152,  2, 44,154,163, 70,221,153,101,155,167, 43,172,  9,
        129, 22, 39,253, 19, 98,108,110, 79,113,224,232,178,185,112,104,
        218,246, 97,228,251, 34,242,193,238,210,144, 12,191,179,162,241,
         81, 51,145,235,249, 14,239,107, 49,192,214, 31,181,199,106,157,
        184, 84,204,176,115,121, 50, 45,127,  4,150,254,138,236,205, 93,
        222,114, 67, 29, 24, 72,243,141,128,195, 78, 66,215, 61,156,180
    };
    private const int hashMask = 255;



    public void FillTexture()
    {
        if (texture.width != resolution)
        {
            texture.Reinitialize(resolution, resolution);
        }

        Vector3 point00 = transform.TransformPoint(new Vector3(-0.5f, -0.5f));
        Vector3 point10 = transform.TransformPoint(new Vector3(0.5f, -0.5f));
        Vector3 point01 = transform.TransformPoint(new Vector3(-0.5f, 0.5f));
        Vector3 point11 = transform.TransformPoint(new Vector3(0.5f, 0.5f));

        float stepSize = 1f / resolution;
        for (int y = 0; y < resolution; y++)
        {
            Vector3 point0 = Vector3.Lerp(point00, point01, (y + 0.5f) * stepSize);
            Vector3 point1 = Vector3.Lerp(point10, point11, (y + 0.5f) * stepSize);
            for (int x = 0; x < resolution; x++)
            {
                Vector3 point = Vector3.Lerp(point0, point1, (x + 0.5f) * stepSize);
                texture.SetPixel(x, y, Color.white * pfilter(point, frequency));
            }
        }
        texture.Apply();
    }


    float NoiseValueHash(Vector3 point, float freq)
    {

        Vector3 orginalPoint = point;
        Vector3 normal = point.normalized;
        float Noise = 1f;

        float u = (normal.x * freq);
        float v = (normal.y * freq);
        float n = (normal.z * freq);

         Noise = Mathf.PerlinNoise(u, v);

        point *= freq;
        int ix = Mathf.FloorToInt(point.x);
        int iy = Mathf.FloorToInt(point.y);
        int iz = Mathf.FloorToInt(point.z);
        ix &= hashMask;
        iy &= hashMask;
        iz &= hashMask;

        return hash[hash[hash[ix] + iy] + iz] * (1f / hashMask);


        //  return Noise;
    }
    

    

    float pfilter(Vector3 point, float period)
{
  
    float noise = ((1 + NoiseLayerAs.Evaluate(point * period)) * 0.5f);

        float EFNoise = noise;
        if (erosion < 0.999f)
        {
            EFNoise = -1 * Mathf.Pow(1 - Mathf.Pow(noise, 4), 0.5f) + 1;
            noise = (noise * erosion) + (EFNoise * (1 - erosion));
        };
        //sharp f: y = 
        //sharp y = sqrt(1 - (x - 1) ^ (2))

      //  noise = (1 + noise);

        return noise;
    }

    float NoiseValue(Vector3 point, float freq)
    {

        Vector3 orginalPoint = point;
        Vector3 normal = point.normalized;
        float Noise = 1f;

        float u = (point.x * freq)+9033;
        float v =  (point.y * freq)+5000;
        float n = (point.z * freq)+25070;



        Noise = Mathf.PerlinNoise(u, v);
        //  Noise = -Mathf.Pow(Noise, 3) + Mathf.Tan(Noise);
    //    Noise = Mathf.Sin(Mathf.PI * 0.5f * Noise) ;

        return Noise;
    }
}