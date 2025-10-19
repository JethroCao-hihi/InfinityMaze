using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelMenu : MonoBehaviour
{
    private AudioManager audioManager;

    private void Awake()
    {
        // Tìm AudioManager trong scene (có th? tr? v? null n?u ch?a kh?i t?o)
        audioManager = FindObjectOfType<AudioManager>();
    }

    public void OpenLevel(int levelId)
    {
        // Phát âm thanh nút (n?u có) và t?i scene theo ID
        audioManager?.PlayButtonSFX();
        string sceneName = "Level" + levelId; // chú ý: ?ang xây d?ng tên scene theo pattern "LevelX" nh?ng dòng d??i t?i theo index
        SceneManager.LoadScene(levelId); // dùng build index; ??m b?o index t??ng ?ng ?ã ???c c?u hình trong Build Settings
    }
}
