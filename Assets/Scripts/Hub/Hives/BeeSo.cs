using UnityEngine;

namespace Hub.Hives
{
    [CreateAssetMenu()]
    public class BeeSo: ScriptableObject
    {
        public string beeName;
        public Sprite beeIcon;
        public PlantSo assignedPlantSo;
    }
}