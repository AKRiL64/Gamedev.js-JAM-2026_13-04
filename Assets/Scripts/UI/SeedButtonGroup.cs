using Hub;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class SeedButtonGroup: MonoBehaviour
    {
        [SerializeField] private GameObject buttonTemplate;
        [SerializeField] private SeedChooser seedChooser;
        
        private HubState hubState;

        private void Awake()
        {
            buttonTemplate.SetActive(false);
        }
        private void Start()
        {
            hubState = HubState.GetInstance();
            UpdateAvailableSeeds();
        }

        private void UpdateAvailableSeeds()
        {
            foreach (Transform child in transform) 
            {
                if (child.gameObject.name == "Template" || !child.gameObject.activeSelf) 
                {
                    continue; 
                }
    
                Destroy(child.gameObject);
            }
            
            var availableSeeds = hubState.GetAvailableSeeds();
            
            foreach (var seed in availableSeeds) {
                var button = Instantiate(buttonTemplate, transform);
                Button buttonComponent = button.GetComponent<Button>();
                Transform iconTransform = button.transform.Find("Icon"); 
                Image image = iconTransform != null ? iconTransform.GetComponent<Image>() : button.GetComponent<Image>();
                image.sprite = seed.appearance;
                Debug.Log("Icon of button changed to: " + seed.name);
                buttonComponent.onClick.AddListener(() =>
                {
                    seedChooser.ChangeSeed(seed);
                });
                button.SetActive(true);
            }
            seedChooser.ChangeSeed(hubState.GetAvailableSeeds()[0]);
        }
    }
}