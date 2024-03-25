using TMPro;
using UnityEngine;

public class LevelSelectionDescriptor : MonoBehaviour
{
    public LevelDescriptor LevelDescriptor;
    public float duration = 1.0f;
    public Color color = Color.black;

    public TMP_Text scoreText;
    

    public void RefreshScore()
    {
        var score = SaveManager.Instance.GetScoreForLevel(LevelDescriptor.levelName);
        scoreText.text = score.ToString();
        
        var time = SaveManager.Instance.GetTimeForLevel(LevelDescriptor.levelName);
        if (time < 0)
        {
            scoreText.text = score.ToString();
        }
        else
        {
            scoreText.text = score + $"\n({(int)time / 60:00}:{(int)time % 60:00}.{(int)((decimal)time % 1 * 1000):000})";
        }
    }

    public void PerformTransition()
    {
        Transition.LoadLevel(LevelDescriptor.levelName, duration, color);
    }
}