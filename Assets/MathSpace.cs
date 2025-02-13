using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Math
{
    class MathFunctions
    {
      
        public static float GetRandomOrbitInclination(int seed)
        {
            float maxInclination = 10; // deg
            float inclination = StandardDeviation(0, maxInclination, seed + 500);
            inclination /= 180f;
            return inclination;
        }

    
        public static float GetOrbitSpeed(float orbitDistance)
        {
            float orbitSpeed = 0;
            orbitSpeed = (300f / ((1f / 15f) * orbitDistance * Mathf.PI * orbitDistance)) * Random.Range(2, 5);
            orbitSpeed /= 360f;
            return orbitSpeed;
        }


        public static float GetEffectiveTemperature(float distance, float albedo, float solarTemp)
        {
            float temperatureB = 0;
            distance = distance / 80.000f; // distance conversion to AUs

            if (distance <= 0.55f)
            {
                temperatureB = -560f * distance + 733f;
            }
            else if (distance > 0.55f && distance <= 0.819f)
            {
                temperatureB = -500f * distance + 700f;
            }
            else if (distance > 0.819f && distance <= 2.968f)
            {
                temperatureB = -107f * distance + 378f;
            }
            else
            {
                temperatureB = -13.7f * distance + 101;
            }


            if (temperatureB <= 3) { temperatureB = 3; }; // Planets cant get colder than space

            temperatureB = temperatureB * (0.000173f * solarTemp - 0.0008f); //Solar Heat Factor
            temperatureB = temperatureB - (temperatureB * (0.1667f * albedo)); //Bond albedo reflection
            return temperatureB - 273;
        }

        public static Vector3 RandomPositionOnCircle(float minDistance, float maxDistance, Vector3 middlePos)
        {
            float RandomDistanceFromCenter = Random.Range(minDistance, maxDistance);
            float angle = Random.Range(0f, 6.28318531f);
            float tempX = middlePos.x + Mathf.Cos(angle) * RandomDistanceFromCenter;
            float tempZ = middlePos.z + Mathf.Sin(angle) * RandomDistanceFromCenter;

            Vector3 PlanetPos = new Vector3(tempX, 0f, tempZ);
            return PlanetPos;
        }

        public static Vector3 PositionOnCircle(float distance, float phase, Vector3 middlePos)
        {
            float angle = phase * 6.28318531f; 
            float tempX = middlePos.x + Mathf.Cos(angle) * distance;
            float tempZ = middlePos.z + Mathf.Sin(angle) * distance;

            Vector3 PlanetPos = new Vector3(tempX, 0f, tempZ);
            return PlanetPos;
        }

        public static Vector3 RandomPositionOnCircle(float distance, Vector3 middlePos)
        {
            float angle = Random.Range(0f, 6.28318531f);
            float tempX = middlePos.x + Mathf.Cos(angle) * distance;
            float tempZ = middlePos.z + Mathf.Sin(angle) * distance;

            Vector3 PlanetPos = new Vector3(tempX, 0f, tempZ);
            return PlanetPos;
        }

        public static float GetOrbitalDistance(Vector3 pos)
        {
            float distance = Vector3.Distance(pos, new Vector3(0f, 0f, 0f));
            return distance;
        }

        public static float StandardDeviation(float mean, float standardDeviation)
        {
            return StandardDeviation(mean, standardDeviation, 0);
        }


        public static float StandardDeviation(float mean, float standardDeviation, int seed)
        {

            System.Random Random = new System.Random(seed);

            // Box-Muller transform
            float u1 = 1.0f - (float)Random.NextDouble();
            float u2 = 1.0f - (float)Random.NextDouble();
            float normalDistribution = Mathf.Sqrt(-2.0f * Mathf.Log10(u1)) * Mathf.Sin(2.0f * Mathf.PI * u2); //random normal(0,1)

            float gauss = mean + standardDeviation * normalDistribution;

            return gauss;
        }

    }
}
