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

        popupText.text = "It is so dark here. I should look for control room. It can usually be entered through balcony seating.";
        popupUI.SetActive(true);

        if (popupRoutine != null)
            StopCoroutine(popupRoutine);

        popupRoutine = StartCoroutine(HideAfterDelay(3f));
    }

    private IEnumerator HideAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        popupUI.SetActive(false);
        Destroy(gameObject);
    }
}
