using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


public class DBhandler : MonoBehaviour
{


  
    // Start is called before the first frame update
    void Start()
    {
        
      //  StartCoroutine(GetText());
      
    }

    public void Rename()
    {
        StartCoroutine(GetRequest("http://www.weepgaming.rf.gd/getValues.php?Id=1"));
    }

    IEnumerator GetText()
    {
        string url = "http://weepgaming.rf.gd/getValues.php";
        WWWForm form = new WWWForm();
        form.AddField("Id", 1);

        UnityWebRequest www = UnityWebRequest.Post(url, form);
        yield return www.SendWebRequest();

        Debug.Log(www.result);


    }

    IEnumerator GetRequest(string uri)
    {
        WWWForm form = new WWWForm();
        form.AddField("Id", 1);

        UnityWebRequest www = UnityWebRequest.Post(uri, form);
        yield return www.SendWebRequest();

        Debug.Log(www.result);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
