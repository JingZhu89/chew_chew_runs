using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GetPowerUp : MonoBehaviour
{
    public static GetPowerUp SharedInstance;
    public List<PowerUpProbabilityAndDuration> powerUps;

    private void Awake()
    {
        SharedInstance = this;
    }

    public PowerUp getPowerUp()
    {
        int runningTotal = 0;
        int sum = powerUps.Select(p => p.probability).Sum();
        int rand = Random.Range(0, sum);

        foreach (var powerUp in powerUps)
        {
            runningTotal = runningTotal + powerUp.probability;
            if (rand<runningTotal)
            {
                return powerUp.powerUpObject;
            }
        }
        return null;
    }
}
