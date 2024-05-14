using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIClusterNames : MonoBehaviour
{
    public GameObject UIClusterStarNameTag;

    List<GameObject> UIClusterStarNameTags = new List<GameObject>();
    Camera Camera;
    bool TrackingTags = false;

    void Start()
    {

        ClusterController clusterController = ClusterController.GetInstance();

        Camera = CameraOrbit.GetInstance().SystemCamera;

        Vector3 clusterStarPosition = new Vector3(0,0,0);



    }

    void OnDisable()
    {
        UIClusterStarNameTags.Clear();
        foreach (Transform child in gameObject.transform)
        {
            if (child.gameObject.tag == "Cluster")
            {
                DestroyImmediate(child.gameObject);
                Destroy(child.gameObject);
            }
            if (child.gameObject.tag == "System")
            {
                DestroyImmediate(child.gameObject);
            }
        }
    }
    //name: name of the star, position: position of the star
    public void AddClusterNameTag(string name, Vector3 position)
    {


        if (UIClusterStarNameTag != null)
        {
            GameObject newUIClusterStarNameTag = Instantiate(UIClusterStarNameTag) as GameObject;
            newUIClusterStarNameTag.GetComponent<UIClusterStarNameTag>().SetWorldPosition(position);
            newUIClusterStarNameTag.GetComponent<UIClusterStarNameTag>().SetCamera(Camera);
            newUIClusterStarNameTag.GetComponent<UIClusterStarNameTag>().SetName(name);
            newUIClusterStarNameTag.transform.SetParent(this.gameObject.transform, false);


           
        }

        else
        {
            Debug.Log("UIClusterNameTag Prefab not found!");
        }

        TrackingTags = true;

    }

}
