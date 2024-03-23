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
    }

    public void PerformTransition()
    {
        Transition.LoadLevel(LevelDescriptor.levelName, duration, color);
    }
}