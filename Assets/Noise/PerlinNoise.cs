using Game.ID;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using TreeEditor;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

namespace Game.Noise
{
    public class NoiseManager : MonoBehaviour
    {
        public static NoiseManager Instance { get; private set; }
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        // This is the current function in use

        public float SimplePerlinFilter(Vector3 point, float frequency)
        {
            point = new Vector3(point.x * frequency , point.y * frequency , point.z * frequency );

            float noiseA = Mathf.PerlinNoise(point.x + 1.12f , point.y - 3.3f );

            float noiseB = Mathf.PerlinNoise(point.x + 4.02f , point.z + 6.9f );

            float noiseC = Mathf.PerlinNoise(point.y + 8.92f , point.x + 58.4f);

            float noiseD = Mathf.PerlinNoise(point.y + 4.61f , point.z - 5.1f );

            float noiseE = Mathf.PerlinNoise(point.z + 12.78f , point.x - 0.4f );

            float noiseF = Mathf.PerlinNoise(point.z + 52.1f , point.y - 8.2f );

            float noise = (3f - (noiseA + noiseB + noiseC + noiseD + noiseE + noiseF))/3f;

            //   float noise = Mathf.PerlinNoise(point.x + 1.12f * 0.7f, point.y - 4.3f * 0.7f) * Mathf.PerlinNoise(point.y - 3.47f * 0.7f, point.z + 1.77f * 0.7f);


            //  noise -= 0.25f;
            //   noise = (0.25f - noise) * 4f;
            return noise ;
        }

        // Version 2: This is the alternative method, not in used right now
        /*public float PerlinFilter(Vector3 point, Noise noiseFilter, float frequency, int level, float amplitude, float offset)
        {
            point = new Vector3(point.x * frequency + offset, point.y * frequency + offset, point.z * frequency + offset);
            float noise = (noiseFilter.Evaluate(point)+1)/2f;
            return noise * amplitude;
        }*/


        /*
    
        public float NoisePatternFluidic(float noise, Vector3 point, Noise noiseFilter)
        {
            point = new Vector3(point.x * noise, point.y * noise, point.z * noise);
            noise = noiseFilter.Evaluate(point);
            return noise;
        }

        public float NoisePatternCircles(float noise)
        {
            noise = 4 * noise; noise = noise - Mathf.Round(noise); noise = noise / 3; ;
            return noise;
        }

        public float NoisePatternWrinkle(float noise, Vector3 point, Noise noiseFilter)
        {
            noise = noiseFilter.Evaluate(point * (1 + 0.25f * noise));
            return noise;
        }

        public float NoisePatternWarp(float noise, Vector3 point, Noise noiseFilter, float warpForce)
        {
            float xDistortion = noiseFilter.Evaluate(warpForce * (new Vector3(point.x + 2.3f, point.y + 2.9f, point.z + 2.2f)));
            float yDistortion = noiseFilter.Evaluate(warpForce * (new Vector3(point.x + 3.1f, point.y + 4.2f, point.z + 3.5f)));
            float zDistortion = noiseFilter.Evaluate(warpForce * (new Vector3(point.x + 5.3f, point.y + 5.9f, point.z + 4.2f)));
            noise = noiseFilter.Evaluate((new Vector3(point.x + xDistortion, point.y + yDistortion, point.z + zDistortion)));
            return noise;
        }

        public float NoisePatternCheese(float noise)
        {
            if (noise >= 0.7f) { noise = -noise; }
            if (noise <= 0.23f && noise >= -0.23f) { noise = -0.23f; }
            if (noise <= 0.1f) { noise = -0.1f; }
            if (noise < 0f) { noise = 1.1f * noise; }
            return noise;
        }

        public float NoisePatternBlotches(float noise, Vector3 point, Noise noiseFilter)
        {
            float blotchNoise = noiseFilter.Evaluate(point * 1.5f);
            if (blotchNoise < 0) { blotchNoise = 0; }
            if (blotchNoise > 0) { blotchNoise = Mathf.Pow(noise, 4); }
            if (blotchNoise > 0.67f) { blotchNoise = -blotchNoise / 1.5f; }
            return noise + blotchNoise;
        }

        
        public float NoisePatternRidges(float noise, float ridgePower)
        {
            float secondNoise = 0;
            float ridgeNoise = 1;
            /*
            if (antiRidges)
            {
                secondNoise = SecondNoiseLayer.Evaluate(point);
                secondNoise = Mathf.Abs(secondNoise);

            }
            ridgeNoise = Mathf.Abs(noise) - 0.5f;
            ridgeNoise = Mathf.Lerp(ridgeNoise, noise, secondNoise);

            noise = Mathf.Pow(ridgeNoise, ridgePower);
            return noise;
        }

        public float NoisePatternDoubleRidges(float noise, Vector3 point, Noise noiseLayer, float ridgePower)
        {
            float antiRidgeNoise = noiseLayer.Evaluate(point);
            antiRidgeNoise = Mathf.Abs(antiRidgeNoise);
            noise += antiRidgeNoise;
            return noise;
        }
        public float NoisePatternSlush(float noise, Vector3 point, Noise noiseLayer, float ridgePower)
        {
            float slushNoise = noiseLayer.Evaluate(point / 0.8f);
            if (slushNoise > noise) { noise -= slushNoise / 3; }
            if (slushNoise < noise) { noise += slushNoise / 3; }
            return noise;
        }

        public float NoisePatternInvert(float noise)
        {
            noise = -noise;
            return noise;
        }*/
    }
}