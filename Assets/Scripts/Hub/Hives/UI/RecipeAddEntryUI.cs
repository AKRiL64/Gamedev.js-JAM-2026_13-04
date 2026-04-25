using Hub.Hives;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Hub
{
    public class RecipeAddEntryUI: MonoBehaviour
    {
        [SerializeField] public TextMeshProUGUI recipeName;
        [SerializeField] public Image recipeImage;
        [SerializeField] public Image ingredientImage;
        [SerializeField] public Button addButton;
        [SerializeField] public Image activeBackgroundImage;
        [SerializeField] public Image inactiveBackgroundImage;
        public RecipeSo so;
    }
}