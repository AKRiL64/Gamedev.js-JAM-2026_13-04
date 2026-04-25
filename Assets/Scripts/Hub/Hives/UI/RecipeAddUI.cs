using Hub.Hives;
using UnityEngine;

namespace Hub
{
    public class RecipeAddUI: MonoBehaviour
    {
        [SerializeField] private GameObject listOfRecipeButtons;
        [SerializeField] private RecipeAddEntryUI recipeAddEntryUITemplate;
        [SerializeField] private RecipeSo[] availableRecipes;
        [SerializeField] private RecipesUI recipesUI;
        private HubState hubState;

        private void Start()
        {
            hubState = HubState.GetInstance();
            hubState.OnPlantsChanged += UpdateButtons;
            recipeAddEntryUITemplate.gameObject.SetActive(false);
            BuildLayout();
        }
        
        private void BuildLayout()
        {
            foreach (Transform child in listOfRecipeButtons.transform)
            {
                if (child == recipeAddEntryUITemplate.transform) continue;
                Destroy(child.gameObject);
            }

            foreach (RecipeSo so in availableRecipes)
            {
                RecipeAddEntryUI recipeAddEntryUI = Instantiate(recipeAddEntryUITemplate, listOfRecipeButtons.transform);
                recipeAddEntryUI.recipeName.text = so.recipeName;
                recipeAddEntryUI.recipeImage.sprite = so.recipeImage;
                recipeAddEntryUI.ingredientImage.sprite = so.honeyProduced.honeyIcon;
                recipeAddEntryUI.addButton.onClick.AddListener(()=>
                {
                    recipesUI.AddRecipe(so);
                    hubState.UseGrownPlant(so.plantRequired.plantType);
                    UpdateButtons();
                });
                recipeAddEntryUI.so = so;
                recipeAddEntryUI.gameObject.SetActive(true);
            }
            UpdateButtons();
            
        }

        private void UpdateButtons()
        {
            foreach (RecipeAddEntryUI recipeEntry in listOfRecipeButtons.GetComponentsInChildren<RecipeAddEntryUI>())
            {
                if (recipeEntry == recipeAddEntryUITemplate) continue;

                bool hasPlants = hubState.GetCountAvailablePlantsByType(recipeEntry.so.plantRequired.plantType) > 0;
                recipeEntry.addButton.interactable = hasPlants;
                if (hasPlants)
                {
                    recipeEntry.activeBackgroundImage.gameObject.SetActive(true);
                    recipeEntry.inactiveBackgroundImage.gameObject.SetActive(false);
                }
                else
                {
                    recipeEntry.activeBackgroundImage.gameObject.SetActive(false);
                    recipeEntry.inactiveBackgroundImage.gameObject.SetActive(true);
                }
            }
        }
    }
}