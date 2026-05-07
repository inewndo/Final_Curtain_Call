using TMPro;
using UnityEngine;

public class PopUP : MonoBehaviour
{
    public GameObject popupUI;
    public TMP_Text popupText;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {

            popupText.text = "Use T to travel between time";
            popupUI.SetActive(true);
            

            CancelInvoke();
            Invoke(nameof(HidePopup), 4f);
        }
    }

    void HidePopup()
    {
        popupUI.SetActive(false);
    }
}
