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
        private PlantSo nullPlantSo;

        private void Awake()
        {
            plantDeleteButton.onClick.AddListener(DeletePlant);
            
        }

        private void DeletePlant()
        {
            plantPot.ChangePlant(nullPlantSo);
        }
        
        private void Start()
        {
            plantPot.OnPlantChanged += OnPlantChanged;
            nullPlantSo = plantPot.nullPlantSo;
            OnPlantChanged(plantPot.GetPlantSo());
        }

        private void OnPlantChanged(PlantSo plantSo)
        {
            textCurrentPlantName.text = plantSo.name;
            imageCurrentPlant.sprite = plantSo.plantSprite;
            plantInfoPanel.ChangeInfo(plantSo);
        }
    }
}