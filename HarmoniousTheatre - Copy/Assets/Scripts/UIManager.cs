using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public GameObject pauseMenuUI;
    public static bool isPaused = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused) ResumeGame();
            else PauseGame();
        }
    }
    public void Play()
    {
        SceneManager.LoadScene("Present");
    }
    public void Back()
    {
        SceneManager.LoadScene(0);
    }
    public void PauseGame()
    {
        pauseMenuUI.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Time.timeScale = 0f; // Freeze gameplay
        AudioListener.pause = true; // Pause all audio
        isPaused = true;
    }
    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ResumeGame()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f; // Resume gameplay
        AudioListener.pause = false;
        isPaused = false;
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
