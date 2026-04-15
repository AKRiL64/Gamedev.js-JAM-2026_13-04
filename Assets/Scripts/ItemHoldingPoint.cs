using Hub;
using UnityEngine;

public class ItemHoldingPoint: MonoBehaviour
{
    private GameObject currentInstantiatedItem;
    
    public void AddItem(InventoryEntrySo item)
    {
        GameObject prefabToSpawn = item.prefab;
        currentInstantiatedItem = Instantiate(prefabToSpawn, this.transform);
        
        currentInstantiatedItem.transform.rotation = Quaternion.identity;
    }

    public void RemoveItem()
    {
        if (!currentInstantiatedItem) return;
        
        Destroy(currentInstantiatedItem);
        currentInstantiatedItem = null;
    }
}
