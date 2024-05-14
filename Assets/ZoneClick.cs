using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;
using ColorSpace;   
public class ZoneClick : MonoBehaviour
{

    public int ZoneId;

    public Material ZoneMaterial;
    Color ColorMouseIn;
    Color ColorMouseOut;


    ColorFunctions ColorFunctions = new ColorFunctions();

    void OnMouseDown()
    {
        ZoneMaterial.color = ColorMouseOut;
        //   ZoneMaterial.color = new Color32(0, 166, 255, 26);
        transform.root.GetComponent<GalaxyCatalog>().CreateCluster(ZoneId);

        //StartCoroutine(LoadScene("cluster", ZoneId)); // Use in multiple scene - mode

    }


    void OnEnable()
    {


    
    }


    void OnMouseEnter()
    {
       ZoneMaterial.color = ColorMouseIn;
    }

    void OnMouseExit() 
    {

        ZoneMaterial.color = ColorMouseOut;
    }


    void TaskOnClick()
    {


    }

    void Start()
    {
        ZoneMaterial = GetComponent<Renderer>().material;

        ColorMouseOut = ZoneMaterial.GetColor("_Color");

        ColorMouseIn = ColorFunctions.SaturationShiftColor(ColorMouseOut, 0, 0.7f);
        ColorMouseIn = ColorFunctions.ValueShiftColor(ColorMouseIn, 0, 1.3f);
    }
}






