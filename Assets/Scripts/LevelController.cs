using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DefaultNamespace;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class LevelController : MonoBehaviour
{
    [Header("Level Infos")]
    [SerializeField] private LevelDescriptor levelDescriptor;
    [SerializeField] private float timeRemaining = 0;

    [Header("LevelScreen")]
    [SerializeField] private GameObject levelUI;
    [SerializeField] private TMP_Text timeText;

    [Header("IntroScreen")]
    [SerializeField] private TMP_Text sheepCounterLeftText;
    [SerializeField] private GameObject introGameObject;

    private AudioSource _audioSource;
    private TimeOfDayController _timeOfDayController;
    private bool isLastSeconds = false;

    [Header("EndScreen")] [SerializeField] private GameObject endScreen;
    [SerializeField] private TMP_Text endSheepCounterText;
    [SerializeField] private TMP_Text endSheepCounterTotalText;
    [SerializeField] private GameObject winTitle;
    [SerializeField] private GameObject loseTitle;
    [SerializeField] private GameObject endButtons;

    [Header("Musics")]
    [SerializeField] private AudioClip introMusic;
    [SerializeField] private AudioClip levelMusic;
    [SerializeField] private AudioClip winMusic;
    [SerializeField] private AudioClip loseMusic;


    private GameController gameController;

    // Start is called before the first frame update
    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        _timeOfDayController = GetComponent<TimeOfDayController>();
        gameController = GameObject.FindWithTag("GameController").GetComponent<GameController>();

        introGameObject.SetActive(true);
        levelUI.SetActive(false);
        endScreen.SetActive(false);

        _audioSource.clip = introMusic;
        _audioSource.Play();

        // StartLevel();
    }

    public void StartLevel()
    {
        timeRemaining = levelDescriptor.timeToComplete;
        _audioSource.clip = levelMusic;
        _audioSource.Play();

        introGameObject.SetActive(false);
        levelUI.SetActive(true);
        endScreen.SetActive(false);

        _timeOfDayController.StartDay(levelDescriptor.timeToComplete);
        GameController.STATE = State.Play;

        DOTween.To(() => timeRemaining, x => timeRemaining = x, 0, levelDescriptor.timeToComplete).SetEase(Ease.Linear).OnUpdate(() =>
        {
            // Debug.Log(timeRemaining);
            timeText.text = "TIME: " + timeRemaining.ToString("F0");

            if (timeRemaining <= 10 && isLastSeconds == false)
            {
                isLastSeconds = true;
                timeText.DOColor(Color.red, 1).SetLoops(10).SetEase(Ease.OutSine).From();
                timeText.transform.DOShakePosition(10, 10, 10, 90, false, false).SetEase(Ease.InCirc);
            }

            _timeOfDayController.UpdateTime(timeRemaining);
        }).OnComplete(() =>
        {
            isLastSeconds = false;
            timeText.DOKill();
            timeText.transform.DOKill();

            ShowEndScreen();
        });
        
        // Total sheeps
        sheepCounterLeftText.text = gameController.Sheeps.Count(sheep => !sheep.isConfined) + "";
        Debug.Log(sheepCounterLeftText.text);
    }

    private void ShowEndScreen()
    {
        GameController.STATE = State.Pause;
        bool win = gameController.Sheeps.Count(sheep => !sheep.isConfined) == 0;
        int nbSheep = gameController.Sheeps.Count(sheep => sheep.isConfined);
        int totalSheep = gameController.Sheeps.Count();
        
        endSheepCounterTotalText.text = "/ " + totalSheep;
        
        _audioSource.Stop();
        introGameObject.SetActive(false);
        levelUI.SetActive(false);
        endScreen.SetActive(true);
        endButtons.SetActive(false);
        winTitle.SetActive(false);
        loseTitle.SetActive(false);
        

        // Grab a free Sequence to use
        Sequence mySequence = DOTween.Sequence();
        // Add a movement tween at the beginning
        mySequence.Append(endSheepCounterText.DOCounter(0, nbSheep, 3, false).OnComplete(() =>
        {
            if (win)
            {
                winTitle.SetActive(true);
                _audioSource.clip = winMusic;
                _audioSource.Play();
            }
            else
            {
                loseTitle.SetActive(true);
                _audioSource.clip = loseMusic;
                _audioSource.Play();
            }
            SaveManager.Instance.SaveScoreForLevel(levelDescriptor.levelName, nbSheep);

            endButtons.SetActive(true);
        }));

        // Add a rotation tween as soon as the previous one is finished
        // mySequence.Append(transform.DORotate(new Vector3(0,180,0), 1));
        // // Delay the whole Sequence by 1 second
        // mySequence.PrependInterval(1);
        // // Insert a scale tween for the whole duration of the Sequence
        // mySequence.Insert(0, transform.DOScale(new Vector3(3,3,3), mySequence.Duration()));
        
        

        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadScene(string sceneName)
    {
        GameManager.Instance.LoadScene(sceneName);
    }

    public void LoadLevel(LevelDescriptor nextLevel)
    {
        GameManager.Instance.LoadScene(nextLevel.levelName);
    }

    public void NextLevel(LevelDescriptor nextLevel)
    {
        GameManager.Instance.LoadScene(nextLevel.levelName);
    }

    public void RetryLevel()
    {
        GameManager.Instance.LoadScene(levelDescriptor.levelName);
    }
    
    public void Back()
    {
        GameManager.Instance.LoadScene("StartScreen");
    }

    public void QuitGame()
    {
        GameManager.Instance.QuitGame();
    }

    public void AddConfinedSheep()
    {
        Debug.Log("AddConfined");
        Debug.Log(gameController.Sheeps.Where(sheep => !sheep.isConfined).Count());
        if (gameController.Sheeps.Where(sheep => !sheep.isConfined).Count() == 0)
        {
            ShowEndScreen();
        }
        sheepCounterLeftText.text = gameController.Sheeps.Where(sheep => !sheep.isConfined).Count() + "";
    }
}
