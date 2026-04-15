using UnityEngine;

namespace Hub
{
    [CreateAssetMenu()]
    public class InventoryEntrySo : ScriptableObject
    {
        public string name;
        public Sprite appearance;
        public GameObject prefab;
    }
}
