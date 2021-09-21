using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreAndHighScoreDisplay : MonoBehaviour
    
{
    public PlayerMovement Player;
    public Text scoreText;
    public Text powerUpText;
    public Text topScoreText;


    void Update()
    {
        scoreText.text = "Score : " + Player.playerScore;
        topScoreText.text = "Top Score : " + GameControl.control.PlayerHighestScore;
        powerUpText.text = "Powerup Remaining Time : " + Player.powerUpRemainingTime;

    }

    

}
