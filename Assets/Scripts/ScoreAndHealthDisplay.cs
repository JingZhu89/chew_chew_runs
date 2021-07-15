using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreAndHealthDisplay : MonoBehaviour
{
    public PlayerMovement Player;
    public Text scoreText;
    public Text healthText;
    public Text powerUpText;
    void Update()
    {
        scoreText.text = "Score : " + Player.playerScore;
        healthText.text = "Health : " + Player.healthScore;
        powerUpText.text = "Powerup Remaining Time : " + Player.powerUpRemainingTime;
    }
}
