using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public static SceneController instance;
    public GameObject mainMenu;
    public GameObject selectionMenu;

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
    }

    public void LevelSelection()
    {
        mainMenu.SetActive(false);
        selectionMenu.SetActive(true);
    }

    public void LoadLevel(string level)
    {
        SceneManager.LoadScene(level);
    }

    public void BackToMain()
    {
        selectionMenu.SetActive(false);
        mainMenu.SetActive(true);
    }

    public void QuitGame()
    {
        Application.Quit();
    }


}
