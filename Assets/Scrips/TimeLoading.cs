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
    [SerializeField] private GameObject loseMenu; // gán trong inspector: panel UI ch?a nút th? l?i/thoát

    private AudioManager audioManager;
    private PauseMenu[] pauseMenus;
    private Game gameController;
    private bool isGameOver = false;

    private void Awake()
    {
        audioManager = FindObjectOfType<AudioManager>();
        pauseMenus = FindObjectsOfType<PauseMenu>();
        gameController = FindObjectOfType<Game>();

        // kh?i t?o remainingTime t? initialTime n?u ch?a ???c gán
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

        // ?n/vô hi?u hóa UI pause và các component liên quan
        SetPauseUIActive(false);

        // vô hi?u hóa di chuy?n c?a ng??i ch?i
        gameController?.SetPlayable(false);

        Time.timeScale = 0f;
        audioManager?.PlayButtonSFX();
    }

    // ???c g?i b?i các script khác (ví d? Game) ?? ??t l?i b? ??m th?i gian khi ??t t?i ?ích
    public void ResetTimer()
    {
        remainingTime = initialTime;
        isGameOver = false;

        // ??m b?o gameplay ti?p t?c
        Time.timeScale = 1f;

        // b?t l?i kh? n?ng di chuy?n n?u tr??c ?ó b? t?t
        gameController?.SetPlayable(true);

        // b?t l?i UI pause và các component liên quan
        SetPauseUIActive(true);
    }

    // ???c g?i b?i nút UI ?? th? l?i level hi?n t?i
    public void Retry()
    {
        audioManager?.PlayButtonSFX();
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // ???c g?i b?i nút UI ?? tr? v? menu chính
    public void QuitToMenu()
    {
        audioManager?.PlayButtonSFX();
        Time.timeScale = 1f;
        SceneManager.LoadScene("Menu");
    }

    // Hàm tr? giúp: b?t/t?t UI pause m?t cách ?áng tin c?y. ?u tiên dùng API PauseMenu n?u có, n?u không thì fallback sang tìm theo tên/tag.
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

        // ph??ng án d? phòng: th? tìm theo tên ??i t??ng ph? bi?n
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
