using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MainMenu : MonoBehaviour
{
    private AudioManager audioManager;

    private void Awake()
    {
        audioManager = FindObjectOfType<AudioManager>();
    }

    public void PlayGame()
    {
        StartCoroutine(PlayGameWithSFX());
    }

    private IEnumerator PlayGameWithSFX()
    {
        audioManager?.PlayButtonSFX();
        yield return new WaitForSeconds(audioManager?.buttonSFX != null ? audioManager.buttonSFX.length : 0.2f);
        SceneManager.LoadSceneAsync(1);
    }

    public void QuitGame()
    {
        audioManager?.PlayButtonSFX();
        Debug.Log("QUIT!");
        Application.Quit();
    }
}
