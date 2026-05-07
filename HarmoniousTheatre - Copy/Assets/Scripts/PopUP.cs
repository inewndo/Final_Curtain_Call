using System.Collections;
using TMPro;
using UnityEngine;

public class PopUP : MonoBehaviour
{
    public GameObject popupUI;
    public TextMeshProUGUI popupText;
    private Coroutine popupRoutine;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        popupText.text = "Use T to travel between time";
        popupUI.SetActive(true);

        if (popupRoutine != null)
            StopCoroutine(popupRoutine);

        popupRoutine = StartCoroutine(HideAfterDelay(4f));
    }

    private IEnumerator HideAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        popupUI.SetActive(false);
    }
}
