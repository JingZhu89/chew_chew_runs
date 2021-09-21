using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GameControl : MonoBehaviour
{
    public static GameControl control;
    public List<int> PlayerScoreList = new List<int>();
    public List<int> PlayerHighScoreList = new List<int>();
    public int PlayerHighestScore { get; private set; }
    public int LastScore { get; private set; }

    void Awake()
    {
        if (control == null)
        {
            DontDestroyOnLoad(gameObject);
            control = this;
            print("using existing control");
        }
        else if (control != this)
        {
            Destroy(gameObject);
            print("destroy duplicate control");
        }

    }

    public void AddScore(int score)
    {
        PlayerScoreList.Add(score);
        if (PlayerScoreList.Count >= 20)
        {
            PlayerScoreList = PlayerScoreList.OrderByDescending(x => x).Take(10).ToList();
        }
    }

    public void GetTop5Scores()
    {
        PlayerHighScoreList=PlayerScoreList.OrderByDescending(x => x).Take(5).ToList();
    }

    public int TopScore()
    {
        return PlayerHighestScore=PlayerScoreList.Max(x => x);
    }

    public void GetLastScore(int score)
    {
        LastScore = score;
    }

}
