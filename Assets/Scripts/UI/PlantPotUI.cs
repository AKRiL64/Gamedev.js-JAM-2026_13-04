using Hub;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class PlantPotUI: MonoBehaviour
    {
        [SerializeField] private Image imageCurrentPlant;
        [SerializeField] private TextMeshProUGUI textCurrentPlantName;
        [SerializeField] private PlantInfoPanel plantInfoPanel;
        [SerializeField] private Button plantDeleteButton;
        [SerializeField] private PlantPot plantPot;

        private void Awake()
        {
            plantDeleteButton.onClick.AddListener(DeletePlant);
        }

        private void DeletePlant()
        {
            plantPot.ChangePlant(null);
        }
        
        private void Start()
        {
            plantPot.OnPlantChanged += OnPlantChanged;
        }

        private void OnPlantChanged(PlantSo plantSo)
        {
            textCurrentPlantName.text = plantSo.name;
            imageCurrentPlant.sprite = plantSo.plantSprite;
            plantInfoPanel.ChangeInfo(plantSo);
        }
    }
}