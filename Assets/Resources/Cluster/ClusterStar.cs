using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LineSpace;
using ColorSpace;
using static CelestialBody;

public class ClusterStar : MonoBehaviour
{

    public int StarId;
    public int HomeClusterId;
    public Color Color;
    public Material StarHeightLines;

    string  StarName;

    ColorFunctions ColorFunctions = new ColorFunctions();


   public void SetStarName(string name)
    {
        StarName = name;
    }


    void Start()
    {
        Vector3 position = this.transform.position;

        Material lineMaterial = new Material(Shader.Find("Sprites/Default"));
        Vector3[] heightLineSegments = new[] { new Vector3(position.x, 0, position.z), position };


        GameObject line = LineFunctions.CreateLineObject(this.transform, new Vector3(0, 0, 0), "Height Line", heightLineSegments, lineMaterial, 0.6f);


        if (this.GetComponent<Renderer>() != StarHeightLines)
        {
            Debug.LogWarning("Material StarHeightLines not set!");
            line.GetComponent<LineRenderer>().material = StarHeightLines;
        }

        if (this.GetComponent<Renderer>() != null)
        {
            this.GetComponent<Renderer>().material.SetColor("_Color", Color);

        }



        foreach (Transform child in gameObject.transform)
        {
            if (child.gameObject.name == "StarCorona")
            {
                GameObject corona = child.gameObject;

                if (corona.GetComponent<ParticleSystem>() != null)
                {

                    var main = corona.gameObject.GetComponent<ParticleSystem>().main;
                    main.startColor = Color;


                }
               

            }
        }


        UIClusterNames uIClusterNames = GameObject.Find("/Canvas/UIClusterNameTrackers").GetComponent<UIClusterNames>();

        uIClusterNames.AddClusterNameTag(StarName, position);


    }



    void OnMouseDown()
    {
        transform.parent.parent.GetComponent<GalaxyCatalog>().CreateSystem(HomeClusterId, StarId);
    }




}
