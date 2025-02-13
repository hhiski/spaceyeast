using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISelector : MonoBehaviour
{
    public Camera Camera;
    public GameObject Corner;
    GameObject TopLeft;
    GameObject TopRight;
    GameObject ButtomLeft;
    GameObject ButtomRight;

    public Transform TargetTransform;
    public float TargetSize = 10;

    void Start()
    {


        TopLeft = Instantiate(Corner, transform);
        TopRight = Instantiate(Corner, transform);
        ButtomLeft = Instantiate(Corner, transform);
        ButtomRight = Instantiate(Corner, transform);

        TopRight.transform.rotation = TopLeft.transform.rotation * Quaternion.Euler(0, 0, 270);
        ButtomLeft.transform.rotation = TopLeft.transform.rotation * Quaternion.Euler(0, 0, 180);
        ButtomRight.transform.rotation = TopLeft.transform.rotation * Quaternion.Euler(0, 0, 90);
        Camera = CameraOrbit.GetInstance().GetCamera(); ;
    }




    void UpdateSelectorCorners(Vector3 targetPos, float targetSize)
    {

        float offset = targetSize * 7f;


        Vector3 topLeftPos = targetPos + Camera.transform.right * -offset + Camera.transform.up * offset;
        Vector3 topRightPos = targetPos + Camera.transform.right * offset + Camera.transform.up * offset;
        Vector3 buttomLeftPos = targetPos + Camera.transform.right * offset + Camera.transform.up * -offset;
        Vector3 buttomRightPos = targetPos + Camera.transform.right * -offset + Camera.transform.up * -offset;


        Vector3 targetPosViewport = Camera.WorldToScreenPoint(topLeftPos);
        topLeftPos = new Vector3(targetPosViewport.x, targetPosViewport.y, 5);
        topLeftPos = Camera.ScreenToWorldPoint(topLeftPos);
        TopLeft.transform.position = topLeftPos;

        targetPosViewport = Camera.WorldToScreenPoint(topRightPos);
        topRightPos = new Vector3(targetPosViewport.x, targetPosViewport.y, 5);
        topRightPos = Camera.ScreenToWorldPoint(topRightPos);
        TopRight.transform.position = topRightPos;

        targetPosViewport = Camera.WorldToScreenPoint(buttomLeftPos);
        buttomLeftPos = new Vector3(targetPosViewport.x, targetPosViewport.y, 5);
        buttomLeftPos = Camera.ScreenToWorldPoint(buttomLeftPos);
        ButtomLeft.transform.position = buttomLeftPos;

        targetPosViewport = Camera.WorldToScreenPoint(buttomRightPos);
        buttomRightPos = new Vector3(targetPosViewport.x, targetPosViewport.y, 5);
        buttomRightPos = Camera.ScreenToWorldPoint(buttomRightPos);
        ButtomRight.transform.position = buttomRightPos;



    }

    void Update()
    {

        if (TargetTransform != null)
        {
            UpdateSelectorCorners(TargetTransform.position, TargetSize);
        }
    }
}
