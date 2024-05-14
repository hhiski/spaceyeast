using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetSociety : MonoBehaviour
{
    private Rigidbody2D Rigidbody;
    static Camera SystemCamera;
    private bool isDragging = false;

    void Start()
    {
        Rigidbody = GetComponent<Rigidbody2D>();

        SystemCamera = CameraOrbit.GetInstance().SystemCamera;
    }

    void OnMouseDown()
    {
        isDragging = true;
    }

    void OnMouseUp()
    {
        isDragging = false;
    }

    void FixedUpdate()
    {
        if (isDragging)
        {
            Vector3 mousePosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0);
            Vector3 objPosition = SystemCamera.ScreenToWorldPoint(mousePosition);
            Rigidbody.MovePosition(objPosition);
        }
    }
}
