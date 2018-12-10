using UnityEngine;
using UnityEngine.UI;

public class HighScoreUI : MonoBehaviour
{
    [SerializeField] private Text rank;
    [SerializeField] private Text name;
    [SerializeField] private Text score;

    public void Init(int rank, string name, int score)
    {
        this.rank.text = string.Format("#{0}", rank);
        this.name.text = name;
        this.score.text = string.Format("{0}", score);
    }
}