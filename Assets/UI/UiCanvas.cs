using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using static CelestialBody;

public  class UiCanvas : MonoBehaviour
{
    GameObject SystemCamera;
    CameraOrbit CameraOrbit;
    GameObject Selector;

    private static UiCanvas _instance;
    private static UiCanvas Instance
    {
        get
        {
            if (_instance == null)
            {
                // If the instance is null, try to find it in the scene
                _instance = FindObjectOfType<UiCanvas>();

                // If still null, make an error
                if (_instance == null)
                {
                    Debug.LogError("UICANVAS NULL, CANT BE FOUND");
                }
            }

            return _instance;
        }
    }
    public static UiCanvas GetInstance()
    {
        return Instance;
    }

    public Material UIMainMaterial;
    public Material UICautionMaterial;
    public Material UIWarningMaterial;

    public GameObject PopUpPanel;

    GameObject UIDescriptor;
    GameObject UIDownPanel;
    GameObject UILeftPanel;
    GameObject UISelector;
    GameObject UIZoomOutButton;
    GameObject UILocateSolButton;
    GameObject UIOpenSocietyButton;
    GameObject UINameTrackers;
    GameObject FullScreenLightEffect;

    void Awake()
    {

        SystemCamera = GameObject.Find("/Camera/ClusterCameraSystem/CameraTarget/System Camera");
        CameraOrbit = GameObject.Find("/Camera/ClusterCameraSystem/CameraTarget/").GetComponent<CameraOrbit>();
        UIDescriptor = this.transform.Find("Descriptor").gameObject;
        UIDownPanel = this.transform.Find("DownPanel").gameObject;
        UILeftPanel = this.transform.Find("LeftPanel").gameObject;
        UISelector = this.transform.Find("Selector").gameObject;
        UIZoomOutButton = this.transform.Find("ZoomOutButton").gameObject;
        UILocateSolButton = this.transform.Find("LocateSolButton").gameObject;
        UIOpenSocietyButton = this.transform.Find("SocietyButton").gameObject;
        UINameTrackers = this.transform.Find("UINameTrackers").gameObject;
        FullScreenLightEffect = this.transform.Find("FullScreenLightEffect").gameObject;

        



    }
    void Start()
    {
        GalaxyCatalog GalaxyCatalog = GameObject.Find("/Galaxy").GetComponent<GalaxyCatalog>();
    }





    public void HideAllElements()
    {
  
        foreach (Transform child in gameObject.transform)
        {   if (child.CompareTag("UIElement"))
            child.gameObject.SetActive(false);
        }
    }
    enum UiScope
    {
        Galaxy,
        Cluster,
        System,
        Star,
        Planet,
        Society
    }
    void ShowUi(UiScope uiScope)
    {
        HideAllElements();

        // Default elements to activate for most scopes
        UIDescriptor.SetActive(true);
        UIDownPanel.SetActive(true);
        UILocateSolButton.SetActive(true);

        switch (uiScope)
        {
            case UiScope.Galaxy:
                break;

            case UiScope.Cluster:
            case UiScope.System:
                UIZoomOutButton.SetActive(true);
                UINameTrackers.SetActive(true);
                FullScreenLightEffect.SetActive(true);
                break;
            case UiScope.Star:
                UIZoomOutButton.SetActive(true);
                UINameTrackers.SetActive(true);
                break;

            case UiScope.Planet:
                UIZoomOutButton.SetActive(true);
                UINameTrackers.SetActive(true);
                UILeftPanel.SetActive(true);
                UIOpenSocietyButton.SetActive(true);
                FullScreenLightEffect.SetActive(true);
                break;

            case UiScope.Society:
                UIDownPanel.SetActive(false);
                UILeftPanel.SetActive(false);
                UISelector.SetActive(false);
                UIZoomOutButton.SetActive(true);
                break;

            default:
                Debug.LogWarning($"Unknown UI scope: {uiScope}");
                break;
        }
    }

    public void SocietytDataView(string name)
    {
        ShowUi(UiScope.Society);
        UpdateDescriptor(name, "tests");

        CameraOrbit.CameraTo2D();
        CameraOrbit.CameraDisabled = true;
    }

    public void ClusterDataView(string name)
    {
        ShowUi(UiScope.Cluster);
        UpdateDescriptor(name, "");
        Vector3 targetPos = new Vector3(0, 0, 0);
        CameraOrbit.CameraTo3D();
        CameraOrbit.CameraToPos(targetPos);
        UIClusterNames.GetInstance().CreateClusterNameTags();

    }


    public void SystemDataView(string name)
    {
        ShowUi(UiScope.System);

        UpdateDescriptor(name, "");
        Vector3 targetPos = new Vector3(0, 0, 0);
        CameraOrbit.CameraTo3D();
        CameraOrbit.CameraToPos(targetPos);
        UIClusterNames.GetInstance().CreateSystemNameTags();

    }

    public void GalaxyDataView(string name)
    {
        ShowUi(UiScope.Galaxy);
        UpdateDescriptor(name, "");
        Vector3 targetPos = new Vector3(0, 0, 0);
        CameraOrbit.CameraTo3D();
        CameraOrbit.CameraToPos(targetPos);

    }
    public void StarDataView(Transform targetTransform, Star star)
    {
        ShowUi(UiScope.Star);
        UpdateDescriptor(star.Name, star.Type.Name);
        float targetSize = 2;
        Vector3 targetPos = new Vector3(0, 0, 0);
        CameraOrbit.CameraToPos(targetPos);
        CameraOrbit.CameraTo3D();
        Tracker(true, targetTransform, targetSize);
        UIClusterNames.GetInstance().CreateSystemNameTags();
    }

    public void PlanetDataView(Transform targetTransform, Planet planet)
    {

        ShowUi(UiScope.Planet);

        UpdateDescriptor(planet.Name, planet.Type.Name + " World");

        UILeftPanel.GetComponent<UILeftPanel>().UpdatePlanetConditionData(planet);
            
        float targetSize = planet.Mass;
        
        CameraOrbit.CameraFollowTransform(targetTransform);
        CameraOrbit.CameraTo3D();
        UIClusterNames.GetInstance().CreateSystemNameTags();
        //Target tracking starts after the camera and the screen is at the right place. 
        StartCoroutine(ResumeTargetTracking(targetTransform, targetSize));

    }

    IEnumerator ResumeTargetTracking(Transform targetTransform, float targetSize)
    {
        yield return null;
        yield return null;
        yield return null;
        yield return null;
        Tracker(true, targetTransform, targetSize); 
    }

    public void UpdateDescriptor(string name, string type)
    {
        Text UIName = UIDescriptor.transform.Find("UITitle/UIName").GetComponent<UnityEngine.UI.Text>();
        Text UIType = UIDescriptor.transform.Find("UITitle/UIType").GetComponent<UnityEngine.UI.Text>();

        string nameText = name.ToString();
        string typeName = type.ToString();

        UIName.text = nameText;
        UIType.text = typeName;
    }





    void Tracker(bool status, Transform targetTransform, float targetSize)
    {

        UISelector selector = UISelector.GetComponent<UISelector>();
        selector.Camera = CameraOrbit.GetInstance().SystemCamera;
        selector.TargetTransform = targetTransform;
        selector.TargetSize = targetSize;
        UISelector.SetActive(true);

    }



 }

