using UnityEngine;
using System.IO;
using TMPro;

public class LoadGameRankScript : MonoBehaviour
{
    //Fields for display the player info

    public TextMeshProUGUI topPlayerName;

    //Static variables for holding the best player data
    private static int topScore;
    private static string topPlayer;

    private void Awake()
    {
        LoadGameRank();
    }

    private void SetTopPlayer()
    {
        if (topPlayer == null && topScore == 0)
        {
            topPlayerName.text = "";
        }
        else
        {
            topPlayerName.text = $"Top Score - {topPlayer}: {topScore}";
        }
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
            SetTopPlayer();
        }
    }

    [System.Serializable]
    class SaveData
    {
        public int highestScore;
        public string theTopPlayer;
    }
}
