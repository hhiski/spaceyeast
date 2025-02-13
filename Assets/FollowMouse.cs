using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class FollowMouse : MonoBehaviour
    {
        private Camera SystemCamera;


    void OnGUI()
        {
            Vector3 point = new Vector3();
            Event currentEvent = Event.current;
            Vector2 mousePos = new Vector2();


        if (SystemCamera == null)
        {
            SystemCamera = CameraOrbit.GetInstance().SystemCamera;
        }

        mousePos.x = currentEvent.mousePosition.x;
        mousePos.y = SystemCamera.pixelHeight - currentEvent.mousePosition.y;

        point = SystemCamera.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, 5));
        transform.position = point;

        }
    }