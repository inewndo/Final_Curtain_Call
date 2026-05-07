using UnityEngine;

public class UnlockDoor : MonoBehaviour
{
    public GameObject todestroy;
    public GameObject ticket;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == ticket)
        {
            Destroy(todestroy);
        }
    }
}
