using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public static SceneController instance;
    public GameObject mainMenu;
    public GameObject selectionMenu;

    // Start is called before the first frame update
    private void Start()
    {
        instance = this;
    }

    public void LevelSelection()
    {
        mainMenu.SetActive(false);
        selectionMenu.SetActive(true);

        var levelSelectionDescriptors =
            FindObjectsByType<LevelSelectionDescriptor>(FindObjectsSortMode.None);
        foreach (var descriptor in levelSelectionDescriptors) descriptor.RefreshScore();
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