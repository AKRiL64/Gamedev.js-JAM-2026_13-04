using UnityEngine;

public class SeedGeneratorControlPanel : MonoBehaviour
{
    [SerializeField] private InteractiveObject interactiveObject;
    [SerializeField] private ItemMaterializer itemMaterializer;
    [SerializeField] private GameObject seeds;

    private void Start()
    {
        interactiveObject.OnInteract += MaterializeSeeds;
    }

    private void MaterializeSeeds(GameObject actor)
    {
        if (!itemMaterializer.Materialize(seeds))
        {
            Debug.LogWarning("Could not materialize seeds");
        }
        
    }
}
