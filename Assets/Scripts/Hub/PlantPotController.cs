using UnityEngine;

namespace Hub
{
    public class PlantPotController: MonoBehaviour
    {
        [SerializeField] private PlantPot[] plantPots;

        public void GrowAllPlants()
        {
            foreach (var plantPot in plantPots)
            {
                plantPot.GrowPlant();
            }
        }
    }
}