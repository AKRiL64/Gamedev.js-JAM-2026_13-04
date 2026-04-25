using Hub.Hives;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Hub
{
    public class BeeInfoUI: MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI beeName;
        [SerializeField] private Image beeIcon;
        [SerializeField] private Image assignedPlantIcon;
        [SerializeField] private Button reassignButton;
        private BeeSo currentBeeSo;
        public void ShowBeeInfo(BeeSo beeSo)
        {
            beeName.text = beeSo.beeName;
            beeIcon.sprite = beeSo.beeIcon;
            assignedPlantIcon.sprite = beeSo.assignedPlantSo.plantSprite;
            currentBeeSo = beeSo;
        }
    }
}