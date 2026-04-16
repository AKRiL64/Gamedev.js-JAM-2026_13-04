using Hub;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class ChosenSeedIcon : MonoBehaviour
    {
        [SerializeField] private SeedChooser seedChooser;
        [SerializeField] private Image image;
        void Start()
        {
            image.sprite = seedChooser.GetChosenSeed().appearance;
            seedChooser.OnChosenSeedChanged += ChangeSeedIcon;
        }

        private void ChangeSeedIcon(InventoryEntrySo newSeed)
        {
            image.sprite = newSeed.appearance;
        }

    }
}
