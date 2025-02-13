using System.Collections;
using System.Collections.Generic;
using UnityEngine;


using ColorSpace;
using static CelestialBody;

public class ClusterController : MonoBehaviour
{
    public GameObject StarPrefab;

    GameObject Deepfield;

    ColorFunctions ColorFunctions = new ColorFunctions();

    List<GameObject> StarObjects = new List<GameObject>() { };

    void Awake()
    {
        Deepfield = this.transform.Find("BackgroundSpace/BgDeepfield").gameObject;
      //  UI = GameObject.Find("/Canvas").GetComponent<UiCanvas>();

    }


    private static ClusterController _instance;
    private static ClusterController Instance
    {
        get
        {
            if (_instance == null)
            {
                // If the instance is null, try to find it in the scene
                _instance = FindObjectOfType<ClusterController>();

                if (_instance == null)
                {
                    Debug.LogError("ClusterController NULL, CANT BE FOUND");
                }
            }

            return _instance;
        }
    }

    public  static ClusterController GetInstance()
    {
        return Instance;
    }

    public List<GameObject> ListStarObjects()
    {
        List<GameObject> starObjects = new();
        foreach (Transform child in gameObject.transform)
        {
            if (child.GetComponent<ClusterStar>() != null)
            {
                starObjects.Add(child.gameObject);
            }
        }

        return starObjects;
    }

    //only for currently existing gameObjects 
    public GameObject FindStarGameObject(int starId)
    {
        GameObject star = null;
        foreach (Transform child in gameObject.transform)
        {
            if (child.GetComponent<ClusterStar>())
            {
                int id = child.GetComponent<ClusterStar>().Star.Id;

                if (id == starId)
                {
                    star = child.gameObject;
                    break;
                }

            }
        }

        return star;
    }

    void OnDisable()
    {
        foreach (Transform child in gameObject.transform)
        {
            if (child.gameObject.tag == "Cluster")
            {
                Destroy(child.gameObject);
            }
            else if (child.gameObject.tag == "System")
            {
                Destroy(child.gameObject);
            }
        }
    }

    public void VisualizeCluster(Cluster activeCluster)
    {

        StarObjects.Clear();


        foreach (Star star in activeCluster.Stars)
        {

            GameObject clusterStar = Instantiate(StarPrefab, star.Pos, transform.rotation) as GameObject;
            clusterStar.GetComponent<ClusterStar>().SetStar(star);
            clusterStar.GetComponent<ClusterStar>().SetClusterStarColor();
            clusterStar.GetComponent<ClusterStar>().AddZLine();
            clusterStar.transform.parent = this.transform;
            StarObjects.Add(clusterStar);

        }

        ColorizeCluster(activeCluster.Id);

        UiCanvas.GetInstance().ClusterDataView(activeCluster.Name + " " + activeCluster.Id);

        //  UI.ClusterDataView(activeCluster.Name);
        // StartCoroutine(SkyboxController.SetSkybox("cluster"));
    }

    void ColorizeCluster(int seed)
    {

        System.Random Random = new System.Random(seed);

        Color PrimaryColor = new Color(0.0f, 0.0f, 0f, 1f);
        Color SecondaryColor = new Color(0.0f, 0.0f, 0f, 1f);


        float primaryColorHue;
        float primaryColorSaturation;
        float primaryColorValue;

        float secondaryColorHue;
        float secondaryColorSaturation;
        float secondaryColorValue;

        primaryColorHue = (float)Random.NextDouble();
        primaryColorSaturation = (float)Random.NextDouble() * 0.3f + 0.7f;
        primaryColorValue = (float)Random.NextDouble() * 0.3f + 0.7f;

        secondaryColorHue = primaryColorHue + ((float)Random.NextDouble() * 0.5f - 0.25f);
        secondaryColorSaturation = (float)Random.NextDouble() * 0.5f + 0.5f;
        secondaryColorValue = (float)Random.NextDouble() * 0.5f + 0.5f;




        PrimaryColor = Color.HSVToRGB(primaryColorHue, primaryColorSaturation, primaryColorValue);
        SecondaryColor = Color.HSVToRGB(secondaryColorHue, secondaryColorSaturation, secondaryColorValue);

        Deepfield = this.transform.Find("BackgroundSpace/BgDeepfield").gameObject;

        if (Deepfield.GetComponent<ParticleSystem>() != null)
        {
            ParticleSystem.MainModule main = Deepfield.GetComponent<ParticleSystem>().main;

            main.startColor = new ParticleSystem.MinMaxGradient(PrimaryColor, SecondaryColor);

        }
    }

}
