using System;
using Hub;
using UnityEngine;

public class PlayerItemSlotController: MonoBehaviour
{
    public event Action OnItemChanged;

    [SerializeField] private ItemHoldingPoint holdingPoint;
    
    private InventoryEntrySo currentItem;
    
    public bool HasItem => currentItem != null;
    public InventoryEntrySo CurrentItem => currentItem;

    public bool SetItem(InventoryEntrySo newItem)
    {
        if (!newItem) return false;
        if (HasItem) return false;
        
        currentItem = newItem;
        holdingPoint.AddItem(newItem);
        OnItemChanged?.Invoke();
        return true;
    }

    public bool ClearItem()
    {
        if (!HasItem) return false;
        currentItem = null;
        holdingPoint.RemoveItem();
        OnItemChanged?.Invoke();
        return true;
    }
    
}
