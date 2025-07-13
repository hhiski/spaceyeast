using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;



public class SocietyPreceptBlock : MonoBehaviour
{
    private Rigidbody2D Rigidbody;
    static Camera SystemCamera;
    private bool isDragging = false;
    private bool mouseInsideArea = true;

    private Collider2D SocietyBuildingAreaCollider;
    private Collider2D BlockUnderMouseCollider;

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



        GameObject SocietyBuildingArea = GameObject.FindGameObjectWithTag("SocietyBuildingArea");
        if (SocietyBuildingArea != null)
        {
            if (!SocietyBuildingArea.TryGetComponent<Collider2D>(out SocietyBuildingAreaCollider))
            {
                Debug.LogError($"Collider2D component missing on {SocietyBuildingArea.name}.");
            }
        }
        else
        {
            Debug.LogError("GameObject with tag 'SocietyBuildingArea' not found.");
        }


    }

    void OnMouseDown()
    {
        isDragging = true;

        void OnPointerDown(PointerEventData eventData)
        {
            Debug.Log("Pointer Down " + eventData.selectedObject.name);
        }

        void OnPointerUp(PointerEventData eventData)
        {
            Debug.Log("Pointer Up " + eventData.selectedObject.name);
        }
    }

    void OnMouseUp()
    {
        isDragging = false;
    }
 
    void FixedUpdate()
    {
        Vector3 mouseScreenPosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0);
        Vector3 mouseWorldPos = SystemCamera.ScreenToWorldPoint(mouseScreenPosition);


        BlockUnderMouseCollider = Physics2D.OverlapPoint(mouseWorldPos);


       
        Renderer surfaceRenderer = BlockUnderMouseCollider.gameObject.transform.gameObject.GetComponent<Renderer>();
        surfaceRenderer.material.color = Color.blue;

        if (SocietyBuildingAreaCollider.OverlapPoint(mouseWorldPos))
        {
            mouseInsideArea = true;
        }
        else
        {
            mouseInsideArea = false;
        }


        if (isDragging && mouseInsideArea)
        {
           
            
            Rigidbody.MovePosition(mouseWorldPos);
        }

    }
}
