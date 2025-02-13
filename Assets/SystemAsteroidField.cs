using Game.Lines;
using Game.Math;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static CelestialBody;

public class SystemAsteroidField : MonoBehaviour
{
    public GameObject SystemAsteroidPrefab;
    float AsteroidFieldThickness = 6f;
    float AsteroidScaleVariation = 2f;
    float FieldDistanceVariation = 6f;
    float FieldDistance = 100f;
    float AsteroidScale = 1;
    public void VisualizeAsteroidField(AsteroidBelt asteroidBelt)
    {
        FieldDistance = asteroidBelt.Distance;

        //Creates asteroid belts for the system
        for (int i = 0; i < 44; i++)
        {

            if (SystemAsteroidPrefab != null)
            {
                GameObject Asteroid = Instantiate(SystemAsteroidPrefab, new Vector3(0f, 0f, 0f), transform.rotation) as GameObject;
                Asteroid.transform.parent = this.transform;
                AsteroidScale = Random.Range(1/ AsteroidScaleVariation, AsteroidScaleVariation);
                Asteroid.transform.localScale = new Vector3(AsteroidScale, AsteroidScale, AsteroidScale);
                AsteroidFieldThickness = Random.Range(-FieldDistanceVariation, FieldDistanceVariation);
                Asteroid.transform.position = MathFunctions.RandomPositionOnCircle(FieldDistance + AsteroidFieldThickness, new Vector3(0, 0, 0));
                Asteroid.transform.GetChild(0).transform.rotation = Random.rotation;

                GameObject circle = LineManager.Instance.CreateCircleObject(Asteroid.transform, "Orbital Circle", new Vector3(0,0,0), 360, LineType.Orbital);
                circle.GetComponent<LineRenderer>().loop = true;
                circle.GetComponent<LineRenderer>().startWidth *= 0.25f;


            }
            else
            {
                Debug.Log("null SystemAsteroidPrefab!");
            }
        }


    }
}
