using System.Linq;
using Hub;
using Hub.Hives;
using UnityEngine;

namespace SaveSystem
{
    [CreateAssetMenu()]
    public class GameDatabase: ScriptableObject
    {
        public PlantSo[] allPlants;
        public BeeSo[] allBees;
        public RecipeSo[] allRecipes;
        public Honey[] allHoney;
        public PlantSo nullPlant;

        public PlantSo GetPlantByName(string name)
        {
            if (string.IsNullOrEmpty(name) || name == nullPlant.name) return nullPlant;
            return allPlants.FirstOrDefault(plant => plant.name == name);
        }

        public RecipeSo GetRecipeByName(string name)
        {
            return allRecipes.FirstOrDefault(recipe => recipe.name == name);
        }

        public Honey GetHoneyByName(string name)
        {
            return allHoney.FirstOrDefault(honey => honey.name == name);
        }
    }
}