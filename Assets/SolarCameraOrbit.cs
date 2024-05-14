using UnityEngine;
using System.Collections;
using UnityEngine.UI;




public class SolarCameraOrbit : MonoBehaviour
{

    protected Transform _XForm_Camera;
    protected Transform _XForm_Parent;

    protected Vector3 _LocalRotation;
    protected float _CameraDistance = 280f;

    public float MouseSensitivity = 4f;
    public float ScrollSensitvity = 238f;
    public float OrbitDampening = 1f;
    public float ScrollDampening = 1f;

    public bool CameraDisabled = false;

    Camera main_cam;
    Camera solar_cam;
    Canvas solar_canvas;

    Canvas solar_canvas_b;
    Canvas solar_canvas_c;

    // Use this for initialization
    void Start()
    {
        this._XForm_Camera = this.transform;
        this._XForm_Parent = this.transform.parent;


        solar_cam = GameObject.Find("Solar Camera").GetComponent<Camera>();
        solar_canvas_b = solar_cam.transform.Find("Solar_canvas_b").GetComponent<Canvas>();
        solar_canvas_c = solar_cam.transform.Find("Solar_canvas_c").GetComponent<Canvas>();
        SolarCamTitle("Random solar");
    }

    public void SolarCamSwitch(bool status)
    {
        if (status)
        {

            solar_cam.enabled = true;
            solar_canvas_b.enabled = true;
            solar_canvas_c.enabled = true;

        }

        else if (!status)
        {
            solar_cam.enabled = false;
            solar_canvas_b.enabled = false;
            solar_canvas_c.enabled = false;

        }

    }

    public void SolarCamTitle(string i_name)
    {
        i_name = "solar cam";
        solar_canvas_c.transform.Find("solar_name_ui").GetComponent<Text>().text = i_name;

    }

    void LateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
            CameraDisabled = !CameraDisabled;

        if (!CameraDisabled && Input.GetButton("Fire1") == true)
        {
            //Rotation of the Camera based on Mouse Coordinates
            if (Input.GetAxis("Mouse X") != 0 || Input.GetAxis("Mouse Y") != 0)
            {
                _LocalRotation.x += Input.GetAxis("Mouse X") * MouseSensitivity;
                _LocalRotation.y += Input.GetAxis("Mouse Y") * MouseSensitivity;

                //Clamp the y Rotation to horizon and not flipping over at the top
                if (_LocalRotation.y < -90f)
                    _LocalRotation.y = -90f;
                else if (_LocalRotation.y > 30f)
                    _LocalRotation.y = 30f;
            }
            //Zooming Input from our Mouse Scroll Wheel
        }

    //kesken nappi move parent

        if (Input.GetButton("Fire2") == true)
        {

            //  SolarCamSwitch(false);
            //  main_cam.enabled = true;

        }

        if (Input.GetAxis("Mouse ScrollWheel") != 0f)
        {
            float ScrollAmount = Input.GetAxis("Mouse ScrollWheel") * ScrollSensitvity;

            //ScrollAmount *= (this._CameraDistance * 0.8f);
            ScrollAmount = 20 * ScrollAmount;
            this._CameraDistance += ScrollAmount * 1f;

            this._CameraDistance = Mathf.Clamp(this._CameraDistance, -250.4f, -20f);

         //   Debug.Log(_CameraDistance);
        }

        //Actual Camera Rig Transformations
        Quaternion QT = Quaternion.Euler(_LocalRotation.y, _LocalRotation.x, 0);
        this._XForm_Parent.rotation = Quaternion.Lerp(this._XForm_Parent.rotation, QT, Time.deltaTime * OrbitDampening);

        if (this._XForm_Camera.localPosition.z != this._CameraDistance * -1f)
        {
            this._XForm_Camera.localPosition = new Vector3(0f, 0f, Mathf.Lerp(this._XForm_Camera.localPosition.z, this._CameraDistance * -1f, Time.deltaTime * ScrollDampening));
        }
    }
}