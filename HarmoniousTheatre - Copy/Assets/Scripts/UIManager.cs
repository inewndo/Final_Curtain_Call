using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    //public GameObject tutPannel;
    public void Play()
    {
        SceneManager.LoadScene("Present");
    }
    public void Back()
    {
        SceneManager.LoadScene(0);
    }

   

    public void Exit()
    {
        //for the build
        Application.Quit();

        //for play mode
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#endif
    }
}
