using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{

    private const float PLAYER_DISTANCE_SPAWN_PART = 20f;
    [SerializeField] private Transform groundPart_Start;
    [SerializeField] private Transform groundManhole_1;
    [SerializeField] private PlayerMovement player;
    public int startingLevelParts;
    public float manholePercentage = 0.15f;
    private Vector3 groundEndPosition;
    private Vector3 floatingEndPosition;
    public int maxContinuouseGround = 20;
    public int minDistanceBetweenObstacles = 3;
    public int minDistanceBetweenManholes = 2;
    private bool manholeJustSpawned = false;
    public float obstacleSpawnPercentage = 0.10f;
    public float collectableSpawnPercentage = 0.15f;
    public float powerUpSpawnPercentage = 0.08f;
    private bool obstacleJustSpawned = false;
    public float distanceToGround_1 = -1.0f;
    public float distanceToGround_2 = 2.8f;
    public int maxContinuousPlatform = 5;
    public float minDistanceBetweenPlatformSets = 5.0f;
    public float maxDistanceBetweenPlatformSets = 10.0f;

    int numberOfGroundSpawned;
    int numberOfNoObstacleSpawned;




    // spawn initial parts//
    private void Start()
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
        float obstaclesSpawnRNG = Random.Range(0.0f, 1.0f);
        float collectableSpawnRNG = Random.Range(0.0f, 1.0f);
        float powerUpSpawnRNG = Random.Range(0.0f, 1.0f);



        
        if ((Random.Range(0.0f, 1.0f)< manholePercentage || numberOfGroundSpawned>maxContinuouseGround) && obstacleJustSpawned == false && numberOfGroundSpawned > minDistanceBetweenManholes)
        {
             lastGroundPartTransform = SpawnManhole(groundEndPosition);
             manholeJustSpawned = true;
             numberOfGroundSpawned = 0;
        }
        else 
        {
            lastGroundPartTransform = SpawnGround(groundEndPosition);
            numberOfGroundSpawned++; print("number of ground spawned =" + numberOfGroundSpawned);

            if (obstacleJustSpawned == false && obstaclesSpawnRNG < obstacleSpawnPercentage && manholeJustSpawned == false && numberOfNoObstacleSpawned > minDistanceBetweenObstacles)
            {
                Transform obstacleTransform= SpawnObstacle(lastGroundPartTransform.Find("up").position);
                var obstacleDownPosition = obstacleTransform.Find("down").localPosition;
                obstacleTransform.position -= Vector3.Scale(obstacleDownPosition, obstacleTransform.localScale);        
                obstacleJustSpawned = true;
                numberOfNoObstacleSpawned = 0;
            }

            else
            {
                obstacleJustSpawned = false;
                numberOfNoObstacleSpawned++; print("number of noObstacle spawned =" + numberOfNoObstacleSpawned);

                if (collectableSpawnRNG < collectableSpawnPercentage)
                {
                    Transform collectableTrasnform = SpawnCollectable(lastGroundPartTransform.Find("up").position);
                    var coinDownPosition = collectableTrasnform.Find("down").localPosition;
                    collectableTrasnform.position -= Vector3.Scale(coinDownPosition, collectableTrasnform.localScale);
                }

                else if (powerUpSpawnRNG < powerUpSpawnPercentage)
                {
                    Transform powerupTrasnform = SpawnPowerUps(lastGroundPartTransform.Find("up").position);
                    var chestDownPosition = powerupTrasnform.Find("down").localPosition;
                    powerupTrasnform.position -= Vector3.Scale(chestDownPosition, powerupTrasnform.localScale);

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

        float obstaclesSpawnRNG = Random.Range(0.0f, 1.0f);
        float collectableSpawnRNG = Random.Range(0.0f, 1.0f);
        float powerUpSpawnRNG = Random.Range(0.0f, 1.0f);

        if (obstaclesSpawnRNG < obstacleSpawnPercentage)
        {
            Transform obstacleTransform = SpawnObstacle(lastFloatingPartTransform.Find("up").position);
            var obstacleDownPosition = obstacleTransform.Find("down").localPosition;
            obstacleTransform.position -= Vector3.Scale(obstacleDownPosition, obstacleTransform.localScale);
        }

        else if (collectableSpawnRNG < collectableSpawnPercentage)
        {
            Transform collectableTrasnform = SpawnCollectable(lastFloatingPartTransform.Find("up").position);
            var collectableDownPosition = collectableTrasnform.Find("down").localPosition;
            collectableTrasnform.position -= Vector3.Scale(collectableDownPosition, collectableTrasnform.localScale);
        }
        else if (powerUpSpawnRNG < powerUpSpawnPercentage)
        {
            Transform powerUpTransform = SpawnPowerUps(lastFloatingPartTransform.Find("up").position);
            var powerUpDownPosition = powerUpTransform.Find("down").localPosition;
            powerUpTransform.position -= Vector3.Scale(powerUpDownPosition, powerUpTransform.localScale);
        }

    }


    //grab the object to spawn//


    private Transform SpawnGround(Vector3 spawnPosition)
    {
        GameObject groundPart = TileObjectPool.SharedInstance.GetPooledGroundTiles();
        if(groundPart != null)
        {
            groundPart.transform.position = spawnPosition;
            groundPart.SetActive(true);
            return groundPart.transform;
        }
        return null;
    }

    private Transform SpawnManhole(Vector3 spawnPosition)
    {
        GameObject waterTile = TileObjectPool.SharedInstance.GetPooledWaterTiles();
        if (waterTile != null)
        {
            waterTile.transform.position = spawnPosition;
            waterTile.SetActive(true);
            return waterTile.transform;
        }
        return null;
    }



       
    private Transform SpawnFloatingPlatformMiddle(Vector3 spawnPosition)
    {
        GameObject platformMiddlePart = TileObjectPool.SharedInstance.GetPooledPlatformMiddleTiles();
        if (platformMiddlePart != null)
        {
            platformMiddlePart.transform.position = spawnPosition;
            platformMiddlePart.SetActive(true);
            return platformMiddlePart.transform;
        }
        return null;
    }

    private Transform SpawnFloatingPlatformLeft(Vector3 spawnPosition)
    {
        GameObject platformLeftPart = TileObjectPool.SharedInstance.GetPooledPlatformLeftTiles();
        if (platformLeftPart != null)
        {
            platformLeftPart.transform.position = spawnPosition;
            platformLeftPart.SetActive(true);
            return platformLeftPart.transform;
        }
        return null;
    }

    private Transform SpawnFloatingPlatformRight(Vector3 spawnPosition)
    {
        GameObject platformRightPart = TileObjectPool.SharedInstance.GetPooledPlatformRightTiles();
        if (platformRightPart != null)
        {
            platformRightPart.transform.position = spawnPosition;
            platformRightPart.SetActive(true);
            return platformRightPart.transform;
        }
        return null;
    }


    private Transform SpawnFloatingPlatformSingle(Vector3 spawnPosition)
    {
        GameObject platformSinglePart = TileObjectPool.SharedInstance.GetPooledPlatformSingleTiles();
        if (platformSinglePart != null)
        {
            platformSinglePart.transform.position = spawnPosition;
            platformSinglePart.SetActive(true);
            return platformSinglePart.transform;
        }
        return null;
    }


    private Transform SpawnCollectable(Vector3 spawnPosition)
    {
        Transform CollectableTransform;
        var collectable = GetCollectable.SharedInstance.getCollectable();
        CollectableTransform = Instantiate(collectable.gameObject, spawnPosition, Quaternion.identity).transform;
        return CollectableTransform;
    }


    private Transform SpawnPowerUps(Vector3 spawnPosition)
    {
        Transform  PowerUpTransform;
        var powerup = GetPowerUp.SharedInstance.getPowerUp();
        PowerUpTransform = Instantiate(powerup.gameObject, spawnPosition, Quaternion.identity).transform;
        return PowerUpTransform;
    }


    private Transform SpawnObstacle(Vector3 spawnPosition)
    {
        Transform ObstacleTransform;
        var obstacle = GetObstacle.SharedInstance.getObstacle();
        ObstacleTransform = Instantiate(obstacle, spawnPosition, Quaternion.identity).transform;
        return ObstacleTransform;
    }




}



