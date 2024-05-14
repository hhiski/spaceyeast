using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class FollowMouse : MonoBehaviour
    {
        private Camera SystemCamera;

        void Start()
          {
        SystemCamera = GameObject.Find("System Camera").GetComponent<Camera>();
        
        }

        void OnGUI()
        {
            Vector3 point = new Vector3();
            Event currentEvent = Event.current;
            Vector2 mousePos = new Vector2();

            // Get the mouse position from Event.
            // Note that the y position from Event is inverted.
            mousePos.x = currentEvent.mousePosition.x;
            mousePos.y = SystemCamera.pixelHeight - currentEvent.mousePosition.y;

            point = SystemCamera.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, 5));
              transform.position = point;

        }
    }