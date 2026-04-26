using UnityEngine;

namespace Hub
{
    [CreateAssetMenu()]
    public class PlantSo: ScriptableObject
    {
        public enum PlantType
        {
            Buckwheat,
            BloomingSally,
            NullPlant
        }

        public new string name;
        public PlantType plantType;
        public int growTimeMax;
        public Sprite plantSprite;
        
        public GameObject grownPlantPrefab;
        public GameObject notGrownPlantPrefab;
        
        private int currentGrowTime = 0;
        
        public bool IsGrown => currentGrowTime >= growTimeMax;
        
        public void GrowByOne()
        {
            if (currentGrowTime < growTimeMax)
            {
                currentGrowTime++;
            }
            else
            {
                Debug.Log("Grow time already grown");
            }
        }
        
        public int GetGrowthTime()
        {
            return currentGrowTime;
        }

        public void SetGrowthTime(int potDataCurrentGrowth)
        {
            this.currentGrowTime = potDataCurrentGrowth;
        }
    }
}