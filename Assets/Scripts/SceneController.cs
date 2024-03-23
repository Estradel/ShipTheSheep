using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public static GameController instance;

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
    }

    public void PlayGame()
    {
        SceneManager.LoadScene("OutdoorScene");
    }


    public void QuitGame()
    {
        Application.Quit();
    }


}
