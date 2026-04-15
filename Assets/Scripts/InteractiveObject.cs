using System;
using Unity.VisualScripting;
using UnityEngine;

public class InteractiveObject : MonoBehaviour
{
    public static event Action<EmptyEventArgs> OnInteract;
    
    [SerializeField] private SelectableObject selectablePart;

    public void Interact()
    {
        OnInteract?.Invoke(new EmptyEventArgs());
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
