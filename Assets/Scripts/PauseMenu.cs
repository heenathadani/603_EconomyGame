using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;
    public GameObject pauseMenuUI;
    public static PauseMenu instnace;
    public GameObject HUD;
    public TMPro.TextMeshProUGUI tipText;   

    private void Awake()
    {
        instnace = this;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        tipText.gameObject.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
        HUD.SetActive(true);
    }

    void Pause()
    {
        pauseMenuUI.SetActive(true);
        tipText.gameObject.SetActive(false);
        HUD.SetActive(false);
        Time.timeScale = 0f;
        GameIsPaused = true;
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
    }

    public void LoadMenu()
    {
        // You can adjust the scene index or scene name to match your main menu
        SceneManager.LoadScene("UIScene");
    }

}

