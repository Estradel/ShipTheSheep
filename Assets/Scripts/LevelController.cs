using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    [SerializeField] private TMP_Text sheepCounterText;
    [SerializeField] private GameObject introGameObject;

    private AudioSource _audioSource;
    private TimeOfDayController _timeOfDayController;
    private bool isLastSeconds = false;

    [Header("EndScreen")] [SerializeField] private GameObject endScreen;
    [SerializeField] private TMP_Text endSheepCounter;
    [SerializeField] private GameObject winTitle;
    [SerializeField] private GameObject loseTitle;
    [SerializeField] private GameObject endButtons;

    [Header("Musics")]
    [SerializeField] private AudioClip introMusic;
    [SerializeField] private AudioClip levelMusic;
    [SerializeField] private AudioClip winMusic;
    [SerializeField] private AudioClip loseMusic;
    // Start is called before the first frame update
    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        _timeOfDayController = GetComponent<TimeOfDayController>();

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

            // TODO Check if Lose or Win

            ShowEndScreen(false, 100);
        });
    }

    private void ShowEndScreen(bool win, int nbSheep)
    {
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
        mySequence.Append(endSheepCounter.DOCounter(0, nbSheep, 5, false).OnComplete(() =>
        {
            if (win)
            {
                winTitle.SetActive(true);
                _audioSource.clip = winMusic;
                _audioSource.Play();

                SaveManager.Instance.SaveScoreForLevel(levelDescriptor.levelName, nbSheep);
            }
            else
            {
                loseTitle.SetActive(true);
                _audioSource.clip = loseMusic;
                _audioSource.Play();
            }

            endButtons.SetActive(true);
        }));

        // Add a rotation tween as soon as the previous one is finished
        // mySequence.Append(transform.DORotate(new Vector3(0,180,0), 1));
        // // Delay the whole Sequence by 1 second
        // mySequence.PrependInterval(1);
        // // Insert a scale tween for the whole duration of the Sequence
        // mySequence.Insert(0, transform.DOScale(new Vector3(3,3,3), mySequence.Duration()));
        
        
        // Total sheeps
        GameController gameController = GameObject.FindWithTag("GameController").GetComponent<GameController>();
        sheepCounterText.text = gameController.Sheeps.Where(sheep => !sheep.isConfined).Count() + "";
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

    public void QuitGame()
    {
        GameManager.Instance.QuitGame();
    }

    public void AddConfinedSheep()
    {
        GameController gameController = GameObject.FindWithTag("GameController").GetComponent<GameController>();
        sheepCounterText.text = gameController.Sheeps.Where(sheep => !sheep.isConfined).Count() + "";
    }
}
