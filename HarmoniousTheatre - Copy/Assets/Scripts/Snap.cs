using UnityEngine;

public class Snap : MonoBehaviour
{
    public string tag;
    public GameObject snapPiece;
    public bool snap = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void OnTriggerEnter(Collider other)
    {
        Vector3 pos = new Vector3(snapPiece.transform.position.x, snapPiece.transform.position.y + 0.2f, snapPiece.transform.position.z);
        if (other.CompareTag(tag))
        {
            transform.position = pos;
            snap = true;
        }
    }
}
