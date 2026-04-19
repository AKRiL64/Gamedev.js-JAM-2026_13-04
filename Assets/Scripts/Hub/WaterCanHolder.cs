using System;
using System.Linq;
using UnityEngine;

namespace Hub
{
    public class WaterCanHolder: MonoBehaviour
    {
        [SerializeField] private Transform waterCanHoldingPoint;
        [SerializeField] private InteractiveObject interactiveObject;
        [SerializeField] private InventoryEntrySo waterCanSo;
        private bool hasWaterCan = true;

        private void Start()
        {
            interactiveObject.OnInteract += Interaction;
        }

		private void Interaction(GameObject player) 
        {
            if (hasWaterCan)
            {
                if (TryGiveWaterCan(player))
                {
                    DeleteWaterCan();
                }
            }
            else
            {
                if (TryTakeWaterCan(player))
                {
                    AddWaterCan();
                }
            }
        }

        private void DeleteWaterCan()
        {
            hasWaterCan = false;
            foreach (Transform child in waterCanHoldingPoint.transform)
            {
                Destroy(child.gameObject);
            }
        }

        private void AddWaterCan()
        {
            hasWaterCan = true;
            GameObject waterCan = Instantiate(waterCanSo.prefab, waterCanHoldingPoint.position, waterCanHoldingPoint.rotation);
            waterCan.transform.SetParent(waterCanHoldingPoint);
            waterCan.transform.localPosition = Vector3.zero;
            waterCan.transform.localRotation = Quaternion.identity;
        }
        
        private bool TryGiveWaterCan(GameObject player)
        {
            PlayerItemSlotController playerItemSlotController = player.GetComponent<PlayerItemSlotController>();
            if (!playerItemSlotController)
            {
                Debug.LogWarning("PlayerItemSlotController not found");
                return false;
            }

            if (playerItemSlotController.HasItem)
            {
                Debug.LogWarning("Player already has an item");
                return false;
            }

            if (playerItemSlotController.SetItem(waterCanSo))
            {
                return true;
            }
            
            Debug.LogWarning("Could not give water can to a player");
            return false;
        }

        private bool TryTakeWaterCan(GameObject player)
        {
            PlayerItemSlotController playerItemSlotController = player.GetComponent<PlayerItemSlotController>();
            if (!playerItemSlotController)
            {
                Debug.LogWarning("PlayerItemSlotController not found");
                return false;
            }

            if (!playerItemSlotController.HasItem)
            {
                Debug.LogWarning("Nothing to take from player");
                return false;
            }

            InventoryEntrySo item = playerItemSlotController.CurrentItem;
            if (item.itemTypes.Contains(InventoryEntrySo.ItemTypes.WateringCan))
            {
                playerItemSlotController.ClearItem();
                return true;
            }
            Debug.LogWarning("Item player holds is not a watering can");
            return false;
        }
    }
}