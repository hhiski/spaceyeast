using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameConfig", menuName = "ScriptableObjects/GameConfig", order = 1)]
public class GameConfig : ScriptableObject
{
    [Range(1, 50)] public int AvgStarNumPeCluster = 5;
    [Range(0, 20)] public int AvgPlanetNumPerStar = 5;
    [Range(100, 500)] public int MaxSolarRadius = 400;
    [Range(1, 28)] public int GalacticMapZoneNumber = 20; // the current galaxy .fbx holds 29 child objects


}