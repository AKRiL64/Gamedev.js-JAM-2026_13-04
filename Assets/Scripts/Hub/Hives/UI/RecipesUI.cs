using System.Collections.Generic;
using Hub.Hives;
using UI;
using UnityEngine;
using UnityEngine.UI;

namespace Hub
{
    public class RecipesUI: MonoBehaviour
    {
        [SerializeField] private Button addRecipeButton;
        [SerializeField] private GameObject closingWindowRecipeAdd;
        [SerializeField] private GameObject recipeList;
        [SerializeField] private RecipeEntryUI recipesEntryTemplate;
        private List<RecipeSo> addedRecipes = new List<RecipeSo>();
        private HubState hubState;

        private void Start()
        {
            addRecipeButton.onClick.AddListener(OpenAllRecipeUI);
            recipesEntryTemplate.gameObject.SetActive(false);
            hubState = HubState.GetInstance();
        }
        

        private void UpdateRecipeEntries()
        {
            foreach (Transform child in recipeList.transform)
            {
                if (child == recipesEntryTemplate.transform)
                {
                    continue;
                }
                Destroy(child.gameObject);
            }

            foreach (RecipeSo so in addedRecipes)
            {
                RecipeEntryUI entryUI = Instantiate(recipesEntryTemplate, recipeList.transform);
                entryUI.recipeImage.sprite = so.recipeImage;
                entryUI.recipeName.text = so.recipeName;
                entryUI.gameObject.SetActive(true);
                entryUI.removeButton.onClick.AddListener(() =>
                {
                    RemoveRecipe(so);
                    hubState.UnuseGrownPlant(so.plantRequired.plantType);
                });
                
            }
        }

        public void AddRecipe(RecipeSo recipe)
        {
            addedRecipes.Add(recipe);
            UpdateRecipeEntries();
        }

        public void RemoveRecipe(RecipeSo recipe)
        {
            addedRecipes.Remove(recipe);
            UpdateRecipeEntries();
        }

        private void OpenAllRecipeUI()
        {
            closingWindowRecipeAdd.SetActive(true);
        }
        
    }
}