using UnityEngine;

public class HivesController : MonoBehaviour
{
    [SerializeField] private Hive[] hives;
    [SerializeField] private InteractiveObject interactiveObject;
    [SerializeField] private GameObject closingWindow;

    private void Start()
    {
        interactiveObject.OnInteract += Interaction;
    }

    private void Interaction(GameObject player)
    {
        closingWindow.SetActive(true);
    }
}
