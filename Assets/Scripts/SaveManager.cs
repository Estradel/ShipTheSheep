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

    public void SaveScoreIfBetterForLevel(string levelName, int score, float time)
    {

        int previousScore = GetScoreForLevel(levelName);
        float previousTime = GetTimeForLevel(levelName);

        if (time < 0)
        {
            if (previousTime < 0 && score >= previousScore)
            {
                PlayerPrefs.SetInt(levelName, score);
                PlayerPrefs.SetFloat(levelName + "_time", time);
            }
        }
        else
        {
            if (time < previousTime || previousTime < 0)
            {
                PlayerPrefs.SetInt(levelName, score);
                PlayerPrefs.SetFloat(levelName + "_time", time);
            }
        }
        PlayerPrefs.Save();


    }

    public int GetScoreForLevel(string levelName)
    {
        // Get the score for the level
        return PlayerPrefs.GetInt(levelName, 0);
    }
    public float GetTimeForLevel(string levelName)
    {
        return PlayerPrefs.GetFloat(levelName + "_time", -1);
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