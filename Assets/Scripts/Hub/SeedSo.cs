using System.Collections.Generic;
using UnityEngine;

namespace Hub
{
    [CreateAssetMenu()]
    public class SeedSo: InventoryEntrySo
    {
        public PlantSo plantSo;
        
        
        public SeedSo()
        {
            itemTypes = new ItemTypes[2] { ItemTypes.Disposable, ItemTypes.Seed };
        }

        
        
        
    }
}