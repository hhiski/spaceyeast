using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SystemAsteroidBelt : MonoBehaviour
{
    public int Type;
    public float Distance;

    public GameObject RockAsteroidBelt;
    public GameObject IceAsteroidBelt;
    public GameObject GasAsteroidBelt;
    public GameObject AsteroidBeltPrefab;

    void Start()
    {

        switch (Type)
        {
            case 1:
                AsteroidBeltPrefab = IceAsteroidBelt; break;
            case 2:
                AsteroidBeltPrefab = RockAsteroidBelt; break;
            case 3:
                AsteroidBeltPrefab = GasAsteroidBelt; break;
            default:
                AsteroidBeltPrefab = RockAsteroidBelt; break;
        }

        AsteroidBeltPrefab = Instantiate(AsteroidBeltPrefab, transform.position, transform.rotation) as GameObject;

        ParticleSystem AstroidSystem = AsteroidBeltPrefab.GetComponent<ParticleSystem>();

        ParticleSystem.ShapeModule Shape = AsteroidBeltPrefab.GetComponent<ParticleSystem>().shape;
        Shape.radius = Distance;

        AsteroidBeltPrefab.transform.parent = this.transform;

        var mainAstroidSystem = AstroidSystem.main;
        AstroidSystem.Clear();
        mainAstroidSystem.simulationSpeed = 1f;
        AstroidSystem.Simulate(6.3f);

        mainAstroidSystem.simulationSpeed = 0.05f;


        AstroidSystem.Play();

    }

    void Awake()
    {

      
    }

    void Update()
    {
        
    }
}
