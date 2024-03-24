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
        if (time == -1)
        {
            scoreText.text = score.ToString();
        }
        else
        {
            scoreText.text = score+  $" ({time / 60:00}:{time % 60:00})";;
        }
    }

    public void PerformTransition()
    {
        Transition.LoadLevel(LevelDescriptor.levelName, duration, color);
    }
}