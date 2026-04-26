using System.Collections.Generic;
using Hub.Hives;
using UI;
using UnityEngine;

namespace Hub
{
    public class BeeReassignUI: MonoBehaviour
    {
        [SerializeField] private GameObject availableFlowersContainer;
        [SerializeField] private AvailableFlowerEntry availableFlowerEntryTemplate;
        [SerializeField] private ClosingWindow closingWindow;
        [SerializeField] private PlantSo nullPlantSo;
        [SerializeField] private BeeInfoUI beeInfo;
        private List<PlantSo> unassignedFlowers = new List<PlantSo>();
        private BeeSo beeSo;

        private void Start()
        {
            availableFlowerEntryTemplate.plantSo = nullPlantSo;
            availableFlowerEntryTemplate.assignableFlowerIcon.sprite = nullPlantSo.plantSprite;
            availableFlowerEntryTemplate.assignFlowerButton.onClick.AddListener(() =>
            {
                AssignPlantToBee(beeSo, nullPlantSo);
            });
        }

        public void ChangeBee(BeeSo beeSo)
        {
            this.beeSo = beeSo;
            BuildButtons();
        }

        private void BuildButtons()
        {
            foreach (Transform child in availableFlowersContainer.transform)
            {
                if (child == availableFlowerEntryTemplate.transform) continue;
                Destroy(child.gameObject);
            }
            
            unassignedFlowers = HubState.GetInstance().GetUnassignedPlants();
            
            foreach (PlantSo plant in unassignedFlowers)
            {
                AvailableFlowerEntry availableFlowerEntry = Instantiate(availableFlowerEntryTemplate, availableFlowersContainer.transform);
                availableFlowerEntry.plantSo = plant;
                availableFlowerEntry.assignableFlowerIcon.sprite = plant.plantSprite;
                availableFlowerEntry.assignFlowerButton.onClick.AddListener(() =>
                {
                    AssignPlantToBee(beeSo, plant);
                });
            }
        }
        
        private void AssignPlantToBee(BeeSo bee, PlantSo newPlant)
        {
            var hub = HubState.GetInstance();
            if (bee.assignedPlantSo != null && bee.assignedPlantSo.plantType != PlantSo.PlantType.NullPlant)
            {
                hub.UnassignPlant(bee.assignedPlantSo);
            }
            bee.assignedPlantSo = newPlant;
            if (newPlant != null && newPlant.plantType != PlantSo.PlantType.NullPlant)
            {
                hub.AssignPlant(newPlant);
            }

            beeInfo.ShowBeeInfo(bee);
            ExitThisWindow();
        }

        private void ExitThisWindow()
        {
            closingWindow.Close();
        }
    }
}