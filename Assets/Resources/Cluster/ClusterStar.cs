using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ColorSpace;
using Game.Lines;
using static CelestialBody;
using Unity.VisualScripting;

public class ClusterStar : MonoBehaviour
{

    public Star Star { get;  set; }
  //  public int StarId { get; private set; }
 //   public int HomeClusterId { get; private set; }


    public void SetStar(Star star)
    {
        Star = star;
    }
    public Star GetStar()
    {
        return Star;
    }



    public void AddZLine() 
    {
        Vector3 position = this.transform.position;

        Vector3[] heightLineSegments = new[] { new Vector3(position.x, 0, position.z), position };

        LineManager.Instance.CreateLineObject(this.transform, "Z-axis Line", heightLineSegments, LineType.Cluster);
    }

    public void SetClusterStarColor()
    {
        if (Star.Type.StarColor == null)
        {
            Debug.LogWarning("star.Type.StarColor");
            return;
        }

        Color starColor = Star.Type.StarColor;


        if (TryGetComponent<Renderer>(out Renderer renderer))
        {
            GetComponent<Renderer>().material.SetColor("_Color", starColor);
        }
        else
        {
            Debug.LogWarning("ClusterStar object does not have a Renderer component!");
        }


        foreach (Transform child in transform)
        {
            if (child.name != "StarCorona") continue;

            if (child.TryGetComponent<ParticleSystem>(out ParticleSystem particleSystem))
            {
                var mainModule = particleSystem.main;
                mainModule.startColor = starColor;
            }
            else
            {
                Debug.LogWarning("StarCorona object does not have a ParticleSystem component!");
            }
        }
    }

    void OnMouseDown()
    {
        transform.parent.parent.GetComponent<GalaxyCatalog>().CreateSystem(Star.HomeClusterId, Star.Id);
    }




}
