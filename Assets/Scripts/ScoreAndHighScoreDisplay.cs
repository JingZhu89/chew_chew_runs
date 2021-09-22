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

    private void Start()
    {
        powerUpText.color = new Vector4(0.0f, 0.0f, 0.0f, 0.0f);
    }

    void Update()
    {
        scoreText.text = "Current Score : " + Player.playerScore;
        topScoreText.text = "Top Score : " + GameControl.control.PlayerHighestScore;
        powerUpText.text = Player.powerUpRemainingTime.ToString() + " s left";
        if (Player.freeze == true || Player.crashThroughEverything == true || Player.flyingMode == true)
        {
            powerUpText.color = new Vector4(1.0f, 1.0f, 1.0f, 1.0f);
        }
        else
        {
            powerUpText.color = new Vector4(1.0f, 1.0f, 1.0f, 0.0f);
        }


    }

    

}
