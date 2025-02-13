using System.Collections;
using System.Collections.Generic;
using UnityEngine;





    public class StarType
    {
        public int Id = 0;
        public string Name = "EmptyType";
        public Color StarColor;
        public Color StarColorCold;
        public Color StarLightColor;
        public float[] TemperatureRange = new float[1]; //min and max values
       

        public StarType(int id)
        {
            Id = id;
        }
        public StarType(string name)
        {
            Name = name;
        }
        public StarType() { }
    }

    public class StarFormation
    {

        public StarType GetRandomTypeStar()
        {

            List<StarType> EveryStarTypeList = new List<StarType>();
            EveryStarTypeList = PopulateStarList();

            int random = Random.Range(0, EveryStarTypeList.Count);
            StarType randomType = EveryStarTypeList[random];
            return randomType;
        }

        public StarType GetSpecificTypeStar(string starTypeName) //kesken lis‰‰ astron
        {
            bool viable = false;
            List<StarType> EveryStarTypeList = new List<StarType>(12);
            EveryStarTypeList = PopulateStarList();
            StarType type = new StarType();

            foreach (StarType specificType in EveryStarTypeList)
            {
                if (specificType.Name == starTypeName)
                {
                    type = specificType;
                    viable = true;
                    break;
                }
            }

            if (viable == false)
            {
                Debug.Log("CANT FIND STAR TYPE :" + starTypeName + " - USING RANDOM TYPE");
                type = GetRandomTypeStar();

            }

            return type;
        }


        public float getStarTypeRandomTemperature(StarType type)
        {
            float solarTemperature = Random.Range(type.TemperatureRange[0], type.TemperatureRange[1]);
            return solarTemperature;
        }

        public List<StarType> PopulateStarList()
        {
            List<StarType> EveryStarTypeList = new List<StarType>(12);

            StarType O = new StarType();
            O.Id = 0;
            O.Name = "O-type";
            O.StarColor = new Color(0.0f, 0, 1f, 1f);
            O.StarColorCold = new Color(0.82f, 0.85f, 1f, 1f);
            O.StarLightColor = new Color(0f, 0f, 1f, 1f);
            O.TemperatureRange = new float[] { 35000, 45000 };
            EveryStarTypeList.Add(O);

            StarType B = new StarType();
            B.Id = 1;
            B.Name = "B-type";
            B.StarColor = new Color(0.487f, 0.78f, 0.8f, 1f);
            B.StarColorCold = new Color(0.0f, 0.76f, 1f, 1f);
            B.StarLightColor = new Color(0.0f, 0.61f, 1f, 1f);
            B.TemperatureRange = new float[] { 10000, 20000 };
            EveryStarTypeList.Add(B);

            StarType A = new StarType();
            A.Id = 2;
            A.Name = "A-type";
            A.StarColor = new Color(0.792f, 0.792f, 0.792f, 1f);
            A.StarColorCold = new Color(0.786f, 1f, 0.984f, 1f);
            A.StarLightColor = new Color(0.022f, 1f, 0.859f, 1f);
            A.TemperatureRange = new float[] { 7500, 10000 };
            EveryStarTypeList.Add(A);

            StarType F = new StarType();
            F.Id = 3;
            F.Name = "F-type";
            F.StarColor = new Color(0.99f, 1f, 0.74f, 1f);
            F.StarColorCold = new Color(0.942f, 0.94f, 0.554f, 1f);
            F.StarLightColor = new Color(0.985f, 1f, 0.164f, 1f);
            F.TemperatureRange = new float[] { 6000, 7500 };
            EveryStarTypeList.Add(F);

            StarType G = new StarType();
            G.Id = 4;
            G.Name = "G-type";
            G.StarColor = new Color(0.857f, 0.871f, 0.41f, 1f);
            G.StarColorCold = new Color(1f, 0.63f, 0.202f, 1f);
            G.StarLightColor = new Color(1f, 0.9f, 0.36f, 1f);
            G.TemperatureRange = new float[] { 5200, 6000 };
            EveryStarTypeList.Add(G);

            StarType K = new StarType();
            K.Id = 5;
            K.Name = "K-type";
            K.StarColor = new Color(0.86f, 0.46f, 0.0f, 1f);
            K.StarColorCold = new Color(1f, 0.06f, 0.0f, 1f);
            K.StarLightColor = new Color(1f, 0.35f, 0.00f, 1f);
            K.TemperatureRange = new float[] { 3700, 5200 };
            EveryStarTypeList.Add(K);

            StarType M = new StarType();
            M.Id = 6;
            M.Name = "M-type";
            M.StarColor = new Color(1f, 0.37f, 0.43f, 1f);
            M.StarColorCold = new Color(0.67f, 0.00f, 0.00f, 1f);
            M.StarLightColor = new Color(0.69f, 0.00f, 0.05f, 1f);
            M.TemperatureRange = new float[] { 2400, 3700 };
            EveryStarTypeList.Add(M);

            return EveryStarTypeList;
        }
    }


