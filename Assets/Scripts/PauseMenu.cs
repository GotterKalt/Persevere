using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenu;
    public static bool isPaused = false;

    public void mainMenu()
    {
        SceneManager.LoadSceneAsync("Main Menu");
    }
    public void Resume()
    {
        ResumeGame();
    }
    public void Quit()
    {
        Application.Quit();
    }

    void Start()
    {
        pauseMenu.SetActive(false);
    }


    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }

    }



    public void PauseGame()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
    }

    public void ResumeGame()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }
}
