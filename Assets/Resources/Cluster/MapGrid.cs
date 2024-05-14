using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LineSpace;

public class MapGrid : MonoBehaviour
{

    public int GridLineNumber = 40;
    public int LineSpace = 10;
    public float LineThickness = 0.8f;
    public Material LineMaterial;
    public int LineCircleNumber = 0;
    public float LineCircleSpace = 5;
    public bool drawGrid = true;
    public bool drawCircles = false;

    public Color lineColor = new Color(1, 1, 1, 1);
    void Start()
    {

        Vector3 startPos;
        Vector3 endPos;

        int lineNum = GridLineNumber;
        float fullLineLenght = (LineSpace * LineSpace);
        float lineDistanceNormalized;
        float lineLenght;
        float lineAlpha = 1;
        float fadeTime;

      

        Gradient lineGradient = new Gradient();




        if (drawCircles)
        {
            Vector3 parentPos = transform.position;


            for (int lineIndex = 1; lineIndex < LineCircleNumber; lineIndex++)
            {
                Vector3[] circleLineSegments = new Vector3[100];
                int circleSegmentCount = circleLineSegments.Length;

                float orbitalDistance = LineCircleSpace * 2 * lineIndex;


                Vector3 segmentPos = new Vector3(0, 0, 0);
                float angle = 0;
                for (int segmentIndex = 0; segmentIndex < circleSegmentCount; segmentIndex++)
                {
                    segmentPos.x = parentPos.x + Mathf.Sin(Mathf.Deg2Rad * angle) * orbitalDistance;
                    segmentPos.y = parentPos.y;
                    segmentPos.z = parentPos.z + Mathf.Cos(Mathf.Deg2Rad * angle) * orbitalDistance;

                    circleLineSegments[segmentIndex] = segmentPos;
      
                    angle += (360f / circleSegmentCount);
                }

                GameObject line = LineFunctions.CreateLineObject(this.transform, new Vector3(0, 0, 0), "Circle Line", circleLineSegments, LineMaterial, true, lineColor,  LineThickness);
                line.GetComponent<LineRenderer>().useWorldSpace = true;

            }
        }


        if (drawGrid)
        {

            Vector3[] heightLineSegments;

            if (lineNum % 2 != 0) { lineNum++; };
            lineNum = lineNum / 2;

            //Horizontal lines
            for (int lineIndex = -lineNum; lineIndex <= lineNum; lineIndex++)
            {


                fullLineLenght = (lineNum * LineSpace);
                lineDistanceNormalized = Mathf.Abs((float)lineIndex / (float)(lineNum));
                lineLenght = fullLineLenght * Mathf.Pow((1 - Mathf.Pow(lineDistanceNormalized, 2)), 0.5f);
                lineAlpha = LineMaterial.color.a;
                lineColor = LineMaterial.color;
                    //LineColor.a * (1f - Mathf.Clamp(lineDistanceNormalized - 0.90f, 0f, 1f) * 10f);

                startPos = transform.position + new Vector3(lineIndex * LineSpace, 0.0f, -lineLenght);
                endPos = transform.position + new Vector3(lineIndex * LineSpace, 0.0f, lineLenght);

                fadeTime = 10f / lineLenght;

                lineGradient.SetKeys(
                   new GradientColorKey[] { new GradientColorKey(lineColor, 0.0f), new GradientColorKey(lineColor, 1.0f) },
                   new GradientAlphaKey[] { new GradientAlphaKey(0, 0.0f), new GradientAlphaKey(lineAlpha, fadeTime), new GradientAlphaKey(lineAlpha, 0.5f), new GradientAlphaKey(lineAlpha, 1f - fadeTime), new GradientAlphaKey(0, 1.0f) }
                );

                heightLineSegments = new[] { startPos, (startPos * 0.9f + endPos * 0.1f), (startPos * 0.5f + endPos * 0.5f), (startPos * 0.1f + endPos * 0.9f), endPos };
                LineFunctions.CreateLineObject(this.transform, new Vector3(0, 0, 0), "Grid Line", heightLineSegments, LineMaterial, lineGradient, LineThickness);

            }

            //Vertical lines
            for (int lineIndex = -lineNum; lineIndex <= lineNum; lineIndex++)
            {
                fullLineLenght = (lineNum * LineSpace);
                lineDistanceNormalized = Mathf.Abs((float)lineIndex / (float)(lineNum));
                lineLenght = fullLineLenght * Mathf.Pow((1 - Mathf.Pow(lineDistanceNormalized, 2)), 0.5f);
                lineAlpha = lineColor.a * (1f - Mathf.Clamp(lineDistanceNormalized - 0.90f, 0f, 1f) * 10f);

                startPos = transform.position + new Vector3(-lineLenght, 0.0f, lineIndex * LineSpace);
                endPos = transform.position + new Vector3(lineLenght, 0.0f, lineIndex * LineSpace);

                fadeTime = 10f / lineLenght;

                lineGradient.SetKeys(
                   new GradientColorKey[] { new GradientColorKey(lineColor, 0.0f), new GradientColorKey(lineColor, 1.0f) },
                   new GradientAlphaKey[] { new GradientAlphaKey(0, 0.0f), new GradientAlphaKey(lineAlpha, fadeTime), new GradientAlphaKey(lineAlpha, 0.5f), new GradientAlphaKey(lineAlpha, 1f - fadeTime), new GradientAlphaKey(0, 1.0f) }
                );

                heightLineSegments = new[] { startPos, (startPos * 0.9f + endPos * 0.1f), (startPos * 0.5f + endPos * 0.5f), (startPos * 0.1f + endPos * 0.9f), endPos };
                LineFunctions.CreateLineObject(this.transform, new Vector3(0, 0, 0), "Grid Line", heightLineSegments, LineMaterial, lineGradient, LineThickness);

            }

        }

    }

    

    // Update is called once per frame
    void Update()
    {

    }
}
