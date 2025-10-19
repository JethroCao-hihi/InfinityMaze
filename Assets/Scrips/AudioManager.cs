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
        // Đảm bảo chỉ có một AudioManager tồn tại khi chuyển scene (nếu có hơn 1 thì huỷ bản này)
        if (FindObjectsOfType<AudioManager>().Length > 1)
        {
            Destroy(gameObject);
            return;
        }
        // Không huỷ AudioManager khi tải scene mới
        DontDestroyOnLoad(gameObject);
        // Đăng ký sự kiện khi scene được tải để đổi nhạc nền phù hợp
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Nếu scene là Menu thì phát nhạc menu, ngược lại phát nhạc game
        if (scene.name == "Menu")
        {
            PlayMenuMusic();
        }
        else
        {
            PlayGameMusic();
        }
    }

    // Phát hiệu ứng âm thanh khi di chuyển
    public void PlayMoveSFX()
    {
        // sfxSource có thể null nếu chưa gán trong inspector
        sfxSource?.PlayOneShot(moveSFX);
    }

    // Phát hiệu ứng âm thanh khi bấm nút
    public void PlayButtonSFX()
    {
        sfxSource?.PlayOneShot(buttonSFX);
    }

    // Phát hiệu ứng âm thanh khi đạt đích
    public void PlayGoalSFX()
    {
        sfxSource?.PlayOneShot(goalSFX);
    }

    // Phát nhạc nền cho menu chính
    public void PlayMenuMusic()
    {
        // Nếu đoạn clip hiện tại không phải menuMusic hoặc đang dừng thì đổi và phát lại
        if (musicSource.clip != menuMusic || !musicSource.isPlaying)
        {
            musicSource.clip = menuMusic;
            musicSource.loop = true;
            musicSource.Play();
        }
    }

    // Phát nhạc nền cho gameplay
    public void PlayGameMusic()
    {
        if (musicSource.clip != gameMusic || !musicSource.isPlaying)
        {
            musicSource.clip = gameMusic;
            musicSource.loop = true;
            musicSource.Play();
        }
    }

    // Dừng nhạc nền
    public void StopMusic()
    {
        // musicSource có thể null nếu chưa gán trong inspector
        musicSource.Stop();
    }
}