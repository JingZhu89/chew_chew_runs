using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ShowResult : MonoBehaviour
{
    public Text newRecord;
    public Text newHighScore;
    public Text newScore;
    public Text lastScore;
    public Text top1;
    public Text top2;
    public Text top3;
    public Text top4;
    public Text top5;


    private void Start()
    {
        newRecord.color = new Vector4(0.0f, 0.0f, 0.0f, 0.0f);
        newHighScore.color = new Vector4(0.0f, 0.0f, 0.0f, 0.0f);
        newScore.color = new Vector4(0.0f, 0.0f, 0.0f, 0.0f);

    }


    void Update()
    {
        if (GameControl.control.LastScore == GameControl.control.PlayerHighestScore && GameControl.control.LastScore !=0)
        {
            newRecord.color= new Vector4(0.0f, 0.0f, 0.0f, 1.0f);
        }
        else if (GameControl.control.LastScore!= GameControl.control.PlayerHighestScore && GameControl.control.PlayerHighScoreList.Contains(GameControl.control.LastScore))
        {
            newHighScore.color= new Vector4(0.0f, 0.0f, 0.0f, 1.0f);
        }
        else
        {
            newScore.color = new Vector4(0.0f, 0.0f, 0.0f, 1.0f);
        }

        lastScore.text = GameControl.control.LastScore.ToString();
        top1.text = "1st: " + GetScore(0);
        top2.text = "2nd: " + GetScore(1);
        top3.text = "3rd: " + GetScore(2);
        top4.text = "4th: " + GetScore(3);
        top5.text = "5th: " + GetScore(4);

    }

    public void BackToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    private string GetScore(int scoreNum)
    {
        if (GameControl.control.PlayerHighScoreList.Count > scoreNum)
        {
            return GameControl.control.PlayerHighScoreList[scoreNum].ToString();
        }
        else
        {
            return "";
        }
    }
}
