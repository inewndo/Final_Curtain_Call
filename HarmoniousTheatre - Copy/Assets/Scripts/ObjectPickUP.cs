using Unity.VisualScripting;
using UnityEngine;

public class ObjectPickUP : MonoBehaviour
{
    public ObjectData Objects;
    void PickUp()
    {
        InventoryManager.Instance.Add(Objects);
        Destroy(gameObject);
    }

    public void OnMouseDown()
    {
        PickUp();
    }
}
