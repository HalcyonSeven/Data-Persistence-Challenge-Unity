using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;
using TMPro;

public class MainManager : MonoBehaviour
{
    public Brick BrickPrefab;
    public int LineCount = 6;
    public Rigidbody Ball;

    public Text ScoreText;

    public TextMeshProUGUI currentPlayerName;
    public TextMeshProUGUI topPlayerNameAndScore;

    public GameObject GameOverText;

    private bool m_Started = false;
    private int m_Points;

    private bool m_GameOver = false;

    //Static variables for holding the top player data
    private static int topScore;
    private static string topPlayer;

    private void Awake()
    {
        LoadGameRank();
    }
    // Start is called before the first frame update
    void Start()
    {
        const float step = 0.6f;
        int perLine = Mathf.FloorToInt(4.0f / step);

        int[] pointCountArray = new[] { 1, 1, 2, 2, 5, 5 };
        for (int i = 0; i < LineCount; ++i)
        {
            for (int x = 0; x < perLine; ++x)
            {
                Vector3 position = new(-1.5f + (step * x), 2.5f + (i * 0.3f), 0);
                var brick = Instantiate(BrickPrefab, position, Quaternion.identity);
                brick.PointValue = pointCountArray[i];
                brick.onDestroyed.AddListener(AddPoint);
            }
        }

        currentPlayerName.text = PlayerDataHandler.Instance.playerName;

        SetTopPlayer();
    }

    private void Update()
    {
        if (!m_Started)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                m_Started = true;
                float randomDirection = Random.Range(-1.0f, 1.0f);
                Vector3 forceDir = new(randomDirection, 1, 0);
                forceDir.Normalize();

                Ball.transform.SetParent(null);
                Ball.AddForce(forceDir * 2.0f, ForceMode.VelocityChange);
            }
        }

        // Press Space to play again
        else if (m_GameOver)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
            // Press Esc to return to Menu scene
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                SceneManager.LoadScene(0);
            }
        }
    }

    void AddPoint(int point)
    {
        m_Points += point;
        PlayerDataHandler.Instance.score = m_Points;
        ScoreText.text = $"Score : {m_Points}";
    }

    public void GameOver()
    {
        m_GameOver = true;
        CheckTopPlayer();
        GameOverText.SetActive(true);
    }

    private void CheckTopPlayer()
    {
        int CurrentScore = PlayerDataHandler.Instance.score;

        if (CurrentScore > topScore)
        {
            topPlayer = PlayerDataHandler.Instance.playerName;
            topScore = CurrentScore;

            topPlayerNameAndScore.text = $"Best Score - {topPlayer}: {topScore}";

            SaveGameRank(topPlayer, topScore);
        }
    }

    private void SetTopPlayer()
    {
        if (topPlayer == null && topScore == 0)
        {
            topPlayerNameAndScore.text = "";
        }
        else
        {
            topPlayerNameAndScore.text = $"Top Score - {topPlayer}: {topScore}";
        }
    }

    public void SaveGameRank(string bestPlaterName, int bestPlayerScore)
    {
        SaveData data = new()
        {
            theTopPlayer = bestPlaterName,
            highestScore = bestPlayerScore
        };

        string json = JsonUtility.ToJson(data);
        File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);
    }

    public void LoadGameRank()
    {
        string path = Application.persistentDataPath + "/savefile.json";

        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SaveData data = JsonUtility.FromJson<SaveData>(json);

            topPlayer = data.theTopPlayer;
            topScore = data.highestScore;
        }
    }

    [System.Serializable]
    class SaveData
    {
        public int highestScore;
        public string theTopPlayer;
    }
}
