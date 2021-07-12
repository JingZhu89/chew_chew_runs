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
    [SerializeField] private Transform floatingPlatform_short;
    [SerializeField] private Transform floatingPlatform_long;
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
    public float shortPlatformPercentage = 0.5f;
    public float minDistanceBetweenPlatforms = 2.0f;
    public float maxDistanceBetweenPlatforms = 10.0f;




    // spawn initial parts//
    private void Awake()
    {
        groundEndPosition = groundPart_Start.Find("right").position + (groundPart_Start.Find("right").position - groundPart_Start.Find("left").position) / 2;
        floatingEndPosition = new Vector3(Random.Range(minDistanceBetweenPlatforms, maxDistanceBetweenPlatforms), Random.Range(distanceToGround_1, distanceToGround_2), 0);

        for (int i=0; i>startingLevelParts;i++)
        {
            SpawnGroundPart();
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
                obstacle_1= SpawnObstacle(lastGroundPartTransform.Find("up").position);
                var obstacleDownPosition=obstacle_1.Find("down").localPosition;
                obstacle_1.position -= Vector3.Scale(obstacleDownPosition ,obstacle_1.localScale);
                               
                obstacleJustSpawned = true;
            }
            else
            { obstacleJustSpawned = false;
            if(Random.Range(0.0f, 1.0f) < collectableSpawnPercentage)
                {
                    collectable_coin = SpawnCollectable(lastGroundPartTransform.Find("up").position);
                    var coinDownPosition = collectable_coin.Find("down").localPosition;
                    collectable_coin.position -= Vector3.Scale(coinDownPosition, collectable_coin.localScale);

                }

            }


            manholeJustSpawned = false;
        }
        
        groundEndPosition = lastGroundPartTransform.Find("right").position + (lastGroundPartTransform.Find("right").position - lastGroundPartTransform.Find("left").position) / 2;
    }


// define spawning method for floating parts//
    private void SpawnFloatingPart()
    {
        Transform lastFloatingPartTransform;
        lastFloatingPartTransform = SpawnFloatingPlatform(floatingEndPosition);
        floatingEndPosition = new Vector3(lastFloatingPartTransform.Find("right").position.x + (lastFloatingPartTransform.Find("right").position.x - lastFloatingPartTransform.Find("left").position.x) / 2 + Random.Range(minDistanceBetweenPlatforms,maxDistanceBetweenPlatforms),Random.Range(distanceToGround_1, distanceToGround_2),0) ;


    }

       



    private Transform SpawnGround(Vector3 spawnPosition)
    {
            Transform GroundPartTransform = Instantiate(groundPart_1, spawnPosition, Quaternion.identity);
            return GroundPartTransform;
    }

    private Transform SpawnManhole(Vector3 spawnPosition)
    {
        Transform GroundPartTransform = Instantiate(groundManhole_1, spawnPosition, Quaternion.identity);
        return GroundPartTransform;
    }

    private Transform SpawnObstacle(Vector3 spawnPosition)
    {
        Transform obstacleTransform = Instantiate(obstacle_1, spawnPosition, Quaternion.identity);
        return obstacleTransform;
    }

       
    private Transform SpawnFloatingPlatform(Vector3 spawnPosition)
    {
        Transform floatingPlatformTransform;
        if (Random.Range(0.0f, 1.0f) <= shortPlatformPercentage)
        {  floatingPlatformTransform = Instantiate(floatingPlatform_short, spawnPosition, Quaternion.identity); }
        else
        {  floatingPlatformTransform = Instantiate(floatingPlatform_long, spawnPosition, Quaternion.identity); }
        return floatingPlatformTransform; 
        
    }


    private Transform SpawnCollectable(Vector3 spawnPosition)
    {
        Transform collectableTransform;
        collectableTransform = Instantiate(collectable_coin, spawnPosition, Quaternion.identity);
        return collectableTransform;
    }

}



