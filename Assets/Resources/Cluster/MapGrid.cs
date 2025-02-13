using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Lines;

public class MapGrid : MonoBehaviour
{
    [SerializeField] LineType LineType = LineType.Galactic;
    public int GridLineNumber = 40;
    public int LineSpace = 10;
    public int LineCircleNumber = 0;
    public float LineCircleSpace = 5;
    public bool drawGrid = true;
    public bool drawCircles = false;

    void Start()
    {

        Vector3 horizontalLineStartPos,verticalLineStartPos, horizontalLineEndPos, verticalLineEndPos;

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


                GameObject line = LineManager.Instance.CreateLineObject(this.transform, "Circle", circleLineSegments, LineType);
                line.GetComponent<LineRenderer>().useWorldSpace = true;
                line.GetComponent<LineRenderer>().loop = true;
            }
        }


        if (drawGrid)
        {

            Vector3[] horizontalLineSegments, verticalLineSegments;

            if (lineNum % 2 != 0) { lineNum++; };
            lineNum = lineNum / 2;

            //Horizontal lines
            for (int lineIndex = -lineNum; lineIndex <= lineNum; lineIndex++)
            {


                fullLineLenght = (lineNum * LineSpace);
                lineDistanceNormalized = Mathf.Abs((float)lineIndex / (float)(lineNum));
                lineLenght = fullLineLenght * Mathf.Pow((1 - Mathf.Pow(lineDistanceNormalized, 2)), 0.5f);
                lineAlpha =  (1f - Mathf.Clamp(lineDistanceNormalized - 0.90f, 0f, 1f) * 10f);
                horizontalLineStartPos = transform.position + new Vector3(lineIndex * LineSpace, 0.0f, -lineLenght);
                horizontalLineEndPos = transform.position + new Vector3(lineIndex * LineSpace, 0.0f, lineLenght);
                verticalLineStartPos = transform.position + new Vector3(-lineLenght, 0.0f, lineIndex * LineSpace);
                verticalLineEndPos = transform.position + new Vector3(lineLenght, 0.0f, lineIndex * LineSpace);

                fadeTime = 10f / lineLenght;

                lineGradient.SetKeys(
                   new GradientColorKey[] { new GradientColorKey(new Color(1,1,1,1), 0.0f), new GradientColorKey(new Color(1, 1, 1, 1), 1.0f) },
                   new GradientAlphaKey[] { new GradientAlphaKey(0, 0.0f), new GradientAlphaKey(lineAlpha, fadeTime), new GradientAlphaKey(lineAlpha, 0.5f), new GradientAlphaKey(lineAlpha, 1f - fadeTime), new GradientAlphaKey(0, 1.0f) }
                );

                horizontalLineSegments = new[] { horizontalLineStartPos, (horizontalLineStartPos * 0.9f + horizontalLineEndPos * 0.1f), (horizontalLineStartPos * 0.5f + horizontalLineEndPos * 0.5f), (horizontalLineStartPos * 0.1f + horizontalLineEndPos * 0.9f), horizontalLineEndPos };
                verticalLineSegments = new[] { verticalLineStartPos, (verticalLineStartPos * 0.9f + verticalLineEndPos * 0.1f), (verticalLineStartPos * 0.5f + verticalLineEndPos * 0.5f), (verticalLineStartPos * 0.1f + verticalLineEndPos * 0.9f), verticalLineEndPos };

                GameObject horizontalLine = LineManager.Instance.CreateLineObject(this.transform, "Grid Line", horizontalLineSegments, LineType);
                GameObject verticalLine = LineManager.Instance.CreateLineObject(this.transform, "Grid Line", verticalLineSegments, LineType);

                horizontalLine.GetComponent<LineRenderer>().useWorldSpace = true;
                horizontalLine.GetComponent<LineRenderer>().colorGradient = lineGradient;
            
            }

        }

    }

    

    // Update is called once per frame
    void Update()
    {

    }
}
