using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CameraOrbit : MonoBehaviour
{

    protected Transform _XForm_Camera;
    protected Transform _XForm_Parent;

    protected Vector3 _LocalRotation;
    protected float _CameraDistance = -600;

    public float MouseSensitivity = 4f;
    public float ScrollSensitvity = 200f;
    public float OrbitDampening = 100f;
    public float ScrollDampening = 6f;
    public float CloseDistance = 200f;
    public float MaxDistance = 860f;

    public bool GalacticPlane = false;
    public bool CameraDisabled = false;

    bool CameraMovementReady = true;
    bool CameraFollowing = false;

    Transform cameraTarget;

    public Camera SystemCamera;

    // Use this for initialization
    void Start()
    {
        this._XForm_Camera = this.transform.GetChild(0);
        this._XForm_Parent = this.transform;

        SystemCamera = GameObject.Find("System Camera").GetComponent<Camera>();

        GL.ClearWithSkybox(false, SystemCamera);

    }

    private static CameraOrbit _instance;

    static CameraOrbit Instance
    {
        get
        {
            if (_instance == null)
            {
                // If the instance is null, try to find it in the scene
                _instance = FindObjectOfType<CameraOrbit>();

                if (_instance == null)
                {
                    Debug.LogError("CameraOrbit NULL, CANT BE FOUND");
                }
            }

            return _instance;
        }
    }

    public static CameraOrbit GetInstance()
    {
        return Instance;
    }

    public void ResetCamera()
    {
        CameraDisabled = false;
        CameraToPos(new Vector3(0,0,0));
        _LocalRotation.x = 0;
        _LocalRotation.y = -30f;
        _CameraDistance = -200;


        this._XForm_Parent.rotation = Quaternion.Euler(_LocalRotation.y, _LocalRotation.x, 0);
        SystemCamera.orthographic = false;
    }


    public void CameraTo3D()
    {

        if (SystemCamera == null)
        {
            SystemCamera = GameObject.Find("System Camera").GetComponent<Camera>();
        }

        CameraDisabled = false;
        SystemCamera.orthographic = false;

    }

    public void CameraTo2D() 
    {


        if (SystemCamera == null)
        {
            SystemCamera = GameObject.Find("System Camera").GetComponent<Camera>();
        }

        CameraFollowing = false;
        CameraDisabled = true;

        CameraToPos(new Vector3(0, 0, 0));

        this._XForm_Parent.rotation = Quaternion.Euler(0,0, 0);
        this._XForm_Camera.localPosition = new Vector3(0f, 0f, 1);
        SystemCamera.orthographicSize = 1;
        SystemCamera.orthographic = true;
    }

    public void CameraToPos(Vector3 targetPos)
    {
        transform.parent.position = targetPos;
    }

    void CameraToMousePlane()
    {


        Globals.spaceScaleLevel = "system";

        CameraFollowing = false;
        CameraMovementReady = false;
        Plane plane = new Plane(Vector3.up, 0);
        Vector3 pos = new Vector3(0, 0, 0);

        float distance;
        Ray ray = SystemCamera.ScreenPointToRay(Input.mousePosition);
        if (plane.Raycast(ray, out distance))
        {
            pos = ray.GetPoint(distance);
        }

        transform.parent.position = pos;

    }


    void ReadyCamera() {
        if (Input.GetButton("Fire3") == false)
        {
            CameraMovementReady = true;
        } else
        {
            Invoke("ReadyCamera", 0.11f);
        }
    }
    
    public void CameraFollowTransform(Transform targetTransform)
    {
        cameraTarget = targetTransform;
        CameraFollowing = true;
    }


    void LateUpdate()
    {

        if (!CameraDisabled && CameraFollowing && cameraTarget != null)
        {
            CameraToPos(cameraTarget.position);
        }


        if (!CameraDisabled && CameraMovementReady && Input.GetButton("Fire3") == true)
        {
            CameraToMousePlane();
            Invoke("ReadyCamera", 0.11f);
        }


        if (!CameraDisabled && Input.GetButton("Fire1") == true)
        {
            if (Input.GetAxis("Mouse X") != 0 || Input.GetAxis("Mouse Y") != 0)
            {
                _LocalRotation.x += Input.GetAxis("Mouse X") * MouseSensitivity;
                _LocalRotation.y += Input.GetAxis("Mouse Y") * MouseSensitivity;

               if (_LocalRotation.y < -90f)
                    _LocalRotation.y = -90f;
                else if (_LocalRotation.y > 30f)
                    _LocalRotation.y = 30f;
            }
        }




        if (!CameraDisabled && Input.GetAxis("Mouse ScrollWheel") != 0f)
        {
            float ScrollAmount = Input.GetAxis("Mouse ScrollWheel") * ScrollSensitvity;
            ScrollAmount = 20f * ScrollAmount;
            this._CameraDistance += ScrollAmount * -1f;

            this._CameraDistance = Mathf.Clamp(this._CameraDistance, -MaxDistance,-CloseDistance);


        }


        if (!CameraDisabled)
        {
            Quaternion QT = Quaternion.Euler(_LocalRotation.y, _LocalRotation.x, 0);

            this._XForm_Parent.rotation = Quaternion.Slerp(transform.rotation, QT, OrbitDampening * Time.deltaTime);

        }

        if (!CameraDisabled && Mathf.Round(this._XForm_Camera.localPosition.z) != Mathf.Round(this._CameraDistance))
        {
            this._XForm_Camera.localPosition = new Vector3(0f, 0f, Mathf.Lerp(this._XForm_Camera.localPosition.z, this._CameraDistance * -1f, Time.deltaTime * ScrollDampening) );
        }
    }
}