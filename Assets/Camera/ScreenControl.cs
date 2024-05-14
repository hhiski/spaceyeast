using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScreenControl : MonoBehaviour {




	// Use this for initialization
	void Start () {


        GameObject gridspace = GameObject.Find("Gridspace");

    }

	
	// Update is called once per frame
	void Update () {
        if (Input.GetButtonDown("Fire2"))
        {

         //   SceneManager.LoadScene("main", LoadSceneMode.Additive);
            SceneManager.LoadScene("main");
           



        }




	}
}
