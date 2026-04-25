using System;
using System.Collections.Generic;
using System.Linq;
using Hub.Hives;
using UnityEngine;

namespace Hub
{
    public class HubState : MonoBehaviour
    {
        private static HubState Instance { get; set; }
        public event Action OnTimePassed;
        public event Action OnPlantsChanged;

        [SerializeField] private InventoryEntrySo[] availableSeeds;
        [SerializeField] private List<BeeSo> beeList;
        public List<PlantSo> availableGrownPlantsList = new List<PlantSo>();
        public List<PlantSo> usedGrownPlantsList = new List<PlantSo>();

        private void Awake()
        {
            if (Instance == null) Instance = this;
        }

        public static HubState GetInstance()
        {
            if (Instance == null)
            {
                Instance = FindFirstObjectByType<HubState>();
            }
            return Instance;
        }

        public InventoryEntrySo[] GetAvailableSeeds() => availableSeeds;

        public void AddGrownPlant(PlantSo plant)
        {
            if (!availableGrownPlantsList.Contains(plant) && plant.plantType != PlantSo.PlantType.NullPlant)
            {
                availableGrownPlantsList.Add(plant);
                OnPlantsChanged?.Invoke();
            }
        }

        public void RemoveGrownPlant(PlantSo plant)
        {
            if (availableGrownPlantsList.Contains(plant))
            {
                availableGrownPlantsList.Remove(plant);
                OnPlantsChanged?.Invoke();
            }
        }

        public void UseGrownPlant(PlantSo.PlantType plantType)
        {
            PlantSo plantToUse = availableGrownPlantsList.FirstOrDefault(p => p.plantType == plantType);
    
            if (plantToUse != null)
            {
                availableGrownPlantsList.Remove(plantToUse);
                usedGrownPlantsList.Add(plantToUse);
                OnPlantsChanged?.Invoke();
            }
        }

        public int GetCountAvailablePlantsByType(PlantSo.PlantType plantType)
        {
            int count = 0;
            foreach (PlantSo p in availableGrownPlantsList)
            {
                if (p.plantType == plantType)  count++;
            }
            return count;
        }

        public void PassTime() => OnTimePassed?.Invoke();

        public void UnuseGrownPlant(PlantSo.PlantType plantType)
        {
            PlantSo p = usedGrownPlantsList.FirstOrDefault(p => p.plantType == plantType);

            if (p != null)
            {
                usedGrownPlantsList.Remove(p);
                availableGrownPlantsList.Add(p);
                OnPlantsChanged?.Invoke();
            }
        }

        public List<BeeSo> GetBeeList()
        {
            return beeList;
        }
    }
}