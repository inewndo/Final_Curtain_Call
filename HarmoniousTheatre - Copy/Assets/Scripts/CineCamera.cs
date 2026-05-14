using Unity.Cinemachine;
using UnityEngine;

public class CineCamera : MonoBehaviour
{
    public CinemachineCamera mainCam;
    private CinemachineCamera currentCam;
    public string camTag;

    private void Awake()
    {
        mainCam.Priority = 2;
        currentCam = null;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag(camTag))
        {
            currentCam = null;
            currentCam = other.gameObject.GetComponentInParent<CinemachineCamera>();
            currentCam.Priority = 2;
            mainCam.Priority = 1;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag(camTag))
        {
            
            currentCam.Priority = 1;
            currentCam = null;
            mainCam.Priority = 2;
            Destroy(other.gameObject);
        }
    }
}
