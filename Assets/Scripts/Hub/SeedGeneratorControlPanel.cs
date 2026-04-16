using Hub;
using UnityEngine;

public class SeedGeneratorControlPanel : MonoBehaviour
{
    [SerializeField] private InteractiveObject interactiveObject;
    [SerializeField] private ItemMaterializer itemMaterializer;
    [SerializeField] private SeedChooser seedChooser;

    private void Start()
    {
        interactiveObject.OnInteract += MaterializeSeeds;
    }

    private void MaterializeSeeds(GameObject actor)
    {
        if (seedChooser == null)
        {
            Debug.LogError("SeedChooser not assigned!");
            return;
        }

        var seed = seedChooser.GetChosenSeed();
        if (seed == null)
        {
            Debug.LogError("No seed selected!");
            return;
        }

        if (seed.prefab == null)
        {
            Debug.LogError("Seed prefab missing!");
            return;
        }
        
        if (!itemMaterializer.Materialize(seedChooser.GetChosenSeed().prefab))
        {
            Debug.LogWarning("Could not materialize seeds");
        }
    }
}
