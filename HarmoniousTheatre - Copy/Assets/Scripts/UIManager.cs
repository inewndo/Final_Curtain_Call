using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public GameObject pauseMenuUI;
    public GameObject settingsUI;
    public static bool isPaused = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused) ResumeGame();
            else PauseGame();
        }
        if (isPaused)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
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
        
        Time.timeScale = 0f; // Freeze gameplay
        AudioListener.pause = true; // Pause all audio
        isPaused = true;
    }
    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Settings()
    {
        settingsUI.SetActive(true);
    }

    public void CloseSettings()
    {
        settingsUI.SetActive(false);
    }

    public void ResumeGame()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f; // Resume gameplay
        AudioListener.pause = false;
        isPaused = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
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
