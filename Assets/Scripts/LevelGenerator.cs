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
    public int minDistanceBetweenManholes = 2;
    private bool manholeJustSpawned = false;
    public float obstacleSpawnPercentage = 0.10f;
    public float collectableSpawnPercentage = 0.15f;
    public float powerUpSpawnPercentage = 0.08f;
    private bool obstacleJustSpawned = false;
    public float distanceToGround_1 = -1.0f;
    public float distanceToGround_2 = 2.8f;
    public float floatingDistance1Vs2Ratio = 0.5f;
    public int maxContinuousPlatform = 5;
    public float minDistanceBetweenPlatformSets = 5.0f;
    public float maxDistanceBetweenPlatformSets = 10.0f;
    int numberOfGroundSpawned;
    int numberOfNoObstacleSpawned;
    public int minDistanceBetweenObstacles=4;
    public float floatingTallVsFlatRatio = 0.5f;


    // spawn initial parts//
    private void Start()
    {
        groundEndPosition = groundPart_Start.Find("right").position + (groundPart_Start.Find("right").position - groundPart_Start.Find("left").position) / 2;
        floatingEndPosition = new Vector3(Random.Range(minDistanceBetweenPlatformSets, maxDistanceBetweenPlatformSets), GetFloatingPlatformHeight(), 0);

        for (int i=0; i<startingLevelParts;i++)
        {
            SpawnBeginningGroundPart();
        }
  
    }


    // spawn level//

    private void Update()
    {

        if (Vector3.Distance(player.transform.position, floatingEndPosition) < PLAYER_DISTANCE_SPAWN_PART)
        {
            SpawnFloatingPart();
        }
        if (Vector3.Distance(player.transform.position,groundEndPosition) < PLAYER_DISTANCE_SPAWN_PART )
        {
            SpawnGroundPart();
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
            float castDistance = 1.5f;
            var mask = LayerMask.GetMask("floating platform");
            RaycastHit2D hitleft = Physics2D.Raycast(lastGroundPartTransform.Find("left").position, Vector2.up, castDistance, mask);
            RaycastHit2D hitright = Physics2D.Raycast(lastGroundPartTransform.Find("right").position, Vector2.up, castDistance, mask);
            RaycastHit2D hitmiddle = Physics2D.Raycast(lastGroundPartTransform.Find("up").position, Vector2.up, castDistance, mask);
            manholeJustSpawned = false;


            numberOfGroundSpawned++; 


            //spawn obstable and collectable on ground parts//

            if (obstacleJustSpawned == false && obstaclesSpawnRNG < obstacleSpawnPercentage && manholeJustSpawned == false && hitleft.collider==null && hitright.collider==null && hitmiddle.collider==null)
            {
                Transform obstacleTransform= SpawnObstacle(lastGroundPartTransform.Find("up").position);
                var obstacleDownPosition = obstacleTransform.Find("down").localPosition;
                obstacleTransform.position -= Vector3.Scale(obstacleDownPosition, obstacleTransform.localScale);        
                obstacleJustSpawned = true;
                numberOfNoObstacleSpawned = 0;
            }

            else
            {
                numberOfNoObstacleSpawned++;
                obstacleJustSpawned = false;
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
        float ydistanceToGround = GetFloatingPlatformHeight();
        float platformTypeRNG = Random.Range(0.0f, 1.0f);

        if (numberOfContinuousPlatforms == 1)
        {
            if (platformTypeRNG <= floatingTallVsFlatRatio)
            {
                lastFloatingPartTransform = SpawnFloatingPlatformSingle(floatingEndPosition);
                SpawnObjectsOnPlatform(lastFloatingPartTransform);
                floatingEndPosition = new Vector3(GetFloatingEndXPosition(lastFloatingPartTransform) + xdistanceBetweenPlatformSets, ydistanceToGround, 0);
            }
            else
            {
                lastFloatingPartTransform = SpawnFloatingTallPlatformSingle(floatingEndPosition);
                SpawnObjectsOnPlatform(lastFloatingPartTransform);
                floatingEndPosition = new Vector3(GetFloatingEndXPosition(lastFloatingPartTransform) + xdistanceBetweenPlatformSets, ydistanceToGround, 0);
            }
            
        }
        if (numberOfContinuousPlatforms >= 3)
        {
            if (platformTypeRNG <= floatingTallVsFlatRatio)
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
                lastFloatingPartTransform = SpawnFloatingTallPlatformLeft(floatingEndPosition);
                SpawnObjectsOnPlatform(lastFloatingPartTransform);
                floatingEndPosition = GetFloatingMiddlePosition(lastFloatingPartTransform);
                for (int i = 0; i < numberOfMiddlePlatforms; i++)
                {
                    lastFloatingPartTransform = SpawnFloatingTallPlatformMiddle(floatingEndPosition);
                    SpawnObjectsOnPlatform(lastFloatingPartTransform);
                    floatingEndPosition = GetFloatingMiddlePosition(lastFloatingPartTransform);
                }
                lastFloatingPartTransform = SpawnFloatingTallPlatformRight(floatingEndPosition);
                SpawnObjectsOnPlatform(lastFloatingPartTransform);
                floatingEndPosition = new Vector3(GetFloatingEndXPosition(lastFloatingPartTransform) + xdistanceBetweenPlatformSets, ydistanceToGround, 0);
            }

        }
        else
        {
            if (platformTypeRNG <= floatingTallVsFlatRatio)
            {
                lastFloatingPartTransform = SpawnFloatingPlatformLeft(floatingEndPosition);
                SpawnObjectsOnPlatform(lastFloatingPartTransform);
                floatingEndPosition = GetFloatingMiddlePosition(lastFloatingPartTransform);
                lastFloatingPartTransform = SpawnFloatingPlatformRight(floatingEndPosition);
                SpawnObjectsOnPlatform(lastFloatingPartTransform);
                floatingEndPosition = new Vector3(GetFloatingEndXPosition(lastFloatingPartTransform) + xdistanceBetweenPlatformSets, ydistanceToGround, 0);
            }
            else
            {
                lastFloatingPartTransform = SpawnFloatingTallPlatformLeft(floatingEndPosition);
                SpawnObjectsOnPlatform(lastFloatingPartTransform);
                floatingEndPosition = GetFloatingMiddlePosition(lastFloatingPartTransform);
                lastFloatingPartTransform = SpawnFloatingTallPlatformRight(floatingEndPosition);
                SpawnObjectsOnPlatform(lastFloatingPartTransform);
                floatingEndPosition = new Vector3(GetFloatingEndXPosition(lastFloatingPartTransform) + xdistanceBetweenPlatformSets, ydistanceToGround, 0);
            }

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


    //spawon objects on platform//

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


    private float GetFloatingPlatformHeight()
    {
        float floatingHeightRNG = Random.Range(1.0f, 0.0f);
        if(floatingHeightRNG <= floatingDistance1Vs2Ratio)
        {
           return distanceToGround_1;
        }
        else
        {
           return distanceToGround_2;
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


    //floating flat//
       
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

    //floating tall//
    private Transform SpawnFloatingTallPlatformMiddle(Vector3 spawnPosition)
    {
        GameObject platformMiddlePart = TileObjectPool.SharedInstance.GetPooledTallPlatformMiddleTiles();
        if (platformMiddlePart != null)
        {
            platformMiddlePart.transform.position = spawnPosition;
            platformMiddlePart.SetActive(true);
            return platformMiddlePart.transform;
        }
        return null;
    }

    private Transform SpawnFloatingTallPlatformLeft(Vector3 spawnPosition)
    {
        GameObject platformLeftPart = TileObjectPool.SharedInstance.GetPooledTallPlatformLeftTiles();
        if (platformLeftPart != null)
        {
            platformLeftPart.transform.position = spawnPosition;
            platformLeftPart.SetActive(true);
            return platformLeftPart.transform;
        }
        return null;
    }

    private Transform SpawnFloatingTallPlatformRight(Vector3 spawnPosition)
    {
        GameObject platformRightPart = TileObjectPool.SharedInstance.GetPooledTallPlatformRightTiles();
        if (platformRightPart != null)
        {
            platformRightPart.transform.position = spawnPosition;
            platformRightPart.SetActive(true);
            return platformRightPart.transform;
        }
        return null;
    }


    private Transform SpawnFloatingTallPlatformSingle(Vector3 spawnPosition)
    {
        GameObject platformSinglePart = TileObjectPool.SharedInstance.GetPooledTallPlatformSingleTiles();
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



