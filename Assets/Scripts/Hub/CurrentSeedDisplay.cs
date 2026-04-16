using Hub;
using UnityEngine;
using UnityEngine.UI;

public class CurrentSeedDisplay : MonoBehaviour
{
    [SerializeField] private SeedChooser seedChooser;
    [SerializeField] private Image image;
    void Start()
    {
        image.sprite = HubState.GetInstance().GetAvailableSeeds()[0].appearance;
        seedChooser.OnChosenSeedChanged += ChangeSeedIcon;
    }

    private void ChangeSeedIcon(InventoryEntrySo newSeed)
    {
        Debug.Log(seedChooser.GetChosenSeed().name);
        image.sprite = newSeed.appearance;
    }
}
