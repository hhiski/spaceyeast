using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class SkyboxController : MonoBehaviour
{
    public Material GalaxySkybox;
    public Material ClusterSkybox;
    public Material SystemSkybox;
    public Material SocietySkybox;

    Camera Camera;

    private static SkyboxController _instance;
    private static SkyboxController Instance
    {
        get
        {
            if (_instance == null)
            {
                // If the instance is null, try to find it in the scene
                _instance = FindObjectOfType<SkyboxController>();

                if (_instance == null)
                {
                    Debug.LogError("SkyboxController NULL, CANT BE FOUND");
                }
            }

            return _instance;
        }
    }
    public static SkyboxController GetInstance()
    {
        return Instance;
    }

    public enum SkyboxType
    {
        Galaxy,
        Cluster,
        System,
        Society,
    };



    public void SetSkybox(SkyboxType skybox)
    {

        Material skyboxMaterial = GalaxySkybox;

        if (skybox == SkyboxType.Galaxy)
        {
            skyboxMaterial = GalaxySkybox;
        }

        if (skybox == SkyboxType.Cluster)
        {

            skyboxMaterial = ClusterSkybox;
        }

        if (skybox == SkyboxType.System)
        {
            skyboxMaterial = SystemSkybox;

        }
        if (skybox == SkyboxType.Society)
        {
            skyboxMaterial = SocietySkybox;
        }

        if (Camera == null)
        {
            Camera = CameraOrbit.GetInstance().SystemCamera;
        }


        if (Camera.TryGetComponent<Skybox>(out Skybox cameraSkybox))
        {
            cameraSkybox.material = skyboxMaterial;
        }
        else
        {
            Debug.LogWarning("Camera Skybox-component not found");
        }



    }


}
