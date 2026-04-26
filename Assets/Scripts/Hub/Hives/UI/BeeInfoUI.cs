using System;
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
        [SerializeField] private GameObject closingWindow;
        [SerializeField] private BeeReassignUI beeReassignUI;
        [SerializeField] private TextMeshProUGUI beeIsBusyText;
        private BeeSo currentBeeSo;
        public void ShowBeeInfo(BeeSo beeSo)
        {
            beeName.text = beeSo.beeName;
            beeIcon.sprite = beeSo.beeIcon;
            assignedPlantIcon.sprite = beeSo.assignedPlantSo.plantSprite;
            currentBeeSo = beeSo;
            bool isUsed = HubState.GetInstance().IsPlantUsedInRecipe(currentBeeSo.assignedPlantSo);
            beeIsBusyText.gameObject.SetActive(isUsed);
            reassignButton.enabled = !isUsed;
        }

        private void Start()
        {
            reassignButton.onClick.AddListener((() =>
            {
                closingWindow.SetActive(true);
                beeReassignUI.ChangeBee(currentBeeSo);
            }));
        }
    }
}