using UnityEngine;

namespace Hub
{
    public class HubState : MonoBehaviour
    {
        private static HubState Instance { get; set; }

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
        }
        
        
        private int globalTime;
        private int currentTimeOfTheDay;
        
        //TODO: change logic to reading available seeds from file/other object
        [SerializeField] private InventoryEntrySo[] availableSeeds;


        public static HubState GetInstance()
        {
            return Instance;
        }

        public InventoryEntrySo[] GetAvailableSeeds()
        {
            return availableSeeds;
        }
    }
}
