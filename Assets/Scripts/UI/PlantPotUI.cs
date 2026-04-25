using Hub;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class PlantPotUI : MonoBehaviour
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
        
        private void Start()
        {
            plantPot.OnPlantChanged += OnPlantChanged;
            OnPlantChanged(plantPot.GetPlantSo());
        }

        private void DeletePlant()
        {
            plantPot.ChangePlant(plantPot.nullPlantSo);
        }

        private void OnPlantChanged(PlantSo plantSo)
        {
            if (plantSo == null) return; 

            textCurrentPlantName.text = plantSo.name;
            
            if (imageCurrentPlant != null)
            {
                imageCurrentPlant.enabled = plantSo.plantSprite != null;
                imageCurrentPlant.sprite = plantSo.plantSprite;
            }

            if (plantInfoPanel != null)
            {
                plantInfoPanel.ChangeInfo(plantSo); 
            }
        }
    }
}