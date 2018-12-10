using System;
using UnityEngine;
using UnityEngine.Assertions;

public class GameManager : MonoBehaviour
{
    [SerializeField] private InsertScorePanel insertScorePanel;
    
    [SerializeField] private MenuController menuController;
    [SerializeField] private HudController hudController;
    
    [SerializeField] private ScoreTrigger scoreTrigger;
    [SerializeField] private BallController ballController;
    

    private bool playing;
    public bool Playing
    {
        get { return playing; }
        private set
        {
            playing = value;
            menuController.gameObject.SetActive(!playing);
            menuController.SetHighScore(HighScore);
            hudController.gameObject.SetActive(playing);
            ballController.CanThrow = playing;
        }
    }

    private int score;
    public int Score
    {
        get { return score; }
        private set
        {
            score = value;
            hudController.SetScore(score);
        }
        
    }

    private int HighScore
    {
        get { return PlayerPrefs.GetInt("HighScore", 0); }
        set
        {
            PlayerPrefs.SetInt("HighScore", value);
            PlayerPrefs.Save();
        }
    }

    private float timer;
    public float Timer
    {
        get { return timer; }
        private set
        {
            timer = value;
            hudController.SetTime(timer);
        }
    }

    private void Awake()
    {
        Assert.IsNotNull(insertScorePanel);
        Assert.IsNotNull(menuController);
        Assert.IsNotNull(hudController);
        Assert.IsNotNull(scoreTrigger);
        Assert.IsNotNull(ballController);
    }

    private void Start()
    {
        Stop();
    }

    private void Update()
    {
        if (Playing)
        {
            if (Timer > 0)
            {
                Timer -= Time.deltaTime;
            }
            else
            {
                Stop();
            }
        }
        

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (!Playing) Play();
            else Stop();
        }
    }

    private void OnEnable()
    {
        scoreTrigger.OnScored += ScoreTrigger_OnScored;
        menuController.OnStartButtonClick += MenuController_OnStartButtonClick;
        hudController.OnResetButtonClick += HudController_OnResetButtonClick;
    }


    private void OnDisable()
    {
        scoreTrigger.OnScored -= ScoreTrigger_OnScored;
        menuController.OnStartButtonClick -= MenuController_OnStartButtonClick;
        hudController.OnResetButtonClick -= HudController_OnResetButtonClick;
    }

    private void ScoreTrigger_OnScored()
    {
        Score++;
    }
    
    private void MenuController_OnStartButtonClick()
    {
        Debug.LogFormat("MenuController_OnStartButtonClick");
        Play();
    }

    private void HudController_OnResetButtonClick()
    {
        ballController.RestartBall();
    }
    
    private void Restart()
    {
        Score = 0;
        Timer = 30f;
        ballController.RestartBall();
    }

    public void Play()
    {
        Playing = true;
        Restart();
    }

    public void Stop()
    {
        ballController.RestartBall();
        HighScore = Math.Max(Score, HighScore);
        Playing = false;
        if (Score > 0)
        {
            insertScorePanel.gameObject.SetActive(true);
            insertScorePanel.Init(Score);
        }
    }
}