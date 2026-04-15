using UnityEngine;

public class SelectableObject : MonoBehaviour
{
    [SerializeField] private Transform[] selectableElements;

    public void Select()
    {
        foreach (var t in selectableElements)
        {
            t.gameObject.SetActive(true);
        }
    }
    
    public void Deselect()
    {
        foreach (var t in selectableElements)
        {
            t.gameObject.SetActive(false);
        }
    }
}
