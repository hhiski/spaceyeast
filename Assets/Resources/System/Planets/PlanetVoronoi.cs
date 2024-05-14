using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VoronoiSpace
{
    class VoronoiFunctions
    {

        class TerrainCellData
        {
            public Vector3 Vertex;
            public Vector3 CellCenterVertex;
            public float Distance; //The closest cell point distance
            public float DistanceB;//The second closest cell point distance
            public float Strenght;
            public float StrenghtB;

            public TerrainCellData(Vector3 vertex)
            {
                Vertex = vertex;
                Distance = 22.9f;
                DistanceB = 22.9f;
                Strenght = 0;
                StrenghtB = 0;

            }
        }
        /*
         // Use a switch statement to perform different actions based on the selected color
        switch (selectedCellPattern)
        {
            case CellularPattern.Red:
                // Do something when Red is selected
                Debug.Log("Red color selected!");
                break;

            case CellularPattern.Green:
                // Do something when Green is selected
                Debug.Log("Green color selected!");
                break;

            case CellularPattern.Blue:
                // Do something when Blue is selected
                Debug.Log("Blue color selected!");
                break;


            default:
                // Handle any other cases or provide a default action
                break;
        }*/

        public static void UpdateCells(ref Vector3[] vertices, ref Color[] colors, int seed, int cellAmount, float cellPower, AnimationCurve cellCurve, string patternType)
        {


            if (cellAmount > 0)
            {
                System.Random Random = new System.Random(seed);


                List<TerrainCellData> cellVertice = new List<TerrainCellData>();
                List<Vector3> cellCenters = new List<Vector3>();

                string cellPatternType = patternType;
                int cellNumber = cellAmount;

                foreach (Vector3 vertex in vertices)
                {
                    TerrainCellData cellVertex = new TerrainCellData(vertex);
                    cellVertice.Add(cellVertex);
                }

                int density = cellVertice.Count / cellNumber;
                for (int i = 0; i < cellVertice.Count; i = i + density)
                {
                    Vector3 cellCenter = cellVertice[i].Vertex;
                    cellCenters.Add(cellCenter);
                }

                float distance;
                float magnitudeMultiplier;
                float colorMultiplier;

                for (int i = 0; i < cellVertice.Count; i++)
                {
                    for (int j = 0; j < cellCenters.Count; j++)
                    {

                        distance = Vector3.Distance(cellVertice[i].Vertex, cellCenters[j]);
                        if (distance < cellVertice[i].Distance)
                        {
                            cellVertice[i].CellCenterVertex = cellCenters[j];
                            cellVertice[i].Distance = distance;
                        }
                    }

                    for (int j = 0; j < cellCenters.Count; j++)
                    {

                        distance = Vector3.Distance(cellVertice[i].Vertex, cellCenters[j]);
                        if (distance > cellVertice[i].Distance && distance < cellVertice[i].DistanceB)
                        {
                            cellVertice[i].DistanceB = distance;
                        }
                    }
                }


                for (int i = 0; i < cellVertice.Count; i++)
                {

                    float strenght = (cellVertice[i].DistanceB - cellVertice[i].Distance) / cellVertice[i].DistanceB; //relative F2-F1
                    float strenghtB = ( cellVertice[i].Distance) / cellVertice[i].DistanceB; 

                    cellVertice[i].Strenght = strenght;
                    cellVertice[i].StrenghtB = strenghtB;
                }


                float cellStrenght;
                Color coreColor = new Color(0, 0, 0);
                Color tholinColor = new Color(0, 0, 0);
                Color darkerColor = new Color(0, 0, 0);
                Color cellColorGradient = new Color(0, 0, 0);

                if (cellPatternType == "crack")
                {
                    for (int i = 0; i < cellVertice.Count; i++)
                    {
                        magnitudeMultiplier = Mathf.Clamp(cellVertice[i].Strenght - 0.2f, -0.2f, 0f);
                        vertices[i] = vertices[i] * (1 + magnitudeMultiplier * cellPower);

                    
                    }
                }


                if (cellPatternType == "2")
                {
                    for (int i = 0; i < cellVertice.Count; i++)
                    {
                        float curveMultiplier = cellCurve.Evaluate(cellVertice[i].Strenght);

                        vertices[i] = vertices[i] * (curveMultiplier * cellPower);


                    }
                }

                if (cellPatternType == "3")
                {
                    for (int i = 0; i < cellVertice.Count; i++)
                    {
                        float curveMultiplier = cellCurve.Evaluate(cellVertice[i].StrenghtB);
                        vertices[i] = vertices[i] * ( curveMultiplier * cellPower);
                    }
                }

                if (cellPatternType == "d")
                {
                    for (int i = 0; i < cellVertice.Count; i++)
                    {
                        float curveMultiplier = cellCurve.Evaluate(cellVertice[i].StrenghtB- cellVertice[i].Strenght);
                        vertices[i] = vertices[i] * (curveMultiplier * cellPower);
                    }
                }


               
            }
        }


    /*    public static void UpdateCells(ref Vector3[] vertices, ref Color[] colors, int seed, int cellAmount, float cellPower, int pattern)
        {


            if (cellAmount > 0)
            {
                System.Random Random = new System.Random(seed);


                List<TerrainCellData> cellVertice = new List<TerrainCellData>();
                List<Vector3> cellCenters = new List<Vector3>();

                int cellPatternType = pattern;
                int cellNumber = cellAmount;

                foreach (Vector3 vertex in vertices)
                {
                    TerrainCellData cellVertex = new TerrainCellData(vertex);
                    cellVertice.Add(cellVertex);
                }

                int density = cellVertice.Count / cellNumber;
                for (int i = 0; i < cellVertice.Count; i = i + density)
                {
                    Vector3 cellCenter = cellVertice[i].Vertex;
                    cellCenters.Add(cellCenter);
                }

                float distance;
                float magnitudeMultiplier;
                float colorMultiplier;

                for (int i = 0; i < cellVertice.Count; i++)
                {
                    for (int j = 0; j < cellCenters.Count; j++)
                    {

                        distance = Vector3.Distance(cellVertice[i].Vertex, cellCenters[j]);
                        if (distance < cellVertice[i].Distance)
                        {
                            cellVertice[i].CellCenterVertex = cellCenters[j];
                            cellVertice[i].Distance = distance;
                        }
                    }

                    for (int j = 0; j < cellCenters.Count; j++)
                    {

                        distance = Vector3.Distance(cellVertice[i].Vertex, cellCenters[j]);
                        if (distance > cellVertice[i].Distance && distance < cellVertice[i].DistanceB)
                        {
                            cellVertice[i].DistanceB = distance;
                        }
                    }
                }


                for (int i = 0; i < cellVertice.Count; i++)
                {

                    float strenght = (cellVertice[i].DistanceB - cellVertice[i].Distance);
                    strenght = strenght / cellVertice[i].DistanceB;
                    cellVertice[i].Strenght = strenght;
                }


                float cellStrenght;
                Color coreColor = new Color(0, 0, 0);
                Color tholinColor = new Color(0, 0, 0);
                Color darkerColor = new Color(0, 0, 0);
                Color cellColorGradient = new Color(0, 0, 0);

                if (cellPatternType == 1)
                {
                    for (int i = 0; i < cellVertice.Count; i++)
                    {
                        cellStrenght = cellPower * cellVertice[i].Strenght;
                        colors[i] = new Color(cellStrenght, cellStrenght, cellStrenght);
                    }
                }


                if (cellPatternType == 2)
                {
                    for (int i = 0; i < cellVertice.Count; i++)
                    {

                        magnitudeMultiplier = Mathf.Clamp(cellVertice[i].Strenght - 0.2f, -0.2f, 0f); 
                        colorMultiplier =  Mathf.InverseLerp(-0.2f, 0f, magnitudeMultiplier);

                        vertices[i] = vertices[i] * (1 + magnitudeMultiplier * cellPower);

                        cellColorGradient = Color.Lerp(colors[i]*Color.grey, colors[i], colorMultiplier);
                        colors[i] = cellColorGradient;
                    }
                }

                if (cellPatternType == 3)
                {
                    for (int i = 0; i < cellVertice.Count; i++)
                    {
                        magnitudeMultiplier = Mathf.Clamp(cellVertice[i].Strenght - 0.5f, -0.5f, 0.5f);
                        colorMultiplier = Mathf.InverseLerp(-0.5f, 0.5f, magnitudeMultiplier);

                        vertices[i] = vertices[i] * (1 + magnitudeMultiplier * cellPower) ;

                        cellColorGradient = Color.Lerp(colors[i] * Color.grey, colors[i], colorMultiplier);
                        colors[i] = cellColorGradient;


                    }
                }

                if (cellPatternType == 4)
                {
                    for (int i = 0; i < cellVertice.Count; i++)
                    {
                        magnitudeMultiplier = Mathf.Clamp(cellVertice[i].Strenght - 0.5f, -0.5f, 0.5f);
                        colorMultiplier = Mathf.InverseLerp(-0.5f, 0.5f, magnitudeMultiplier);

                        vertices[i] = vertices[i] * (1 + magnitudeMultiplier * cellPower);

                        cellColorGradient = Color.Lerp(colors[i] * Color.grey, colors[i], colorMultiplier);



                    }
                }
                if (cellPatternType == 5)
                {
                    for (int i = 0; i < cellVertice.Count; i++)
                    {
                        magnitudeMultiplier = Mathf.Clamp(cellVertice[i].Strenght - 0.5f, -0.5f, 0.5f);
                        colorMultiplier = Mathf.InverseLerp(-0.5f, 0.5f, magnitudeMultiplier);

                        vertices[i] = vertices[i] * (1 + magnitudeMultiplier * cellPower);

                        cellColorGradient = Color.Lerp(colors[i] * Color.grey, colors[i], colorMultiplier);
                        colors[i] = cellColorGradient;

                    }
                }

                if (cellPatternType == 6)
                {
                    for (int i = 0; i < cellVertice.Count; i++)
                    {
                        int pseudorandom = (int)(cellVertice[i].CellCenterVertex.x);
                        pseudorandom = pseudorandom % 8;
                        pseudorandom = pseudorandom - 4;

                        magnitudeMultiplier = (pseudorandom/10);

                        colorMultiplier = Mathf.InverseLerp(-0.5f, 0.5f, magnitudeMultiplier);

                        vertices[i] = vertices[i] * (1 + magnitudeMultiplier * cellPower);

                        cellColorGradient = Color.Lerp(colors[i] * Color.grey, colors[i], colorMultiplier);
                        colors[i] = cellColorGradient;
                    }
                }
            }
        }

    */


    }
}