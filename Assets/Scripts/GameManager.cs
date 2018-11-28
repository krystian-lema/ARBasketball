using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    [SerializeField] private ScoreTrigger scoreTrigger;
    
    public int Score { get; private set; }

    private void Start()
    {
        Score = 0;
    }

    private void OnEnable()
    {
        scoreTrigger.OnScored += ScoreTrigger_OnScored;
    }

    private void OnDisable()
    {
        scoreTrigger.OnScored -= ScoreTrigger_OnScored;
    }

    private void ScoreTrigger_OnScored()
    {
        Score++;
    }

    private void OnGUI()
    {
        var style = new GUIStyle();
        style.fixedHeight = 500f;
        style.fontSize = 50;
        GUILayout.Label("Score: " + Score, style);
    }
}
