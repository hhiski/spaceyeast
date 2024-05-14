using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyboxController : MonoBehaviour
{
    public Material GalaxySkybox;
    public Material ClusterSkybox;
    public Material SystemSkybox;
    public Material SocietySkybox;

    
    public void SetSystemSkybox()
    {
        RenderSettings.skybox = SystemSkybox;
    }

    public IEnumerator SetSkybox(string skybox)
    {
        Debug.Log("skybox: " + skybox);
        if (skybox == "galaxy")
        {
            RenderSettings.skybox = GalaxySkybox;
        }

        if (skybox == "cluster")
        {

            RenderSettings.skybox = ClusterSkybox;
        }

        if (skybox == "system")
        {
            RenderSettings.skybox = SystemSkybox;
            Debug.Log(RenderSettings.skybox);
        }
        if (skybox == "society")
        {
            RenderSettings.skybox = SocietySkybox;
        }

        yield return null;
        yield return new WaitForEndOfFrame();
    }

   /* public void SetSkybox(string skybox)
    {
        Debug.Log("skybox: " + skybox);
        if (skybox == "galaxy")
        {
            RenderSettings.skybox = GalaxySkybox;
        }

        if (skybox == "cluster")
        {

            RenderSettings.skybox = ClusterSkybox;
        }

        if (skybox == "system")
        {
            RenderSettings.skybox = SystemSkybox;
            Debug.Log(RenderSettings.skybox);
        }
        if (skybox == "society")
        {
            RenderSettings.skybox = SocietySkybox;
        }

    }*/
}
