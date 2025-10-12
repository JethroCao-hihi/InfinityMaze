using Cinemachine;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;

public class Game : MonoBehaviour
{
    public CinemachineVirtualCamera vcam;
    public Transform Player;
    public Transform Goal;
    public Transform Walls;
    public GameObject WallTemplate;
    public GameObject FloorTemplate;
    public float MovementSmoothing;

    public int Width = 3;
    public int Height = 3;
    public bool[,] HWalls, VWalls;
    public float HoleProbability;
    public int GoalX, GoalY;

    public int PlayerX, PlayerY;

    AudioManager audioManager;

    private void Awake()
    {
        audioManager = FindObjectOfType<AudioManager>();
    }

    void Start()
    {
        StartNext();
    }

    void Update()
    {
        HandleSwipe();

        // Làm mượt khi di chuyển
        Vector3 target = new Vector3(PlayerX + 0.5f, PlayerY + 0.5f);
        Player.transform.position = Vector3.Lerp(Player.transform.position, target, Time.deltaTime * MovementSmoothing);

        // Kiểm tra người chơi đã đến đích chưa
        if (Vector3.Distance(Player.transform.position, new Vector3(GoalX + 0.5f, GoalY + 0.5f)) < 0.12f)
        {
            audioManager?.PlayGoalSFX();
            if (Rand(25) < 15)
                Width++;
            else
                Height++;

            StartNext();
        }
        // Bấm G để reset
        if (Input.GetKeyDown(KeyCode.G))
            StartNext();
    }
    Vector2 startTouchPos;
    Vector2 endTouchPos;
    bool isDragging = false;
    float minSwipeDistance = 50f;
    void HandleSwipe()
    {
#if UNITY_EDITOR || UNITY_STANDALONE
        //Xử lý chuột cho test trong editor
        if (Input.GetMouseButtonDown(0))
        {
            isDragging = true;
            startTouchPos = Input.mousePosition;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            if (isDragging)
            {
                endTouchPos = Input.mousePosition;
                DetectSwipeDirection();
            }
            isDragging = false;
        }
#endif

        // Xử lý cảm ứng trên điện thoại
        if (Input.touchCount > 0)
        {
            Touch t = Input.GetTouch(0);
            if (t.phase == TouchPhase.Began)
            {
                isDragging = true;
                startTouchPos = t.position;
            }
            else if (t.phase == TouchPhase.Ended)
            {
                if (isDragging)
                {
                    endTouchPos = t.position;
                    DetectSwipeDirection();
                }
                isDragging = false;
            }
        }
    }

    void DetectSwipeDirection()
    {
        Vector2 delta = endTouchPos - startTouchPos;
        if (delta.magnitude < minSwipeDistance) return; 

        float absX = Mathf.Abs(delta.x);
        float absY = Mathf.Abs(delta.y);

        bool moved = false;
        if (absX > absY)
        {
            if (delta.x > 0 && !HWalls[PlayerX + 1, PlayerY]) { PlayerX++; moved = true; } // Vuốt phải
            else if (delta.x < 0 && !HWalls[PlayerX, PlayerY]) { PlayerX--; moved = true; } // Vuốt trái
        }
        else
        {
            if (delta.y > 0 && !VWalls[PlayerX, PlayerY + 1]) { PlayerY++; moved = true; } // Vuốt lên
            else if (delta.y < 0 && !VWalls[PlayerX, PlayerY]) { PlayerY--; moved = true; } // Vuốt xuống
        }
        if (moved) audioManager?.PlayMoveSFX();
    }
    //Hàm sinh số ngẫu nhiên
    public int Rand(int max)
    {
        return UnityEngine.Random.Range(0, max);
    }
    public float frand()
    {
        return UnityEngine.Random.value;
    }

    public void StartNext()
    {
        //Tạo mê cung mới và xóa mê cung cũ đi
        foreach (Transform child in Walls)
            Destroy(child.gameObject);
        //Sinh mê cung mới
        (HWalls, VWalls) = GenerateLevel(Width, Height);
        //Chọn vị trí ngẫu nhiên cho người chơi và đích
        PlayerX = Rand(Width);
        PlayerY = Rand(Height);

        int minDiff = Mathf.Max(Width, Height) / 2;
        while (true)
        {
            GoalX = Rand(Width);
            GoalY = Rand(Height);
            if (Mathf.Abs(GoalX - PlayerX) >= minDiff) break;
            if (Mathf.Abs(GoalY - PlayerY) >= minDiff) break;
        }

        //Sinh tường và sàn
        for (int x = 0; x < Width + 1; x++)
            for (int y = 0; y < Height; y++)
                if (HWalls[x, y])
                    Instantiate(WallTemplate, new Vector3(x, y + 0.5f, 0), Quaternion.Euler(0, 0, 90), Walls);
        //Sinh tường ngang
        for (int x = 0; x < Width; x++)
            for (int y = 0; y < Height + 1; y++)
                if (VWalls[x, y])
                    Instantiate(WallTemplate, new Vector3(x + 0.5f, y, 0), Quaternion.identity, Walls);
        //Sinh tường dọc
        for (int x = 0; x < Width; x++)
            for (int y = 0; y < Height; y++)
                Instantiate(FloorTemplate, new Vector3(x + 0.5f, y + 0.5f), Quaternion.identity, Walls);
        //Đặt người chơi và đích 
        Player.transform.position = new Vector3(PlayerX + 0.5f, PlayerY + 0.5f);
        Goal.transform.position = new Vector3(GoalX + 0.5f, GoalY + 0.5f);
        //Điều chỉnh camera để phù hợp với kích thước mê cung
        vcam.m_Lens.OrthographicSize = Mathf.Pow(Mathf.Max(Width / 1.5f, Height), 0.70f) * 0.95f;
    }

    public (bool[,], bool[,]) GenerateLevel(int w, int h)
    {
        //Hàm sinh mê cung sử dụng thuật toán DFS
        bool[,] hwalls = new bool[w + 1, h];
        bool[,] vwalls = new bool[w, h + 1];

        bool[,] visited = new bool[w, h];
        //Đệ qy sinh đường đi
        bool dfs(int x, int y)
        {
            if (visited[x, y])
                return false;
            visited[x, y] = true;

            var dirs = new[]
            {
                (x - 1, y, hwalls, x, y),
                (x + 1, y, hwalls, x + 1, y),
                (x, y - 1, vwalls, x, y),
                (x, y + 1, vwalls, x, y + 1),
            };

            foreach (var (nx, ny, wall, wx, wy) in dirs.OrderBy(t => frand()))
                wall[wx, wy] = !(0 <= nx && nx < w && 0 <= ny && ny < h && (dfs(nx, ny) || frand() < HoleProbability));

            return true;
        }
        dfs(0, 0);

        return (hwalls, vwalls);
    }
}
