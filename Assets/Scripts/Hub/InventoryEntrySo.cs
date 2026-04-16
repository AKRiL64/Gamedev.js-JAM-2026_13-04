using UnityEngine;

namespace Hub
{
    [CreateAssetMenu()]
    public class InventoryEntrySo : ScriptableObject
    {
        public enum ItemTypes
        {
            Disposable,
            Seed
        }
        public new string name;
        public Sprite appearance;
        public GameObject prefab;
        public ItemTypes[] itemTypes;
    }
}
