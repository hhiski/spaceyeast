using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static CelestialBody;

public class Society
{

    public List<SocietyBlock> SocietyBlocks = new List<SocietyBlock>();

    public bool AddSocietyBlock(SocietyBlock societyBlock)
    {
        SocietyBlocks.Add(societyBlock);
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
        SocietyBlock adsad = new SocietyBlock("aa", new Vector3(Random.Range(-10, 10), Random.Range(-10, 10), 0));
        SocietyBlocks.Add(adsad);
    }

    public List<SocietyBlock> GetSocietyBlocks()
    {
        // Using copy constructor
        List<SocietyBlock> societyBlocks = new List<SocietyBlock>(SocietyBlocks);

        foreach (SocietyBlock block in SocietyBlocks)
        {


        }


        return SocietyBlocks;
    }
}

public class SocietyBlock
{


    private const int X = 0;

    string Name { get; set; }
    Vector2 Position { get; set; }


    public SocietyBlock(string name, Vector2 position)
    {
        Name = name;
        Position = position;
    }

    public string GetName()
    {
        return Name;
    }

    public Vector2 GetPosition()
    {
        return Position;
    }

}

public class SocietyController : MonoBehaviour
{
    
    public GameObject testBlock;


    public void VisualizeSociety(Planet planet)
    {
        UiCanvas UI = UiCanvas.GetInstance();
        UI.SocietytDataView(planet.Name);

        Society society =  planet.Society;
        int number = 0;
        foreach (SocietyBlock block in society.SocietyBlocks)
        {
            number++;
            Debug.Log(number);
            GameObject societyBlock = Instantiate(testBlock) as GameObject;
            Vector2 dPos = block.GetPosition();
            societyBlock.transform.position = new Vector3(dPos.x , dPos.y);


        }

        /*
    List<SocietyBlock> societyBlocks = new List<SocietyBlock>() planet.Society.SocietyBlocks;

    foreach (SocietyBlock block in SocietyBlocks)
    {


        GameObject societyBlock = Instantiate(testBlock, block.GetPosition()) as GameObject;
        clusterStar.GetComponent<ClusterStar>().StarColor = star.Type.StarColor;
        clusterStar.GetComponent<ClusterStar>().HomeClusterId = activeCluster.Id;
        clusterStar.GetComponent<ClusterStar>().StarId = starIndex;
        clusterStar.GetComponent<ClusterStar>().SetStarName(star.Name);

        clusterStar.transform.parent = this.transform;

    }
    */



    }


    
}
