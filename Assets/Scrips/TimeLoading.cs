using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class TimeLoading : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private float remainingTime;
    [SerializeField] private float initialTime;
    [SerializeField] private GameObject loseMenu; // g�n trong inspector: panel UI ch?a n�t th? l?i/tho�t

    private AudioManager audioManager;
    private PauseMenu[] pauseMenus;
    private Game gameController;
    private bool isGameOver = false;

    private void Awake()
    {
        audioManager = FindObjectOfType<AudioManager>();
        pauseMenus = FindObjectsOfType<PauseMenu>();
        gameController = FindObjectOfType<Game>();

        // kh?i t?o remainingTime t? initialTime n?u ch?a ???c g�n
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

        if (timerText != null)
        {
            int minutes = Mathf.FloorToInt(remainingTime / 60f);
            int seconds = Mathf.FloorToInt(remainingTime % 60f);
            timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }
    }

    void GameOver()
    {
        isGameOver = true;

        if (timerText != null)
            timerText.color = Color.red;

        if (loseMenu != null)
            loseMenu.SetActive(true);

        // ?n/v� hi?u h�a UI pause v� c�c component li�n quan
        SetPauseUIActive(false);

        // v� hi?u h�a di chuy?n c?a ng??i ch?i
        gameController?.SetPlayable(false);

        Time.timeScale = 0f;
        audioManager?.PlayButtonSFX();
    }

    // ???c g?i b?i c�c script kh�c (v� d? Game) ?? ??t l?i b? ??m th?i gian khi ??t t?i ?�ch
    public void ResetTimer()
    {
        remainingTime = initialTime;
        isGameOver = false;

        // ??m b?o gameplay ti?p t?c
        Time.timeScale = 1f;

        // b?t l?i kh? n?ng di chuy?n n?u tr??c ?� b? t?t
        gameController?.SetPlayable(true);

        // b?t l?i UI pause v� c�c component li�n quan
        SetPauseUIActive(true);
    }

    // ???c g?i b?i n�t UI ?? th? l?i level hi?n t?i
    public void Retry()
    {
        audioManager?.PlayButtonSFX();
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // ???c g?i b?i n�t UI ?? tr? v? menu ch�nh
    public void QuitToMenu()
    {
        audioManager?.PlayButtonSFX();
        Time.timeScale = 1f;
        SceneManager.LoadScene("Menu");
    }

    // H�m tr? gi�p: b?t/t?t UI pause m?t c�ch ?�ng tin c?y. ?u ti�n d�ng API PauseMenu n?u c�, n?u kh�ng th� fallback sang t�m theo t�n/tag.
    private void SetPauseUIActive(bool active)
    {
        if (pauseMenus != null && pauseMenus.Length > 0)
        {
            foreach (var p in pauseMenus)
            {
                if (p == null) continue;
                p.enabled = active;
                p.SetPauseUIVisible(active);
            }
            return;
        }

        // ph??ng �n d? ph�ng: th? t�m theo t�n ??i t??ng ph? bi?n
        var pauseButtonObj = GameObject.Find("PauseButton");
        if (pauseButtonObj != null)
            pauseButtonObj.SetActive(active);

        var pauseMenuObj = GameObject.Find("PauseMenu");
        if (pauseMenuObj != null)
            pauseMenuObj.SetActive(active);

        var taggedPauseButtons = GameObject.FindGameObjectsWithTag("PauseButton");
        foreach (var obj in taggedPauseButtons)
            obj.SetActive(active);
    }
}
