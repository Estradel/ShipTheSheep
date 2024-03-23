using System.Collections;
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
    public static GameManager Instance { get; private set; }

    public GAME_STATE GameState { get; private set; }

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

        GameState = GAME_STATE.MENU;

        DontDestroyOnLoad(this.gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }
}
