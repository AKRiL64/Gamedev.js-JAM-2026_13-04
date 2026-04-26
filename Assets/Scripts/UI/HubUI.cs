using Hub;
using UnityEngine;

namespace UI
{
    public class HubUI: MonoBehaviour
    {
        [SerializeField] private GameObject honeyAmountLayoutGroup;
        [SerializeField] private HoneyAmountEntryUI honeyAmountEntryUITemplate;
        private HubState hubState;

        private void Start()
        {
            hubState = HubState.GetInstance();
            hubState.OnHoneyAmountChanged += UpdateHoneyAmountUI;
            honeyAmountEntryUITemplate.gameObject.SetActive(false);
            
            UpdateHoneyAmountUI();
        }

        private void UpdateHoneyAmountUI()
        {
            foreach (Transform child in honeyAmountLayoutGroup.transform)
            {
                if (child == honeyAmountEntryUITemplate.transform) continue;
                Destroy(child.gameObject);
            }

            var inventoryHoney = hubState.GetHoneyAmount();

            foreach (var item in inventoryHoney)
            {
                HoneyAmountEntryUI honeyAmountEntryUI = Instantiate(honeyAmountEntryUITemplate, honeyAmountLayoutGroup.transform);
                honeyAmountEntryUI.currentHoney = item.Key;
                honeyAmountEntryUI.honeyAmount.text = item.Value.ToString();
                honeyAmountEntryUI.honeyImage.sprite = item.Key.honeyIcon;
                
                honeyAmountEntryUI.gameObject.SetActive(true);
                
                Debug.Log($"{item.Key.honeyName}: {item.Value}");
            }
        }
    }
}