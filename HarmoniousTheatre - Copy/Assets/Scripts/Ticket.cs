using System.Collections.Generic;
using UnityEngine;

public class Ticket : Interactable
{
    public List<HingeObject> doors = new List<HingeObject>();
    public GameObject todestroy;

    public override void Interact(CCPlayer ccplayer)
    {
        ccplayer.hasTicket = true;

        

        foreach (var door in doors)
        {
            if (door != null)
            {
                door.isUnlocked = true;
            }
        }
        Destroy(todestroy);
        Debug.Log(todestroy);
        Destroy(gameObject);
        Debug.Log("Destroy");
    }
}
