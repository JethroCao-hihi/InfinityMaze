using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField]
    GameObject pauseMenu;
    private AudioManager audioManager;

    private void Awake()
    {
        audioManager = FindObjectOfType<AudioManager>();
    }

    public void Pause()
    {
        audioManager?.PlayButtonSFX();
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
    }
    public void Resume()
    {
        audioManager?.PlayButtonSFX();
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
    }
    public void Quit()
    {
        audioManager?.PlayButtonSFX();
        SceneManager.LoadScene("Menu");
        Time.timeScale = 1f;
    }
}
