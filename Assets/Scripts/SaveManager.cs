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

    public void SaveScoreIfBetterForLevel(string levelName, int score, int time)
    {

        int previousScore = GetScoreForLevel(levelName);
        int previousTime = GetTimeForLevel(levelName);

        if (time == -1)
        {
            if (previousTime == -1 && score >= previousScore)
            {
                PlayerPrefs.SetInt(levelName, score);
                PlayerPrefs.SetInt(levelName + "_time", time);
            }
        }
        else
        {
            if (time < previousTime || previousTime == -1)
            {
                PlayerPrefs.SetInt(levelName, score);
                PlayerPrefs.SetInt(levelName + "_time", time);
            }
        }
        PlayerPrefs.Save();


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