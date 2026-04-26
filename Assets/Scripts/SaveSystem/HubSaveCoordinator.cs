using Hub;
using Hub.Hives;
using UnityEngine;

namespace SaveSystem
{
    public class HubSaveCoordinator: MonoBehaviour
    {
        [SerializeField] private HubState hubState;
        [SerializeField] private RecipesUI recipesUI;
        [SerializeField] private PlantPot[] allPots;

        private void Start()
        {
            Invoke(nameof(LoadEntireHub), 0.1f);
            hubState.PassTime();
        }
        
        public void SaveEntireHub()
        {
            GameSaveData data = SaveManager.Instance.CurrentData;

            SaveHoney(data);
            SaveBees(data);
            SaveRecipes(data);
            SavePots(); 
        }

        private void SaveHoney(GameSaveData data)
        {
            data.honeyInventory.Clear();
            
            foreach (var kvp in hubState.GetHoneyAmount())
            {
                data.honeyInventory.Add(new HoneySaveData 
                { 
                    honeyName = kvp.Key.honeyName, 
                    amount = kvp.Value 
                });
            }
        }

        private void SaveBees(GameSaveData data)
        {
            data.bees.Clear();
            
            foreach (var bee in hubState.GetBeeList())
            {
                data.bees.Add(new BeeSaveData
                {
                    beeName = bee.beeName,
                    assignedPlantName = (bee.assignedPlantSo != null && bee.assignedPlantSo.plantType != PlantSo.PlantType.NullPlant) 
                                        ? bee.assignedPlantSo.name : ""
                });
            }
        }

        private void SaveRecipes(GameSaveData data)
        {
            data.queuedRecipeNames.Clear();
            
            foreach (var recipe in recipesUI.GetQueuedRecipes())
            {
                data.queuedRecipeNames.Add(recipe.recipeName);
            }
        }

        private void SavePots()
        {
            foreach (var pot in allPots)
            {
                pot.SavePotData();
            }
        }
        
        public void LoadEntireHub()
        {
            hubState.LoadHubData();

            foreach (var pot in allPots)
            {
                pot.LoadPotData();
            }

            hubState.LoadBeeAssignments();

            LoadRecipes();
        }

        private void LoadRecipes()
        {
            var data = SaveManager.Instance.CurrentData;
            var database = SaveManager.Instance.gameDatabase;
    
            recipesUI.GetQueuedRecipes().Clear();

            foreach (string recipeName in data.queuedRecipeNames)
            {
                RecipeSo recipeBase = database.GetRecipeByName(recipeName);
                if (recipeBase != null)
                {
                    recipesUI.AddRecipe(recipeBase); 
                }
            }
        }
    }
}