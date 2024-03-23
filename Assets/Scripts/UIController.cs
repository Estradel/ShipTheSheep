using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
    public static UIController Instance;

    [SerializeField] private TMP_Text _minionText;

    [SerializeField] private RectTransform _pausePanel;
    [SerializeField] private TMP_Text _winLabel;
    [SerializeField] private TMP_Text _loseLabel;
    [SerializeField] private TMP_Text _pauseLabel;
    [SerializeField] private TMP_Text _scoreLabel;

    private bool _endGame;

    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {
    }

    public void IncreaseCounter(int n)
    {
        UpdateCounter(n);
    }

    public void DecreaseNumber(int n)
    {
        UpdateCounter(n);
    }

    public void UpdateCounter(int n)
    {
        _minionText.text = n.ToString("0000");
    }

    public void Pause()
    {
        _pauseLabel.gameObject.SetActive(true);
    }

    public void ShowPausePanel(int score)
    {
        if (_endGame)
        {
        }
        else
        {
            _pauseLabel.gameObject.SetActive(true);
        }

        // var playerController = PlayerController.instance;
        // playerController.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        // playerController.enabled = false;
        // playerController.PlayerActions.UI.Enable();
        // Time.timeScale = 0;
        _pausePanel.gameObject.SetActive(true);
        _scoreLabel.text = score.ToString("0000");
    }

    public void ClosePausePanel()
    {
        if (_endGame) return;

        // var playerController = PlayerController.instance;
        // playerController.PlayerActions.UI.Disable();
        // playerController.enabled = true;

        Time.timeScale = 1;
        _pausePanel.gameObject.SetActive(false);
        _pauseLabel.gameObject.SetActive(false);
        _loseLabel.gameObject.SetActive(false);
        _winLabel.gameObject.SetActive(false);
        _scoreLabel.gameObject.SetActive(false);
    }

    public void Retry()
    {
        Time.timeScale = 1;
        var scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }

    public void SelectLevel()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("LevelSelectionMenu");
    }

    public void WinGame(int followers)
    {
        _endGame = true;
        _winLabel.gameObject.SetActive(true);
        _scoreLabel.gameObject.SetActive(true);
        _scoreLabel.text = "You gained " + followers + "!";
        // ShowPausePanel(MinionManager.Instance.getNbMinions());
        // SoundPlayer.PlaySound(SoundEnum.WIN);
    }

    public void LoseGame()
    {
        _endGame = true;
        _loseLabel.gameObject.SetActive(true);
        _scoreLabel.gameObject.SetActive(true);
        ShowPausePanel(0);
        // SoundPlayer.PlaySound(SoundEnum.LOSE);
    }
}