using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SocietyPreceptBlock : MonoBehaviour
{
    private Rigidbody2D Rigidbody;
    static Camera SystemCamera;
    private bool isDragging = false;

    private const int X = 0;

    string Name { get; set; }


    public SocietyPrecept SocietyPrecept;

    public string GetName()
    {
        return Name;
    }



    public void SavePositionAndRotation()
    {
        SocietyPrecept.SetPosition(transform.position);
        SocietyPrecept.SetRotation(transform.rotation);
    }

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
