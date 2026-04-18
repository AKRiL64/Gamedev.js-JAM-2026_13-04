using System;
using System.Linq;
using UI;
using UnityEngine;

namespace Hub
{
    public class PlantPot: MonoBehaviour
    {
        public event Action<PlantSo> OnPlantChanged;
        
        [SerializeField] private Transform potTopPoint;
        [SerializeField] private InteractiveObject interactiveObject;
        [SerializeField] private GameObject closingWindow;
        [SerializeField] public PlantSo nullPlantSo;
        private PlantSo plantSo;

        public void ChangePlant(PlantSo newPlantSo)
        {
            plantSo = newPlantSo;
            ClearTopPoint();
            
            if (newPlantSo.plantType != PlantSo.PlantType.NullPlant)
            {
                UpdatePlant();
            }
            OnPlantChanged?.Invoke(plantSo);
        }

        public void GrowPlant()
        {
            if (plantSo.plantType == PlantSo.PlantType.NullPlant) return;
            plantSo.GrowByOne();
        }

        private void UpdatePlant()
        {
            GameObject plant = Instantiate(!plantSo.IsGrown ? plantSo.notGrownPlantPrefab : plantSo.grownPlantPrefab,
                potTopPoint.position, potTopPoint.rotation);
            plant.transform.SetParent(potTopPoint);
        }

        private void ClearTopPoint()
        {
            foreach (Transform child in potTopPoint)
            {
                Destroy(child.gameObject);
            }
        }

        private void Interaction(GameObject player)
        {
            PlayerItemSlotController playerItemSlotController = player.GetComponentInParent<PlayerItemSlotController>();
            if (!playerItemSlotController)
            {
                Debug.LogWarning("PlayerItemSlotController not found");
                return;
            }
            InventoryEntrySo item = playerItemSlotController.CurrentItem;
            if (item && item.itemTypes.Contains(InventoryEntrySo.ItemTypes.Seed) && TryToPlant(item))
            {
                playerItemSlotController.ClearItem();
            }
            else
            {
                ShowUi();
            }
        }

        private bool TryToPlant(InventoryEntrySo entrySo)
        {
            if (plantSo.plantType != PlantSo.PlantType.NullPlant)
            {
                Debug.LogWarning("Cant plant: PlantSo is not null");
                return false;
            }
            
            SeedSo seedSo = entrySo as SeedSo;
            if (seedSo == null)
            {
                Debug.LogWarning("Cant plant: SeedSo is null");
                return false;
            }
            
            ChangePlant(seedSo.plantSo);
            return true;
        }
        
        private void ShowUi()
        {
            closingWindow.SetActive(true);
        }

        private void Awake()
        {
            ChangePlant(nullPlantSo);
        }
        private void Start()
        {
            interactiveObject.OnInteract += Interaction;
        }

        public PlantSo GetPlantSo()
        {
            return plantSo;
        }
    }
}