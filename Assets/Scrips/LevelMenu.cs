using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelMenu : MonoBehaviour
{
    private AudioManager audioManager;

    private void Awake()
    {
        audioManager = FindObjectOfType<AudioManager>();
    }

    public void OpenLevel(int levelId)
    {
        audioManager?.PlayButtonSFX();
        string sceneName = "Level" + levelId;
        SceneManager.LoadScene(levelId);
    }
}
