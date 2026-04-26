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
        public event Action OnHoneyAmountChanged;

        [SerializeField] private List<Honey> baseHoneyList = new List<Honey>();
        [SerializeField] private InventoryEntrySo[] availableSeeds;
        [SerializeField] private List<BeeSo> baseBeeList;
        [HideInInspector] private List<BeeSo> beeList = new List<BeeSo>();
        [SerializeField] private PlantSo nullPlant;
        
        [HideInInspector] public List<PlantSo> unassignedGrownPlantsList = new List<PlantSo>();
        [HideInInspector] public List<PlantSo> availableGrownPlantsList = new List<PlantSo>();
        [HideInInspector] public List<PlantSo> usedGrownPlantsList = new List<PlantSo>();
        
        [HideInInspector] public Dictionary<Honey, int> honeyInventory = new Dictionary<Honey, int>();

        private void Awake()
        {
            if (Instance == null) Instance = this;
            
            foreach (BeeSo bee in baseBeeList)
            {
                beeList.Add(Instantiate(bee));
            }

            foreach (Honey honey in baseHoneyList)
            {
                honeyInventory.Add(honey, 0);
            }
        }
        
        public void AddHoney(Honey honey, int amount = 1)
        {
            if (!honeyInventory.TryAdd(honey, amount))
            {
                honeyInventory[honey] += amount;
            }
            OnHoneyAmountChanged?.Invoke();
            Debug.Log($"Obtained {amount} {honey.honeyName}; Total: {honeyInventory[honey]}");
        }
        
        public void ResetWorkforce()
        {
            unassignedGrownPlantsList.AddRange(availableGrownPlantsList);
            unassignedGrownPlantsList.AddRange(usedGrownPlantsList);
    
            availableGrownPlantsList.Clear();
            usedGrownPlantsList.Clear();

            foreach (BeeSo bee in beeList)
            {
                bee.assignedPlantSo = nullPlant; 
            }

            OnPlantsChanged?.Invoke();
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
            if (!unassignedGrownPlantsList.Contains(plant) && plant.plantType != PlantSo.PlantType.NullPlant)
            {
                unassignedGrownPlantsList.Add(plant);
                OnPlantsChanged?.Invoke();
            }
        }

        public void RemoveGrownPlant(PlantSo plant)
        {
            bool wasRemoved = false;

            if (unassignedGrownPlantsList.Contains(plant))
            {
                unassignedGrownPlantsList.Remove(plant);
                wasRemoved = true;
            }
            if (availableGrownPlantsList.Contains(plant))
            {
                availableGrownPlantsList.Remove(plant);
                wasRemoved = true;
            }
            if (usedGrownPlantsList.Contains(plant))
            {
                usedGrownPlantsList.Remove(plant);
                wasRemoved = true;
            }

            if (wasRemoved)
            {
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

        public List<PlantSo> GetUnassignedPlants()
        {
            return unassignedGrownPlantsList;
        }

        public void AssignPlant(PlantSo plant)
        {
            if (unassignedGrownPlantsList.Contains(plant))
            {
                unassignedGrownPlantsList.Remove(plant);
                availableGrownPlantsList.Add(plant);
                OnPlantsChanged?.Invoke();
            }
        }

        public void UnassignPlant(PlantSo plant)
        {
            if (availableGrownPlantsList.Contains(plant))
            {
                availableGrownPlantsList.Remove(plant);
                unassignedGrownPlantsList.Add(plant);
                OnPlantsChanged?.Invoke();
            }
        }
        
        public bool IsPlantUsedInRecipe(PlantSo plant)
        {
            if (plant == null || plant.plantType == PlantSo.PlantType.NullPlant) return false;
    
            return usedGrownPlantsList.Contains(plant);
        }

        public Dictionary<Honey, int> GetHoneyAmount()
        {
            return honeyInventory;
        }
    }
}