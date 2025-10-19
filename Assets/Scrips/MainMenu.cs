using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    private AudioManager audioManager;

    private void Awake()
    {
        // T�m AudioManager trong scene (c� th? tr? v? null n?u ch?a kh?i t?o)
        audioManager = FindObjectOfType<AudioManager>();
    }

    public void PlayGame()
    {
        StartCoroutine(PlayGameWithSFX());
    }

    private IEnumerator PlayGameWithSFX()
    {
        // Ph�t �m thanh n�t (n?u c�) v� ch? �m thanh k?t th�c tr??c khi chuy?n scene
        audioManager?.PlayButtonSFX();
        // L?u �: audioManager ho?c audioManager.buttonSFX c� th? l� null -> s? d?ng th?i gian ch? m?c ??nh
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
