using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#region biosphere classes
public class Biosphere
{
    // 0 - Lifeless
    // 1 - Unicellular 
    // 2 - Multicellular
    // 3 - Civilizations
    int BiosphereLevel { get; set; }

    //Species here?

    public Biosphere(int level)
    {
        BiosphereLevel = level;
    }

    public Biosphere(float lifeProbability)
    {
        BiosphereLevel = GetRandomBiosphereLevel(lifeProbability);
    }

    public Biosphere()
    {
        BiosphereLevel = 0;
    }

    public int GetBiosphereLevel()
    {
        return BiosphereLevel;
    }

    private int GetRandomBiosphereLevel(float lifeProbability)
    {

        float lifeProbabilityTest = Random.Range(0, 1f);
        float biosphereLevelTest = Random.Range(0, 1f);

        int biosphereLevel = 0;

        if (lifeProbabilityTest <= lifeProbability) // life on the planet!
        {
            if (biosphereLevelTest <= 0.75f)
            {
                biosphereLevel = 1;
            }
            else if (biosphereLevelTest <= 0.95f)
            {
                biosphereLevel = 2;
            }
            else
            {
                biosphereLevel = 3;
            }
        }
        else
        {
            biosphereLevel = 0;
        }
        return biosphereLevel;

    }



};
#endregion