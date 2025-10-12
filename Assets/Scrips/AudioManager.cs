using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public AudioSource sfxSource;
    public AudioSource musicSource;

    public AudioClip moveSFX;
    public AudioClip buttonSFX;
    public AudioClip goalSFX;
    public AudioClip menuMusic;
    public AudioClip gameMusic;

    private void Awake()
    {
        // Đảm bảo AudioManager không bị huỷ khi chuyển scene
        if (FindObjectsOfType<AudioManager>().Length > 1)
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Menu")
        {
            PlayMenuMusic();
        }
        else
        {
            PlayGameMusic();
        }
    }

    // Play movement SFX
    public void PlayMoveSFX()
    {
        sfxSource?.PlayOneShot(moveSFX);
    }

    // Play button press SFX
    public void PlayButtonSFX()
    {
        sfxSource?.PlayOneShot(buttonSFX);
    }

    // Play goal SFX
    public void PlayGoalSFX()
    {
        sfxSource?.PlayOneShot(goalSFX);
    }

    // Play menu background music
    public void PlayMenuMusic()
    {
        if (musicSource.clip != menuMusic || !musicSource.isPlaying)
        {
            musicSource.clip = menuMusic;
            musicSource.loop = true;
            musicSource.Play();
        }
    }

    // Play game background music
    public void PlayGameMusic()
    {
        if (musicSource.clip != gameMusic || !musicSource.isPlaying)
        {
            musicSource.clip = gameMusic;
            musicSource.loop = true;
            musicSource.Play();
        }
    }

    // Stop music
    public void StopMusic()
    {
        musicSource.Stop();
    }
}