using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    public event Action OnStartButtonClick; 
    
    [SerializeField] private Text highScoreText;
    [SerializeField] private Button startButton;

    private void Awake()
    {
        Assert.IsNotNull(highScoreText);
        Assert.IsNotNull(startButton);
    }

    private void OnEnable()
    {
        startButton.onClick.AddListener(StartButton_OnClick);
    }

    private void OnDisable()
    {
        startButton.onClick.RemoveListener(StartButton_OnClick);
    }

    private void StartButton_OnClick()
    {
        if (OnStartButtonClick != null)
        {
            OnStartButtonClick();
        }
    }
    
    public void SetHighScore(int score)
    {
        highScoreText.text = score > 0 ? score.ToString() : string.Empty;
    }
}