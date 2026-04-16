using System;
using UnityEngine;

namespace Hub
{
    public class SeedChooser : MonoBehaviour
    {
        public event Action<InventoryEntrySo> OnChosenSeedChanged;
        
        [SerializeField] private InteractiveObject interactiveObject;
        [SerializeField] private GameObject canvasToShow;
        private InventoryEntrySo chosenSeed;
        
        private void Start()
        {
            interactiveObject.OnInteract += ShowCanvas;
            ChangeSeed(HubState.GetInstance().GetAvailableSeeds()[0]);
        }
        
        private void ShowCanvas(GameObject player)
        {
            canvasToShow.SetActive(true);
        }

        private void OnDestroy()
        {
            interactiveObject.OnInteract -= ShowCanvas;
        }

        public void ChangeSeed(InventoryEntrySo newSeed)
        {
            chosenSeed = newSeed;
            OnChosenSeedChanged?.Invoke(chosenSeed);
        }

        public InventoryEntrySo GetChosenSeed()
        {
            return chosenSeed;
        }
    }
}
