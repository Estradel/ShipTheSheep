using System.Collections.Generic;
using UnityEngine;

public enum GAME_STATE
{
    MENU,
    PLAY,
    PAUSE,
    WIN,
    LOSE
}

public class GameManager : MonoBehaviour
{
    public List<LevelDescriptor> levels = new();
    public static GameManager Instance { get; private set; }

    public GAME_STATE GameState { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(gameObject);
        else
            Instance = this;

        GameState = GAME_STATE.MENU;

        DontDestroyOnLoad(gameObject);
    }

    // Start is called before the first frame update
    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {
    }

    public void LoadScene(string sceneName)
    {
        Transition.LoadLevel(sceneName, 1, Color.black);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}