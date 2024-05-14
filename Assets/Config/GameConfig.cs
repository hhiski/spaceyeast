using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameConfig", menuName = "ScriptableObjects/GameConfig", order = 1)]
public class GameConfig : ScriptableObject
{
    [Range(1, 50)] public int AvgStarNumPeCluster;

    [Range(0, 20)] public int AvgPlanetNumPerStar;

}