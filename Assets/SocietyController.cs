using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static CelestialBody;

public class Society
{

    public List<SocietyPrecept> SocietyPrecepts = new List<SocietyPrecept>();

    public bool AddSocietyBlock(SocietyPrecept societyBlock)
    {
        SocietyPrecepts.Add(societyBlock);
        return true;
    }

    public Society()
    {
        Populate();
    }
    public Society(int count)
    {
        for (int i = 0; i < count; i++) {
            Populate();
        }
  
    }


    void Populate()
    {
        for (int i = 0; i < 5; i++)
        {
            SocietyPrecept adsad = new SocietyPrecept("aa", new Vector3(Random.Range(-0.5f, 1.1f), Random.Range(-0.44f, 0.55f), 0), Quaternion.identity);
            SocietyPrecepts.Add(adsad);

        }
    }
    public List<SocietyPrecept> GetSocietyPrecepts()
    {
        // Using copy constructor
        List<SocietyPrecept> societyPrecepts = new List<SocietyPrecept>();



        return societyPrecepts;
    }
}


public class SocietyPrecept
{


    string Name { get; set; }
    Vector2 Position { get; set; }
    Quaternion Rotation { get; set; }

public SocietyPrecept(string name, Vector2 position, Quaternion rotation)
    {
        Name = name;
        Position = position;
        Rotation = rotation;
    }

    public string GetName()
    {
        return Name;
    }
    public Quaternion GetRotation()
    {
        return Rotation;
    }
    public Vector2 GetPosition()
    {
        return Position;
    }
    public void SetRotation(Quaternion rotation)
    {
         Rotation = rotation;
    }
    public void SetPosition(Vector3 position)
    {
         Position = position;
    }
}

public class SocietyController : MonoBehaviour
{
    
    public GameObject SocietyPreceptBlockPrefab;

    private static SocietyController _instance;
    private static SocietyController Instance
    {
        get
        {
            if (_instance == null)
            {
                // If the instance is null, try to find it in the scene
                _instance = FindObjectOfType<SocietyController>();

                if (_instance == null)
                {
                    Debug.LogError("SocietyController NULL, CANT BE FOUND");
                }
            }

            return _instance;
        }
    }

    public static SocietyController GetInstance()
    {
        return Instance;
    }
    void OnDisable()
    {
        Debug.Log("Saving Society");
        foreach (Transform child in gameObject.transform)
        {
            if (child.gameObject.CompareTag("SocietyBlock"))
            {
                child.GetComponent<SocietyPreceptBlock>().SavePositionAndRotation();
            }
        }

        foreach (Transform child in gameObject.transform)
        {
            if (child.gameObject.CompareTag("SocietyBlock"))
            {
                Destroy(child.gameObject);
            }
        }
    
}

    public void VisualizeSociety(Planet planet)
    {
        

        UiCanvas UI = UiCanvas.GetInstance();
        UI.SocietytDataView(planet.Name);

        Society society =  planet.Society;
        int number = 0;
        foreach (SocietyPrecept precept in society.SocietyPrecepts)
        {
            number++;
            Debug.Log(number);
            GameObject societyBlock = Instantiate(SocietyPreceptBlockPrefab, this.transform) as GameObject;

            societyBlock.transform.position = precept.GetPosition();
            societyBlock.transform.rotation = precept.GetRotation();
            societyBlock.GetComponent<SocietyPreceptBlock>().SocietyPrecept = precept;

        }




    }


    
}
