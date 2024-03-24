using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(this);
        else
            Instance = this;
    }

    private void OnApplicationQuit()
    {
        PlayerPrefs.Save();
    }

    public void SaveScoreForLevel(string levelName, int score, int time)
    {
        // Save the score for the level
        PlayerPrefs.SetInt(levelName, score);
        PlayerPrefs.SetInt(levelName + "_time", time);
    }

    public int GetScoreForLevel(string levelName)
    {
        // Get the score for the level
        return PlayerPrefs.GetInt(levelName, 0);
    }
    public int GetTimeForLevel(string levelName)
    {
        return PlayerPrefs.GetInt(levelName + "_time", -1);
    }

    public void ResetScoreForLevel(string levelName)
    {
        PlayerPrefs.DeleteKey(levelName);
    }

    public void ResetAllScores()
    {
        PlayerPrefs.DeleteAll();
    }
}