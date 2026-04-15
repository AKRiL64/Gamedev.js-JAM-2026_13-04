using UnityEngine;

namespace Hub
{
    public class InventoryEntry : MonoBehaviour
    {
        [SerializeField] private InventoryEntrySo inventoryEntrySo;
        
        public string GetName()
        {
            return inventoryEntrySo.name;
        }

        public Sprite GetAppearance()
        {
            return inventoryEntrySo.appearance;
        }

        public GameObject GetPrefab()
        {
            return inventoryEntrySo.prefab;
        }
        
        public InventoryEntrySo GetInventoryEntrySo()
        {
            return inventoryEntrySo;
        }
    }
}
