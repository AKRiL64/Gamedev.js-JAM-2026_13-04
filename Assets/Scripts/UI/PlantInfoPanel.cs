using Hub;
using TMPro;
using UnityEngine;

namespace UI
{
    public class PlantInfoPanel: MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI textPlantStatus;
        
        public void ChangeInfo(PlantSo plantSo)
        {
            if (plantSo.plantType == PlantSo.PlantType.NullPlant)
            {
                textPlantStatus.text = "";
            }
            else
            {
                string statusText = plantSo.IsGrown ? "Completed" : "Growing process";
                textPlantStatus.text = $"Status: {statusText}";
            }
        }
    }
}