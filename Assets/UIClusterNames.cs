
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.UI;
using Unity.VisualScripting;
using System;

class NameTag
{
    Transform TargetTransform { get; set; } 
    Camera Camera { get; set; }

    GameObject TagObject { get; set; }

    string Name { get; set; } = "Not Set";
    Vector3 ScreenPosition { get; set; } = Vector3.zero;
    Vector3 Offset { get; set; } = Vector3.up;
    float TargetSize { get; set; } = 0f;

    public NameTag(Transform targetTransfrom, string name, GameObject tagObject, float targetSize, Camera camera, Vector3 offset)
    {
        TargetTransform = targetTransfrom;
        TagObject = tagObject;
        Camera = camera;
        Name = name;
        Offset = offset;
        TargetSize = targetSize;
        ScreenPosition = targetTransfrom.position;
        TagObject.transform.position = ScreenPosition;
    }

    public bool UpdateScreenPos()
    {
        float distanceToCamera = 5f;
        float worldOffset = TargetSize * 7f;
        Vector3 screenOffset = Offset;

        if (TargetTransform == null)
        {
            Debug.LogWarning("NameTag " + Name + " tracking target not available!");
            return false;
        }
        if (TagObject == null)
        {
            Debug.LogWarning("NameTag " + Name + " tag gameObject not available!");
            return false;
        }

        Vector3 position = TargetTransform.transform.position + Camera.transform.right * -worldOffset + Camera.transform.up * worldOffset;
        Vector3 screenPosition = Camera.WorldToScreenPoint(position);
        screenPosition = new Vector3(screenPosition.x + screenOffset.x, screenPosition.y + screenOffset.y, distanceToCamera);
        position = Camera.ScreenToWorldPoint(screenPosition);

        TagObject.transform.position = position;
        return true;
    }

    public Transform GetTargetTransform()
    {
        return TargetTransform;
    }


    public void UpdateName()
    {
        Text name = TagObject.GetComponent<UnityEngine.UI.Text>();
        try
        {
            name.text = Name;
        }
        catch (Exception ex)
        {
            Debug.Log("Error: " + ex.Message);
        }
 
    }
 
}

public class UIClusterNames : MonoBehaviour
{
    public GameObject UIClusterStarNameTag;

    List<NameTag> NameTags = new();

    Camera Camera;
    public bool TrackingTags = false;
    public Vector3 tagOffsetDirection = Vector3.up;
    private static UIClusterNames _instance;
    private static UIClusterNames Instance
    {
        get
        {
            if (_instance == null)
            {
                // If the instance is null, try to find it in the scene
                _instance = FindObjectOfType<UIClusterNames>();

                if (_instance == null)
                {
                    Debug.LogError("TrajectoryManager NULL, CANT BE FOUND");
                }
            }

            return _instance;
        }
    }
    public static UIClusterNames GetInstance()
    {
        return Instance;
    }

    void OnDisable()
    {
        ClearOldNameTags();
    }

    void LateUpdate()
    {
        if (TrackingTags)
        {
            foreach (NameTag nameTag in NameTags)
            {
                if (!nameTag.UpdateScreenPos())
                {
                    ClearSpecifiNameTag(nameTag);
                }
            }
        }
    }

    public void CreateSystemNameTags()
    {
        TrackingTags = false;
        ClearOldNameTags();

        if (Camera == null)
        {
            Camera = CameraOrbit.GetInstance().SystemCamera;
        }

        List<GameObject> planets = SystemController.GetInstance().GetPlanetObjects();
        Vector3 offset = tagOffsetDirection;

        foreach (GameObject planet in planets)
        {

            GameObject uiNameTag = Instantiate(UIClusterStarNameTag, this.gameObject.transform) as GameObject;

            string name = planet.GetComponent<SystemPlanet>().Planet.Name;
            float size = planet.GetComponent<SystemPlanet>().Planet.Mass;
            NameTag nameTag = new NameTag(planet.transform, name, uiNameTag, size, Camera, offset);
            nameTag.UpdateName();
            NameTags.Add(nameTag);
        }

        TrackingTags = true;
    }
    public void CreateClusterNameTags()
    {
        ClearOldNameTags();
        TrackingTags = false;

        if (Camera == null)
        {
            Camera = CameraOrbit.GetInstance().SystemCamera;
        }

        List<GameObject> stars = ClusterController.GetInstance().ListStarObjects();
        Vector3 tagOffsetDirection = Vector3.up;
        float tagDistance = 1f;
        Vector3 offset = tagOffsetDirection * tagDistance;
       
        foreach (GameObject star in stars)
        {

            GameObject uiNameTag = Instantiate(UIClusterStarNameTag, this.gameObject.transform) as GameObject;

            string name = star.GetComponent<ClusterStar>().Star.Name;

            NameTag nameTag = new NameTag(star.transform, name, uiNameTag, 1f, Camera, offset);
            nameTag.UpdateName();
            NameTags.Add(nameTag);
        }

        TrackingTags = true;
    }

     void ClearOldNameTags()
    {
        NameTags.Clear();
        TrackingTags = false;

        Debug.Log("Clearing name tags..");
        foreach (Transform child in gameObject.transform)
        {
            Destroy(child.gameObject);
        }
    }

    void ClearSpecifiNameTag(NameTag tag)
    {
        List<Transform> childrenToBeDestroyed = new List<Transform>(); // ^^'
        NameTags.Remove(tag);

        foreach (Transform child in gameObject.transform)
        {
            if (tag.GetTargetTransform() == child)
            {
                childrenToBeDestroyed.Add(child);
            }
        }

        foreach (Transform child in childrenToBeDestroyed)
        {
            Destroy(child.gameObject);
        }

    }


}
