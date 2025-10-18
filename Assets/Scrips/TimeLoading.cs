using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class TimeLoading : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI timerText;
    [SerializeField] float remainingTime;
    [SerializeField] float initialTime;
    [SerializeField] GameObject loseMenu; // assign in inspector: UI panel with retry/quit buttons

    AudioManager audioManager;
    bool isGameOver = false;

    private void Awake()
    {
        audioManager = FindObjectOfType<AudioManager>();
        // initialize remainingTime from initialTime if not set
        if (remainingTime <= 0f)
            remainingTime = initialTime;
    }

    void Update()
    {
        if (isGameOver) return;

        if (remainingTime > 0f)
        {
            remainingTime -= Time.deltaTime;
            if (remainingTime <= 0f)
            {
                remainingTime = 0f;
                GameOver();
            }
        }

        int minutes = Mathf.FloorToInt(remainingTime / 60f);
        int seconds = Mathf.FloorToInt(remainingTime % 60f);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    void GameOver()
    {
        isGameOver = true;
        timerText.color = Color.red;

        if (loseMenu != null)
            loseMenu.SetActive(true);

        // hide pause UI so only lose menu is visible
        var pause = FindObjectOfType<PauseMenu>();
        pause?.SetPauseUIVisible(false);

        // additional fallback: hide common named/tagged pause objects and disable pause components
        var pauseButtonObj = GameObject.Find("PauseButton");
        if (pauseButtonObj != null)
            pauseButtonObj.SetActive(false);
        var pauseMenuObj = GameObject.Find("PauseMenu");
        if (pauseMenuObj != null)
            pauseMenuObj.SetActive(false);

        var taggedPauseButtons = GameObject.FindGameObjectsWithTag("PauseButton");
        if (taggedPauseButtons != null)
        {
            foreach (var obj in taggedPauseButtons)
                obj.SetActive(false);
        }

        foreach (var p in FindObjectsOfType<PauseMenu>())
            p.enabled = false;

        // disable player movement
        var game = FindObjectOfType<Game>();
        game?.SetPlayable(false);

        Time.timeScale = 0f;
        audioManager?.PlayButtonSFX();
    }

    // Called by other scripts (e.g. Game) to reset the timer when reaching goal
    public void ResetTimer()
    {
        remainingTime = initialTime;
        isGameOver = false;
        // ensure gameplay resumes
        Time.timeScale = 1f;

        // re-enable player movement if it was disabled
        var game = FindObjectOfType<Game>();
        game?.SetPlayable(true);

        // re-enable pause UI
        var pause = FindObjectOfType<PauseMenu>();
        if (pause != null)
        {
            pause.enabled = true;
            pause.SetPauseUIVisible(true);
        }

        var pauseButtonObj = GameObject.Find("PauseButton");
        if (pauseButtonObj != null)
            pauseButtonObj.SetActive(true);
        var pauseMenuObj = GameObject.Find("PauseMenu");
        if (pauseMenuObj != null)
            pauseMenuObj.SetActive(true);

        var taggedPauseButtons = GameObject.FindGameObjectsWithTag("PauseButton");
        if (taggedPauseButtons != null)
        {
            foreach (var obj in taggedPauseButtons)
                obj.SetActive(true);
        }
    }

    // Called by UI button to retry the current level
    public void Retry()
    {
        audioManager?.PlayButtonSFX();
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // Called by UI button to quit to main menu
    public void QuitToMenu()
    {
        audioManager?.PlayButtonSFX();
        Time.timeScale = 1f;
        SceneManager.LoadScene("Menu");
    }
}
