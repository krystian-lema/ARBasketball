using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighScoresPanel : MonoBehaviour
{
    [SerializeField] private GameObject emptyListPrefab;
    [SerializeField] private HighScoreUI prefab;
    [SerializeField] private HighScoreManager highScoreManager;
    [SerializeField] private Transform highScoresParent;

    private List<GameObject> spawnedHighScores = new List<GameObject>();

    private void OnEnable()
    {
        Init();
    }

    private void Init()
    {
        foreach (var spawnedHighScore in spawnedHighScores)
        {
            Destroy(spawnedHighScore);
        }
        spawnedHighScores = new List<GameObject>();
        
        // Init new
        highScoreManager.GetScores(HighScoreManager_OnComplete);
    }

    private void HighScoreManager_OnComplete(List<HighScore> highScores)
    {   
        if (highScores.Count > 0)
        {
            for (var i = 0; i < highScores.Count; i++)
            {
                var highScore = highScores[i];
                var newHighScore = Instantiate(prefab);
                newHighScore.transform.SetParent(highScoresParent);
                newHighScore.transform.localScale = new Vector3(1, 1, 1);
                newHighScore.Init(i + 1, highScore.Name, highScore.Score);
                spawnedHighScores.Add(newHighScore.gameObject);
            }
        }
        else
        {
            var newHighScore = Instantiate(emptyListPrefab);
            newHighScore.transform.SetParent(highScoresParent);
            newHighScore.transform.localScale = new Vector3(1, 1, 1);
            spawnedHighScores.Add(newHighScore.gameObject);
        }
    }
}
