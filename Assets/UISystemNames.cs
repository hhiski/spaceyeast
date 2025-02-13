using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISystemNames : MonoBehaviour
{
    public GameObject UIClusterStarNameTag;

    List<GameObject> UIClusterStarNameTags = new List<GameObject>();
    Camera Camera;
    bool TrackingTags = false;


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
                Destroy(child.gameObject);
            }
        }
    }
    //name: name of the star, position: position of the star
    public void AddClusterNameTag(string name, Vector3 position)
    {

        if (Camera == null)
        {
            Camera = CameraOrbit.GetInstance().SystemCamera;
        }

        if (UIClusterStarNameTag != null)
        {
            GameObject newUIClusterStarNameTag = Instantiate(UIClusterStarNameTag) as GameObject;
        //    newUIClusterStarNameTag.GetComponent<UIClusterStarNameTag>().SetWorldPosition(position);
         //   newUIClusterStarNameTag.GetComponent<UIClusterStarNameTag>().SetCamera(Camera);
         //   newUIClusterStarNameTag.GetComponent<UIClusterStarNameTag>().SetName(name);
            newUIClusterStarNameTag.transform.SetParent(this.gameObject.transform, false);



        }

        else
        {
            Debug.Log("UIClusterNameTag Prefab not found!");
        }

        TrackingTags = true;

    }

}
