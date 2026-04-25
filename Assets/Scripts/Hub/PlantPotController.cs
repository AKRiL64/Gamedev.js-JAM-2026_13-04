using UnityEngine;

namespace Hub
{
    public class PlantPotController: MonoBehaviour
    {
        [SerializeField] private PlantPot[] plantPots;
        private HubState hubState;

        private void Start()
        {
            hubState = HubState.GetInstance();
            hubState.OnTimePassed += GrowAllPlants;
        }
        
        public void GrowAllPlants()
        {
            foreach (var plantPot in plantPots)
            {
                plantPot.GrowPlant();
            }
        }
    }
}