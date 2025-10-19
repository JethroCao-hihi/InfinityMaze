using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelMenu : MonoBehaviour
{
    private AudioManager audioManager;

    private void Awake()
    {
        // T�m AudioManager trong scene (c� th? tr? v? null n?u ch?a kh?i t?o)
        audioManager = FindObjectOfType<AudioManager>();
    }

    public void OpenLevel(int levelId)
    {
        // Ph�t �m thanh n�t (n?u c�) v� t?i scene theo ID
        audioManager?.PlayButtonSFX();
        string sceneName = "Level" + levelId; // ch� �: ?ang x�y d?ng t�n scene theo pattern "LevelX" nh?ng d�ng d??i t?i theo index
        SceneManager.LoadScene(levelId); // d�ng build index; ??m b?o index t??ng ?ng ?� ???c c?u h�nh trong Build Settings
    }
}
