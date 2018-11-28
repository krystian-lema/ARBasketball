using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

public class HudController : MonoBehaviour
{
	public event Action OnResetButtonClick;
	
	[SerializeField] private Text timeText;
	[SerializeField] private Text scoreText;
	[SerializeField] private Button resetButton;

	private void Awake()
	{
		Assert.IsNotNull(timeText);
		Assert.IsNotNull(scoreText);
		Assert.IsNotNull(resetButton);
	}

	private void OnEnable()
	{
		resetButton.onClick.AddListener(ResetButton_OnClick);
	}

	private void OnDisable()
	{
		resetButton.onClick.RemoveListener(ResetButton_OnClick);
	}

	private void ResetButton_OnClick()
	{
		if (OnResetButtonClick != null)
		{
			OnResetButtonClick();
		}
	}

	public void SetTime(float timeInSec)
	{
		timeText.text = string.Format("{0:00.00}", Mathf.Max(0, timeInSec));
	}

	public void SetScore(int score)
	{
		scoreText.text = score.ToString();
	}
}
