using System.Linq;
using Hub;
using UnityEngine;

public class TrashBin : MonoBehaviour
{
    [SerializeField] private InteractiveObject interactiveObject;

    private void Start()
    {
        interactiveObject.OnInteract += TrashItem;
    }

    private void TrashItem(GameObject player)
    {
        var playerItemSlot = player.GetComponent<PlayerItemSlotController>();
        if (playerItemSlot != null && playerItemSlot.HasItem)
        {
            InventoryEntrySo item = playerItemSlot.CurrentItem;
            if (item.itemTypes.Contains(InventoryEntrySo.ItemTypes.Disposable))
            {
                Debug.Log("Trashing:  " + item.name);
                playerItemSlot.ClearItem();
            }
            else
            {
                Debug.Log("Trashing: Non-disposable item");
            }
        }
        else
        {
            Debug.Log("Trashing:  No item found");
        }
    }

    private void OnDestroy()
    {
        interactiveObject.OnInteract -= TrashItem;
    }
}
