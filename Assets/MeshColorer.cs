using UnityEngine;
using System.Collections;

public class MeshColorer : MonoBehaviour
{
    public GameObject myGameObject;
    private PolygonCollider2D pColider;

    protected void Start()
    {
        pColider = myGameObject.GetComponent<PolygonCollider2D>();
        DrawPolygonCollider(pColider);
        //   highlightAroundCollider(pColider, Color.yellow, Color.red, 4.1f);



    }

    public static void DrawPolygonCollider(PolygonCollider2D collider)
    {
        LineRenderer _lr = collider.gameObject.AddComponent<LineRenderer>();
        _lr.startWidth = 0.125f;
        _lr.endWidth = 0.125f;
        _lr.useWorldSpace = false;
        _lr.positionCount = collider.points.Length + 1;
        for (int i = 0; i < collider.points.Length; i++)
        {
            _lr.SetPosition(i, new Vector3(collider.points[i].x, collider.points[i].y));
        }
        _lr.SetPosition(collider.points.Length, new Vector3(collider.points[0].x, collider.points[0].y));
    }

    
}