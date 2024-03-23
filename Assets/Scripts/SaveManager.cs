using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    public void SaveScoreForLevel(string levelName, int score)
    {
        // Save the score for the level
        PlayerPrefs.SetInt(levelName, score);
    }

    public int GetScoreForLevel(string levelName)
    {

        // Get the score for the level
        return PlayerPrefs.GetInt(levelName, 0);
    }

    public void ResetScoreForLevel(string levelName)
    {
        PlayerPrefs.DeleteKey(levelName);
    }

    public void ResetAllScores()
    {
        PlayerPrefs.DeleteAll();
    }

    private void OnApplicationQuit()
    {
        PlayerPrefs.Save();
    }
}
