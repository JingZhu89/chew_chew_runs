using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GetGroundObstacle : MonoBehaviour
{

    public static GetGroundObstacle SharedInstance;
    public List<ObstacleProbability> obstacles;

    private void Awake()
    {
        SharedInstance = this;

    }

    public GameObject getGroundObstacle()
    {
        int runningTotal = 0;
        int sum = obstacles.Select(p => p.probability).Sum();
        int rand = Random.Range(0, sum);

        foreach (var obstacle in obstacles)
        {
            runningTotal = runningTotal + obstacle.probability;
            if (rand < runningTotal)
            {
                return obstacle.obstacleObject;
            }
        }
        return null;
    }




}
