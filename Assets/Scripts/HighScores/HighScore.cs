using System;
using UnityEngine;

public class HighScore : IComparable<HighScore>
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int Score { get; set; }

    public HighScore(int id, string name, int score)
    {
        Id = id;
        Name = name;
        Score = score;
    }

    public override string ToString()
    {
        return string.Format("Id: {0}, Name: {1}, Score: {2}", Id, Name, Score);
    }

    public int CompareTo(HighScore other)
    {
        // first > second -> -1
        // first < second -> 1
        // first == second -> 0
        
        if (Score > other.Score)
        {
            return -1;
        }
        else if (other.Score > Score)
        {
            return 1;
        }
        else if (Id < other.Id)
        {
            return -1;
        }
        else
        {
            return 0;
        }
    }
}