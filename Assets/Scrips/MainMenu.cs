using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    private AudioManager audioManager;

    private void Awake()
    {
        // Tìm AudioManager trong scene (có th? tr? v? null n?u ch?a kh?i t?o)
        audioManager = FindObjectOfType<AudioManager>();
    }

    public void PlayGame()
    {
        StartCoroutine(PlayGameWithSFX());
    }

    private IEnumerator PlayGameWithSFX()
    {
        // Phát âm thanh nút (n?u có) và ch? âm thanh k?t thúc tr??c khi chuy?n scene
        audioManager?.PlayButtonSFX();
        // L?u ý: audioManager ho?c audioManager.buttonSFX có th? là null -> s? d?ng th?i gian ch? m?c ??nh
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
