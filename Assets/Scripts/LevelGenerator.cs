using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{

    private const float PLAYER_DISTANCE_SPAWN_PART = 20f;
    [SerializeField] private Transform groundPart_Start;
    [SerializeField] private Transform groundPart_1;
    [SerializeField] private Transform groundManhole_1;
    [SerializeField] private Transform obstacle_1;
    [SerializeField] private PlayerMovement player;
    [SerializeField] private Transform floatingPlatform_middle;
    [SerializeField] private Transform floatingPlatform_left;
    [SerializeField] private Transform floatingPlatform_right;
    [SerializeField] private Transform floatingPlatform_single;
    [SerializeField] private Transform collectable_coin;

    public int startingLevelParts;
    public float manholePercentage = 0.15f;
    private Vector3 groundEndPosition;
    private Vector3 floatingEndPosition;
    public int maxContinuouseGround = 20;
    private bool manholeJustSpawned = false;
    public float obstacleSpawnPercentage = 0.10f;
    public float collectableSpawnPercentage = 0.15f;
    private bool obstacleJustSpawned = false;
    public float distanceToGround_1 = -1.0f;
    public float distanceToGround_2 = 2.8f;
    public int maxContinuousPlatform = 5;
    public float minDistanceBetweenPlatformSets = 5.0f;
    public float maxDistanceBetweenPlatformSets = 10.0f;




    // spawn initial parts//
    private void Awake()
    {
        groundEndPosition = groundPart_Start.Find("right").position + (groundPart_Start.Find("right").position - groundPart_Start.Find("left").position) / 2;
        floatingEndPosition = new Vector3(Random.Range(minDistanceBetweenPlatformSets, maxDistanceBetweenPlatformSets), Random.Range(distanceToGround_1, distanceToGround_2), 0);

        for (int i=0; i<startingLevelParts;i++)
        {
            SpawnBeginningGroundPart();
        }
  
    }


    // spawn level//

    private void Update()
    {
        if (Vector3.Distance(player.transform.position,groundEndPosition) < PLAYER_DISTANCE_SPAWN_PART )
        {
            SpawnGroundPart();
        }
        if (Vector3.Distance(player.transform.position,floatingEndPosition)< PLAYER_DISTANCE_SPAWN_PART)
        {
            SpawnFloatingPart();
        }
    }


// define spawning method for ground parts//

    private void SpawnGroundPart()
    {
        Transform lastGroundPartTransform;
  
        
        int numberOfGroundSpawned = 0;

        
        if ((Random.Range(0.0f, 1.0f)< manholePercentage || numberOfGroundSpawned>maxContinuouseGround) && manholeJustSpawned == false && obstacleJustSpawned == false)
        {
             lastGroundPartTransform = SpawnManhole(groundEndPosition);
             manholeJustSpawned = true;
             numberOfGroundSpawned = 0;
        }
        else 
        {
            lastGroundPartTransform = SpawnGround(groundEndPosition);
            numberOfGroundSpawned++;
             if (obstacleJustSpawned == false && Random.Range(0.0f, 1.0f) < obstacleSpawnPercentage && manholeJustSpawned == false)
            {
                Transform obstacleTransform= SpawnObstacle(lastGroundPartTransform.Find("up").position);
                var obstacleDownPosition= obstacleTransform.Find("down").localPosition;
                obstacleTransform.position -= Vector3.Scale(obstacleDownPosition , obstacleTransform.localScale);
                               
                obstacleJustSpawned = true;
            }
            else
            { obstacleJustSpawned = false;
            if(Random.Range(0.0f, 1.0f) < collectableSpawnPercentage)
                {
                    Transform CoinTrasnform = SpawnCollectable(lastGroundPartTransform.Find("up").position);
                    var coinDownPosition = CoinTrasnform.Find("down").localPosition;
                    CoinTrasnform.position -= Vector3.Scale(coinDownPosition, CoinTrasnform.localScale);

                }

            }


            manholeJustSpawned = false;
        }
        
        groundEndPosition = lastGroundPartTransform.Find("right").position + (lastGroundPartTransform.Find("right").position - lastGroundPartTransform.Find("left").position) / 2;
    }



    // define spawning method for begining of game ground parts//

    private void SpawnBeginningGroundPart()
    {
         
        Transform lastGroundPartTransform = SpawnGround(groundEndPosition);

        groundEndPosition = lastGroundPartTransform.Find("right").position + (lastGroundPartTransform.Find("right").position - lastGroundPartTransform.Find("left").position) / 2;
    }




    // define spawning method for floating parts//
    private void SpawnFloatingPart()
    {
        Transform lastFloatingPartTransform;
        int numberOfContinuousPlatforms = Random.Range(1, maxContinuousPlatform);
        int numberOfMiddlePlatforms = Mathf.Max(numberOfContinuousPlatforms - 2, 0);
        float xdistanceBetweenPlatformSets = Random.Range(minDistanceBetweenPlatformSets, maxDistanceBetweenPlatformSets);
        float ydistanceToGround = Random.Range(distanceToGround_1, distanceToGround_2);
        if (numberOfContinuousPlatforms == 1)
        {
            lastFloatingPartTransform = SpawnFloatingPlatformSingle(floatingEndPosition);
            SpawnObjectsOnPlatform(lastFloatingPartTransform);
            floatingEndPosition = new Vector3(GetFloatingEndXPosition(lastFloatingPartTransform) + xdistanceBetweenPlatformSets, ydistanceToGround, 0);
            
        }
        if (numberOfContinuousPlatforms >= 3)
        {
            lastFloatingPartTransform = SpawnFloatingPlatformLeft(floatingEndPosition);
            SpawnObjectsOnPlatform(lastFloatingPartTransform);
            floatingEndPosition = GetFloatingMiddlePosition(lastFloatingPartTransform);
            for (int i = 0; i < numberOfMiddlePlatforms; i++)
            {
                lastFloatingPartTransform = SpawnFloatingPlatformMiddle(floatingEndPosition);
                SpawnObjectsOnPlatform(lastFloatingPartTransform);
                floatingEndPosition = GetFloatingMiddlePosition(lastFloatingPartTransform);
            }
            lastFloatingPartTransform = SpawnFloatingPlatformRight(floatingEndPosition);
            SpawnObjectsOnPlatform(lastFloatingPartTransform);
            floatingEndPosition = new Vector3(GetFloatingEndXPosition(lastFloatingPartTransform) + xdistanceBetweenPlatformSets, ydistanceToGround, 0);

        }
        else
        {
            lastFloatingPartTransform = SpawnFloatingPlatformLeft(floatingEndPosition);
            SpawnObjectsOnPlatform(lastFloatingPartTransform);
            floatingEndPosition = GetFloatingMiddlePosition(lastFloatingPartTransform);
            lastFloatingPartTransform = SpawnFloatingPlatformRight(floatingEndPosition);
            SpawnObjectsOnPlatform(lastFloatingPartTransform);
            floatingEndPosition = new Vector3(GetFloatingEndXPosition(lastFloatingPartTransform) + xdistanceBetweenPlatformSets, ydistanceToGround, 0);

        }

    }

    private float GetFloatingEndXPosition(Transform lastFoatingPartTransform)
    {
        return lastFoatingPartTransform.Find("right").position.x + (lastFoatingPartTransform.Find("right").position.x - lastFoatingPartTransform.Find("left").position.x) / 2;
    }

    private Vector3 GetFloatingMiddlePosition(Transform lastFloatingPartTransform)
    {
        return lastFloatingPartTransform.Find("right").position + (lastFloatingPartTransform.Find("right").position - lastFloatingPartTransform.Find("left").position) / 2;
    }


    private void SpawnObjectsOnPlatform(Transform lastFloatingPartTransform)
    {
        if (Random.Range(0.0f, 1.0f) < obstacleSpawnPercentage)
        {
            obstacle_1 = SpawnObstacle(lastFloatingPartTransform.Find("up").position);
            var obstacleDownPosition = obstacle_1.Find("down").localPosition;
            obstacle_1.position -= Vector3.Scale(obstacleDownPosition, obstacle_1.localScale);
        }

        else if (Random.Range(0.0f, 1.0f) < collectableSpawnPercentage)
            {
                Transform CoinTrasnform = SpawnCollectable(lastFloatingPartTransform.Find("up").position);
                var coinDownPosition = CoinTrasnform.Find("down").localPosition;
                CoinTrasnform.position -= Vector3.Scale(coinDownPosition, CoinTrasnform.localScale);

            }

        
    }


    private Transform SpawnGround(Vector3 spawnPosition)
    {
            Transform groundPartTransform = Instantiate(groundPart_1, spawnPosition, Quaternion.identity);
            return groundPartTransform;
    }

    private Transform SpawnManhole(Vector3 spawnPosition)
    {
        Transform groundPartTransform = Instantiate(groundManhole_1, spawnPosition, Quaternion.identity);
        return groundPartTransform;
    }

    private Transform SpawnObstacle(Vector3 spawnPosition)
    {
        Transform obstacleTransform = Instantiate(obstacle_1, spawnPosition, Quaternion.identity);
        return obstacleTransform;
    }

       
    private Transform SpawnFloatingPlatformMiddle(Vector3 spawnPosition)
    {
        Transform floatingPlatformTransform = Instantiate(floatingPlatform_middle, spawnPosition, Quaternion.identity); 
         return floatingPlatformTransform;
    }

    private Transform SpawnFloatingPlatformLeft(Vector3 spawnPosition)
    {
        Transform floatingPlatformTransform = Instantiate(floatingPlatform_left, spawnPosition, Quaternion.identity);
        return floatingPlatformTransform;
    }

    private Transform SpawnFloatingPlatformRight(Vector3 spawnPosition)
    {
        Transform floatingPlatformTransform = Instantiate(floatingPlatform_right, spawnPosition, Quaternion.identity);
        return floatingPlatformTransform;
    }


    private Transform SpawnFloatingPlatformSingle(Vector3 spawnPosition)
    {
        Transform floatingPlatformTransform = Instantiate(floatingPlatform_single, spawnPosition, Quaternion.identity);
        return floatingPlatformTransform;
    }


    private Transform SpawnCollectable(Vector3 spawnPosition)
    {
        Transform collectableTransform;
        collectableTransform = Instantiate(collectable_coin, spawnPosition, Quaternion.identity);
        return collectableTransform;
    }

}



