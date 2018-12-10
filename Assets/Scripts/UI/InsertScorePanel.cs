using UnityEngine;
using UnityEngine.UI;

public class InsertScorePanel : MonoBehaviour
{
	[SerializeField] private HighScoreManager highScoreManager;
	[SerializeField] private InputField inputField;

	private int currentScore;

	public void Init(int score)
	{
		currentScore = score;
	}

	public void OnClickSubmitScore()
	{
		highScoreManager.InsertScore(inputField.text.ToString(), currentScore, () => { });
		gameObject.SetActive(false);
	}
}