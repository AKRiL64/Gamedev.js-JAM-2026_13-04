using System;
using System.Linq;
using SaveSystem;
using UnityEngine;

namespace Hub
{
    public class PlantPot : MonoBehaviour
    {
        public event Action<PlantSo> OnPlantChanged;

        public string potID;
        
        [Header("References")]
        [SerializeField] private Transform potTopPoint;
        [SerializeField] private InteractiveObject interactiveObject;
        [SerializeField] private GameObject closingWindow;
        [SerializeField] public PlantSo nullPlantSo;
        [SerializeField] public GameObject needsWaterPrompt;
        [SerializeField] public PlantPotGround plantPotGround;

        private PlantSo plantSo;
        private bool isWatered = false;

        public bool IsWatered
        {
            get => isWatered;
            set
            {
                isWatered = value;
                if (plantPotGround) plantPotGround.SetWatered(isWatered);
                
                if (needsWaterPrompt)
                {
                    bool isRealPlant = plantSo != null && plantSo.plantType != PlantSo.PlantType.NullPlant;
                    needsWaterPrompt.SetActive(!isWatered && isRealPlant);
                }
            }
        }

        private void Awake()
        {
            
        }

        private void Start()
        {
            ChangePlant(nullPlantSo);
            interactiveObject.OnInteract += Interaction;
        }

        public void ChangePlant(PlantSo newPlantSo)
        {
            PlantSo oldPlant = plantSo;
            
            if (oldPlant != null && oldPlant.IsGrown)
            {
                var hub = HubState.GetInstance();
                if (hub != null) hub.RemoveGrownPlant(oldPlant);
            }

            if (newPlantSo != plantSo && newPlantSo != null && newPlantSo.plantType != PlantSo.PlantType.NullPlant)
            {
                plantSo = Instantiate(newPlantSo);
            }
            else
            {
                plantSo = newPlantSo;
            }

            ClearTopPoint();
    
            if (plantSo != null && plantSo.plantType != PlantSo.PlantType.NullPlant)
            {
                UpdatePlant();
                if (newPlantSo != plantSo) IsWatered = false; 
            }
            else
            {
                IsWatered = false;
            }
            
            if (plantSo != null && plantSo.IsGrown)
            {
                var hub = HubState.GetInstance();
                if (hub != null) hub.AddGrownPlant(plantSo);
            }

            OnPlantChanged?.Invoke(plantSo);
        }

        public void GrowPlant()
        {
            if (plantSo == null || plantSo.plantType == PlantSo.PlantType.NullPlant) return;

            if (!IsWatered)
            {
                ChangePlant(nullPlantSo);
            }
            else
            {
                bool wasGrown = plantSo.IsGrown;
                
                plantSo.GrowByOne();
                IsWatered = false; 
                
                ClearTopPoint();
                UpdatePlant();
                
                if (!wasGrown && plantSo.IsGrown) 
                {
                    var hub = HubState.GetInstance();
                    if (hub != null) hub.AddGrownPlant(plantSo);
                }
                
                OnPlantChanged?.Invoke(plantSo);
            }
        }

        private void UpdatePlant()
        {
            GameObject prefab = !plantSo.IsGrown ? plantSo.notGrownPlantPrefab : plantSo.grownPlantPrefab;
            if (prefab)
            {
                GameObject plant = Instantiate(prefab, potTopPoint.position, potTopPoint.rotation);
                plant.transform.SetParent(potTopPoint);
            }
        }
        

        private void ClearTopPoint()
        {
            foreach (Transform child in potTopPoint) Destroy(child.gameObject);
        }

        private void Interaction(GameObject player)
        {
            PlayerItemSlotController slot = player.GetComponentInParent<PlayerItemSlotController>();
            if (!slot) return;

            InventoryEntrySo item = slot.CurrentItem;
            
            if (item && item.itemTypes.Contains(InventoryEntrySo.ItemTypes.Seed) && TryToPlant(item))
            {
                slot.ClearItem();
            } 
            else if (item && !isWatered && item.itemTypes.Contains(InventoryEntrySo.ItemTypes.WateringCan) && plantSo.plantType != PlantSo.PlantType.NullPlant)
            {
                IsWatered = true;
            }
            else
            {
                closingWindow.SetActive(true);
            }
        }

        private bool TryToPlant(InventoryEntrySo entrySo)
        {
            if (plantSo.plantType != PlantSo.PlantType.NullPlant) return false;
            SeedSo seedSo = entrySo as SeedSo;
            if (seedSo == null) return false;
            
            ChangePlant(seedSo.plantSo);
            return true;
        }

        public PlantSo GetPlantSo() => plantSo;

        private void OnDestroy() => interactiveObject.OnInteract -= Interaction;
        
        public void SavePotData()
        {
            var data = SaveManager.Instance.CurrentData;
    
            var potData = data.pots.Find(p => p.potID == this.potID);
    
            if (potData.potID == null)
            {
                potData = new PotSaveData { potID = this.potID };
                data.pots.Add(potData);
            }
            
            potData.plantName = plantSo != null ? plantSo.name : nullPlantSo.name;
            potData.currentGrowth = plantSo != null ? plantSo.GetGrowthTime() : 0;
            potData.isWatered = this.IsWatered;
    
            int index = data.pots.FindIndex(p => p.potID == this.potID);
            if(index != -1) data.pots[index] = potData;
        }
        
        
        public void LoadPotData()
        {
            var data = SaveManager.Instance.CurrentData;
            var potSave = data.pots.Find(p => p.potID == this.potID);

            if (!string.IsNullOrEmpty(potSave.potID))
            {
                PlantSo basePlant = SaveManager.Instance.gameDatabase.GetPlantByName(potSave.plantName);
                ChangePlant(basePlant);
        
                if (plantSo != null && plantSo.plantType != PlantSo.PlantType.NullPlant)
                {
                    plantSo.SetGrowthTime(potSave.currentGrowth);
                    ClearTopPoint();
                    UpdatePlant();
                }
        
                IsWatered = potSave.isWatered;
            }
        }
    }
}