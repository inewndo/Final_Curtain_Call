using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;
    public List<ObjectData> Objects = new List<ObjectData>();
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        Instance = this;
    }
    public void Add(ObjectData objectData)
    {
        Objects.Add(objectData);
    }
    public void Remove(ObjectData objectData)
    {
        Objects.Remove(objectData);
    }
}
