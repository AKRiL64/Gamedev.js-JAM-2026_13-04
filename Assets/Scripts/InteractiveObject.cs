using System;
using Unity.VisualScripting;
using UnityEngine;

public class InteractiveObject : MonoBehaviour
{
    public event Action<GameObject> OnInteract;
    
    [SerializeField] private SelectableObject selectablePart;

    public void Interact(GameObject actor)
    {
        OnInteract?.Invoke(actor);
    }

    public void Select()
    {
        selectablePart.Select();
    }

    public void Deselect()
    {
        selectablePart.Deselect();
    }
}
