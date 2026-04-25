using System;
using System.Linq;
using UnityEngine;

namespace Hub
{
    public class PlantPot : MonoBehaviour
    {
        public event Action<PlantSo> OnPlantChanged;
        
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
    }
}