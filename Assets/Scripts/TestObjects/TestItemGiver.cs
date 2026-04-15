using System;
using Hub;
using UnityEngine;

public class TestItemGiver : MonoBehaviour
{
    [SerializeField] InteractiveObject interactiveObject;
    [SerializeField] private InventoryEntrySo inventoryEntrySo;

    public void Start()
    {
        interactiveObject.OnInteract += GiveItem;
    }

    private void GiveItem(GameObject player)
    {
        PlayerItemSlotController playerItemSlotController = player.GetComponent<PlayerItemSlotController>();
        
        if (!playerItemSlotController.SetItem(inventoryEntrySo))
        {
            Debug.LogWarning("Can't give item to " + player.name);
        }
        
    }
    
    private void OnDestroy()
    {
        interactiveObject.OnInteract -= GiveItem;
    }
}
