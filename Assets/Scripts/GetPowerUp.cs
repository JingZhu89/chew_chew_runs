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
        // TODO - change this based on player speed
        bool playerIsReallyFast = PlayerMovement.SharedInstance.currentSpeed >= PlayerMovement.SharedInstance.speedThreshold1;


        List<PowerUpProbabilityAndDuration> filteredPowerups;
        if (playerIsReallyFast)
        {
            filteredPowerups = powerUps;
        }
        else
        {
            filteredPowerups = powerUps.Where(p => !p.onlySpawnWhenPlayerIsFast).ToList();
        }

        int runningTotal = 0;
        int sum = filteredPowerups.Select(p => p.probability).Sum();
        int rand = Random.Range(0, sum);

        foreach (var powerUp in filteredPowerups)
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
