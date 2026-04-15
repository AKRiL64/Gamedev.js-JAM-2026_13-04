using UnityEngine;

public class ItemMaterializer : MonoBehaviour
{
    [SerializeField] private Transform materializePoint;
    private GameObject currentInstantiatedItem;

    public bool Materialize(GameObject item)
    {
        if (currentInstantiatedItem)
        {
            return false;
        }
        currentInstantiatedItem = Instantiate(item, materializePoint.position, materializePoint.rotation);
        currentInstantiatedItem.transform.SetParent(materializePoint);
        return true;
    }
}
