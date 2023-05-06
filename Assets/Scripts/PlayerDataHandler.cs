using UnityEngine;

public class PlayerDataHandler : MonoBehaviour
{
    //Static Class for save the current player data;
    //Singleton pattern

    public static PlayerDataHandler Instance;

    public string playerName;

    public int score;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
}
