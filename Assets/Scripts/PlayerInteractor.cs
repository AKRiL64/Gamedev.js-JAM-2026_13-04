using System.Collections.Generic;
using UnityEngine;

public class PlayerInteractor : MonoBehaviour
{
    [SerializeField] private GameObject player;
    private List<InteractiveObject> interactiveObjects = new List<InteractiveObject>();

    private InteractiveObject currentTarget;

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && currentTarget)
        {
            currentTarget.Interact(player);
            Debug.Log("Interacting with " + currentTarget.name);
        }
        // Debug.Log(interactiveObjects.Count);
        // Debug.Log(currentTarget);
        UpdateHighlight();
    }

    private void OnTriggerEnter(Collider other)
    {
        var interactiveObject = other.gameObject.GetComponentInParent<InteractiveObject>();

        if (!interactiveObject) return;
        
        if (!interactiveObjects.Contains(interactiveObject))
        {
            interactiveObjects.Add(interactiveObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        var interactiveObject = other.GetComponentInParent<InteractiveObject>();

        if (!interactiveObject) return;
        if (interactiveObjects.Contains(interactiveObject))
        {
            interactiveObjects.Remove(interactiveObject);
        }
    }
    
    private void UpdateHighlight()
    {
        if (currentTarget)
        {
            currentTarget.Deselect();
        }
        
        if (interactiveObjects.Count == 0) {
            currentTarget = null;
            return;
        }

        currentTarget = GetClosestInteractable();

        if (currentTarget)
        {
            currentTarget.Select();
        }


    }

    private InteractiveObject GetClosestInteractable()
    {
        InteractiveObject closestInteractable = null;
        var minDistance = float.MaxValue;
        
        ClearListFromRemovedObjects();

        foreach (InteractiveObject interactiveObject in interactiveObjects)
        {
            var distance = Vector3.Distance(transform.position, interactiveObject.transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                closestInteractable = interactiveObject;
            }
        }
        
        return closestInteractable;
        
    }

    private void ClearListFromRemovedObjects()
    {
        interactiveObjects.RemoveAll(interactiveObject => !interactiveObject);
    }


}
