
using UnityEngine;

namespace Game.ID 
{
    public class IDManager : MonoBehaviour
    {

        public static IDManager Instance { get; private set; }

        int NumberOfCreatedCluster = 0;
        int NumberOfCreatedStars = 0;
        int NumberOfCreatedPlanets = 0;

        public int GetUniquePlanetId()
        {
            int id = NumberOfCreatedPlanets;
            NumberOfCreatedPlanets++;
            return id;
        }
        public int GetUniqueStarId()
        {
            int id = NumberOfCreatedStars;
            NumberOfCreatedStars++;
            return id;
        }
        public int GetUniqueClusterId()
        {
            int id = NumberOfCreatedCluster;
            NumberOfCreatedCluster++;
            return id;
        }

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



      
    }

}