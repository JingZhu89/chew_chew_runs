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
            newRecord.color= new Vector4(1.0f, 1.0f, 1.0f, 1.0f);
        }
        else if (GameControl.control.LastScore!= GameControl.control.PlayerHighestScore && GameControl.control.PlayerHighScoreList.Contains(GameControl.control.LastScore))
        {
            newHighScore.color= new Vector4(1.0f, 1.0f, 1.0f, 1.0f);
        }
        else
        {
            newScore.color = new Vector4(1.0f, 1.0f, 1.0f, 1.0f);
        }

        lastScore.text = GameControl.control.LastScore.ToString();
        top1.text = GameControl.control.PlayerHighScoreList[0].ToString();
        top2.text = GameControl.control.PlayerHighScoreList[1].ToString();
        top3.text = GameControl.control.PlayerHighScoreList[2].ToString();
        top4.text = GameControl.control.PlayerHighScoreList[3].ToString();
        top5.text = GameControl.control.PlayerHighScoreList[4].ToString();

    }

    public void BackToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }


}
