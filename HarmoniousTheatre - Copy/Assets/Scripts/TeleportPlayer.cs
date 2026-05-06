using UnityEngine;

public class TeleportPlayer : MonoBehaviour
{
    public Transform endPosition;
    //public CharacterController characterController;

    private void OnTriggerEnter(Collider other)
    {
        //characterController.enabled = false;

        //characterController.transform.position = endPosition.position;

        //characterController.enabled = true; 
        other.transform.position = endPosition.position;
    }

}
