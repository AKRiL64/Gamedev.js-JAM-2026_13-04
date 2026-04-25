using UnityEngine;

namespace Hub.Hives
{
    [CreateAssetMenu()]
    public class RecipeSo: ScriptableObject
    {
        public Sprite recipeImage;
        public string recipeName;
        public PlantSo plantRequired;
        public Honey honeyProduced;
    }
}