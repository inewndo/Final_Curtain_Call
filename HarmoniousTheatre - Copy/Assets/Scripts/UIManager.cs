using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    //public GameObject tutPannel;
    public void Play()
    {
        SceneManager.LoadScene("Present");
    }
}
