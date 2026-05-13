using UnityEngine;

public  class Interactable : MonoBehaviour
{
    public ObjectData objectData;

    public void Dialogue(CCPlayer cCPlayer)
    {
        cCPlayer.RequestDescription(objectData);
    }
}
