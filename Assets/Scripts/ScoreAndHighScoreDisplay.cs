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
    public Text scoreAnimationText;
    private int pointsJustEarnedOld = 0;
    Animator scoreAnimator;
        
    private void Start()
    {
        powerUpText.color = new Vector4(0.0f, 0.0f, 0.0f, 0.0f);
        scoreAnimator = scoreAnimationText.gameObject.GetComponent<Animator>();

    }

    void Update()
    {
        scoreText.text = "Current Score : " + Player.playerScore;
        topScoreText.text = "Top Score : " + GameControl.control.PlayerHighestScore;
        powerUpText.text = Player.powerUpRemainingTime.ToString() + " s left";
        if (Player.pointsJustEarnedNew >= 0)
        {
            scoreAnimationText.text = "+" + Player.pointsJustEarnedNew;
        }
        if(Player.pointsJustEarnedNew<0)
        {
            scoreAnimationText.text = Player.pointsJustEarnedNew.ToString();
        }

        if (Player.freeze == true || Player.crashThroughEverything == true || Player.flyingMode == true)
        {
            powerUpText.color = new Vector4(1.0f, 1.0f, 1.0f, 1.0f);
        }
        else
        {
            powerUpText.color = new Vector4(1.0f, 1.0f, 1.0f, 0.0f);
        }
        if(pointsJustEarnedOld != Player.pointsJustEarnedNew)
        {
            scoreAnimator.SetTrigger("NewScoreAdded");
            pointsJustEarnedOld = Player.pointsJustEarnedNew;

        }
        else
        {
            scoreAnimator.ResetTrigger("NewScoreAdded");
        }

    }

    

}
