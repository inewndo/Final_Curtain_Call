using System.Collections.Generic;
using UnityEngine;

public class UnlockDoor : MonoBehaviour
{
    public GameObject todestroy;
    public GameObject ticket;
    //public HingeObject door;
    public List<HingeObject> doors = new List<HingeObject>();
    public bool doorlocked;
    private void Start()
    {
        //door.unlocked = false;
       
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == ticket)
        {
            foreach (var door in doors)
            {
                if (door != null)
                {
                    door.isUnlocked = true;
                }
            }
            Destroy(todestroy);
            //door.unlocked = true;
            
        }
    }
}
