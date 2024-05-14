using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using LineSpace;

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
    GameObject UIClusterNameTrackers;


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

        //Cluster-level
        UIClusterNameTrackers = this.transform.Find("UIClusterNameTrackers").gameObject;

    }
    void Start()
    {
        GalaxyCatalog GalaxyCatalog = GameObject.Find("/Galaxy").GetComponent<GalaxyCatalog>();
    }





    public void HideAllElements()
    {
  
        foreach (Transform child in gameObject.transform)
        {
            child.gameObject.SetActive(false);
        }
    }

    void ShowUi(string uiScope)
    {
        HideAllElements();

        if (uiScope == "galaxy")
        {
            UIDescriptor.SetActive(true);
            UIDownPanel.SetActive(true);
            UILocateSolButton.SetActive(true);

        }
        else if (uiScope == "cluster")
        {
            UIDescriptor.SetActive(true);
            UIDownPanel.SetActive(true);
            UILocateSolButton.SetActive(true);
            UIZoomOutButton.SetActive(true);
            UIClusterNameTrackers.SetActive(true);
        }
        else if(uiScope == "star")
        {
            UIDescriptor.SetActive(true);
            UIDownPanel.SetActive(true);
            UILocateSolButton.SetActive(true);
            UIZoomOutButton.SetActive(true);
        }
        else if(uiScope == "planet")
        {
            UIDescriptor.SetActive(true);
            UIDownPanel.SetActive(true);
            UILeftPanel.SetActive(true);
            UIDownPanel.SetActive(true);
            UILocateSolButton.SetActive(true);
            UIZoomOutButton.SetActive(true);
            UIOpenSocietyButton.SetActive(true);
        }
        else if (uiScope == "society")
        {
            UIDescriptor.SetActive(true);
            UIDownPanel.SetActive(false);
            UILeftPanel.SetActive(false);
            UIDownPanel.SetActive(false);
            UISelector.SetActive(false);
            UILocateSolButton.SetActive(true);
            UIZoomOutButton.SetActive(true);
        }

    }

    public void SocietytDataView(string name)
    {
        ShowUi("society");
        UpdateDescriptor(name, "tests");

        CameraOrbit.CameraTo2D();
        CameraOrbit.CameraDisabled = true;
    }

    public void ClusterDataView(string name)
    {
        ShowUi("cluster");
        UpdateDescriptor(name, "");
        Vector3 targetPos = new Vector3(0, 0, 0);
        CameraOrbit.CameraTo3D();
        CameraOrbit.CameraToPos(targetPos);

    }
    public void GalaxyDataView(string name)
    {
        ShowUi("galaxy");
        UpdateDescriptor(name, "");
        Vector3 targetPos = new Vector3(0, 0, 0);
        CameraOrbit.CameraTo3D();
        CameraOrbit.CameraToPos(targetPos);

    }
    public void StarDataView(Transform targetTransform, Star star)
    {
        ShowUi("star");
        UpdateDescriptor(star.Name, star.Type.Name);
        float targetSize = 2;
        Vector3 targetPos = new Vector3(0, 0, 0);
        CameraOrbit.CameraToPos(targetPos);
        CameraOrbit.CameraTo3D();
        Tracker(true, targetTransform, targetSize);
    }

    public void PlanetDataView(Transform targetTransform, Planet planet)
    {

        ShowUi("planet");

        UpdateDescriptor(planet.Name, planet.Type.Name + " World");

        UILeftPanel.GetComponent<UILeftPanel>().UpdatePlanetConditionData(planet);
            
        float targetSize = planet.Mass;
        
        CameraOrbit.CameraFollowTransform(targetTransform);
        CameraOrbit.CameraTo3D();

        //Target tracking starts after the camera and the screen is at the right place. 
        StartCoroutine(ResumeTargetTracking(targetTransform, targetSize));
    }

    IEnumerator ResumeTargetTracking(Transform targetTransform, float targetSize)
    {
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

