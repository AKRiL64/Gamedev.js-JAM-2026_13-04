using System.Collections.Generic;
using Hub.Hives;
using UnityEngine;

namespace Hub
{
    public class BeeListUI: MonoBehaviour
    {
        [SerializeField] private GameObject beeButtonList;
        [SerializeField] private BeeListEntryUI beeButtonPrefab;
        [SerializeField] private GameObject closingWindow;
        [SerializeField] private BeeInfoUI beeInfoUI;
        private List<BeeSo>  beeList;
        private HubState hubState;

        private void Start()
        {
            hubState = HubState.GetInstance();
            beeList = hubState.GetBeeList();
            beeButtonPrefab.gameObject.SetActive(false);
            BuildBeeButtons();
        }
        
        private void BuildBeeButtons()
        {
            foreach (Transform child in beeButtonList.transform)
            {
                if (child == beeButtonPrefab.transform) continue;
                Destroy(child.gameObject);
            }
            
            foreach (BeeSo beeSo in beeList)
            {
                BeeListEntryUI entryUI = Instantiate(beeButtonPrefab, beeButtonList.transform);
                entryUI.beeSo = beeSo;
                entryUI.beeName = beeSo.beeName;
                entryUI.beeIcon.sprite = beeSo.beeIcon;
                entryUI.gameObject.SetActive(true);
                entryUI.beeInfoButton.onClick.AddListener(() =>
                {
                    closingWindow.gameObject.SetActive(true);
                    beeInfoUI.ShowBeeInfo(beeSo);
                });
            }
        }
    }
}