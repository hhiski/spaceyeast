using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIClusterStarNameTag : MonoBehaviour
{



    Camera mainCamera;

    Vector3 WorldPosition = new Vector3(0, 0, 0); //The designated star's position
    Vector3 ScreenPosition = new Vector3(0, 0, 0);
    int NameTagDistance = 5; //How far the tag floats next to the star


    public void SetWorldPosition(Vector3 position)
    {
        WorldPosition = position;
    }

    public void SetCamera(Camera camera)
    {
        mainCamera = camera;
    }

    //New tag gameObjects are created when cluster scope is accessed again.
    void OnDisable()
    {
        Destroy(gameObject);
    }

    void Update()
    {
        Vector3 newPosition = WorldPosition +  mainCamera.transform.right * NameTagDistance; //Moves name tag object (this) towards screen right edge
        Vector3 targetPosViewport = mainCamera.WorldToScreenPoint(newPosition);
        Vector3 newScreenPos = new Vector3(targetPosViewport.x, targetPosViewport.y, 5); //Worldspace position to screen position
        newScreenPos = mainCamera.ScreenToWorldPoint(newScreenPos);
        this.gameObject.transform.position = newScreenPos;
    }



    public void SetName(string name)
    {
       Text starName = this.gameObject.GetComponent<UnityEngine.UI.Text>();
       starName.text = name;
    }

    void Start()
    {
        
    }

 
}
