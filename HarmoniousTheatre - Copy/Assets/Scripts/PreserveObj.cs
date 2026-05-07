using UnityEditor.Overlays;
using UnityEngine;

public class PreserveObj : MonoBehaviour
{
    private void Start()
    {
       
        transform.SetParent(SaveData.Instance.transform);
    }
}
