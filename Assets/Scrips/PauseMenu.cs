using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField]
    private GameObject pauseMenu;

    [SerializeField]
    private GameObject pauseButton;

    private AudioManager audioManager;

    private void Awake()
    {
        // Tìm AudioManager trong scene (có thể trả về null nếu AudioManager chưa tồn tại)
        audioManager = FindObjectOfType<AudioManager>();
    }

    public void Pause()
    {
        // Phát âm thanh khi nhấn nút (nếu có), hiện menu tạm dừng và dừng thời gian chơi
        audioManager?.PlayButtonSFX();
        pauseMenu?.SetActive(true);
        // Lưu ý: thay đổi Time.timeScale sẽ tạm dừng mọi logic dựa trên Time.deltaTime
        Time.timeScale = 0f;
    }

    public void Resume()
    {
        // Phát âm thanh khi nhấn nút (nếu có), ẩn menu tạm dừng và tiếp tục thời gian chơi
        audioManager?.PlayButtonSFX();
        pauseMenu?.SetActive(false);
        Time.timeScale = 1f;
    }

    public void Quit()
    {
        // Phát âm thanh khi nhấn nút (nếu có) và tải lại scene Menu
        audioManager?.PlayButtonSFX();
        SceneManager.LoadScene("Menu"); // đảm bảo scene "Menu" đã được thêm vào Build Settings
        Time.timeScale = 1f;
    }

    // Cho phép các script khác hiển thị/ẩn UI tạm dừng (và tùy chọn cả nút pause)
    public void SetPauseUIVisible(bool visible)
    {
        pauseMenu?.SetActive(visible);
        pauseButton?.SetActive(visible);
    }
}
