using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileObjectPool : MonoBehaviour
{
    public static TileObjectPool SharedInstance;

    public List<GameObject> pooledGroundTiles_1;
    public GameObject groundTilesToPool_1;
    public int numberOfGroundTilesToPool;
    public List<GameObject> pooledGroundTiles_2;
    public GameObject groundTilesToPool_2;

    public List<GameObject> pooledForeground_1;
    public GameObject foregroundToPool_1;
    public List<GameObject> pooledForeground_2;
    public GameObject foregroundToPool_2;
    public List<GameObject> pooledForeground_3;
    public GameObject foregroundToPool_3;
    public List<GameObject> pooledForeground_4;
    public GameObject foregroundToPool_4;
    public int numberOfForegroundToPool;

    public List<GameObject> pooledForegroundBase;
    public GameObject foregourndBaseToPool;
    public int numberOfForegourndBaseToPool;

         
    public List<GameObject> pooledWaterTiles;
    public GameObject waterTilesToPool;
    public int numberOfWaterTilesToPool;

    public List<GameObject> pooledPlatformSingleTiles;
    public GameObject platformSingleTilesToPool;
    public int numberOfPlatformSingleTilesToPool;


    public List<GameObject> pooledPlatformLeftTiles;
    public GameObject platformLeftTilesToPool;
    public int numberOfPlatformLeftTilesToPool;


    public List<GameObject> pooledPlatformRightTiles;
    public GameObject platformRightTilesToPool;
    public int numberOfPlatformRightTilesToPool;

    public List<GameObject> pooledPlatformMiddleTiles;
    public GameObject platformMiddleTilesToPool;
    public int numberOfPlatformMiddleTilesToPool;


    public List<GameObject> pooledTallPlatformSingleTiles;
    public GameObject tallPlatformSingleTilesToPool;
    public int numberOfTallPlatformSingleTilesToPool;


    public List<GameObject> pooledTallPlatformLeftTiles;
    public GameObject tallPlatformLeftTilesToPool;
    public int numberOfTallPlatformLeftTilesToPool;


    public List<GameObject> pooledTallPlatformRightTiles;
    public GameObject tallPlatformRightTilesToPool;
    public int numberOfTallPlatformRightTilesToPool;

    public List<GameObject> pooledTallPlatformMiddleTiles;
    public GameObject tallPlatformMiddleTilesToPool;
    public int numberOfTallPlatformMiddleTilesToPool;


    // Start is called before the first frame update

    private void Awake()
    {
        SharedInstance = this;
    }
    void Start()
    {

        //create objectpool for groud tiles
        pooledGroundTiles_1 = new List<GameObject>();
        GameObject tmpG1;
        for (int i = 0; i < numberOfGroundTilesToPool; i++)
        {
            tmpG1 = Instantiate(groundTilesToPool_1);
            tmpG1.SetActive(false);
            pooledGroundTiles_1.Add(tmpG1);
        }


        pooledGroundTiles_2 = new List<GameObject>();
        GameObject tmpG2;
        for (int i = 0; i < numberOfGroundTilesToPool; i++)
        {
            tmpG2 = Instantiate(groundTilesToPool_2);
            tmpG2.SetActive(false);
            pooledGroundTiles_2.Add(tmpG2);
        }





        //create objectpool for water tiles
        pooledWaterTiles = new List<GameObject>();
        GameObject tmpW;
        for (int i = 0; i < numberOfWaterTilesToPool; i++)
        {
            tmpW = Instantiate(waterTilesToPool);
            tmpW.SetActive(false);
            pooledWaterTiles.Add(tmpW);
        }


        //create object pool for flat platform tiles

        pooledPlatformSingleTiles = new List<GameObject>();
        GameObject tmp1;
        for (int i = 0; i < numberOfPlatformSingleTilesToPool; i++)
        {
            tmp1 = Instantiate(platformSingleTilesToPool);
            tmp1.SetActive(false);
            pooledPlatformSingleTiles.Add(tmp1);
        }


        pooledPlatformMiddleTiles = new List<GameObject>();
        GameObject tmp2;
        for (int i = 0; i < numberOfPlatformMiddleTilesToPool; i++)
        {
            tmp2 = Instantiate(platformMiddleTilesToPool);
            tmp2.SetActive(false);
            pooledPlatformMiddleTiles.Add(tmp2);
        }


        pooledPlatformLeftTiles = new List<GameObject>();
        GameObject tmp3;
        for (int i = 0; i < numberOfPlatformLeftTilesToPool; i++)
        {
            tmp3 = Instantiate(platformLeftTilesToPool);
            tmp3.SetActive(false);
            pooledPlatformLeftTiles.Add(tmp3);
        }


        pooledPlatformRightTiles = new List<GameObject>();
        GameObject tmp4;
        for (int i = 0; i < numberOfPlatformRightTilesToPool; i++)
        {
            tmp4 = Instantiate(platformRightTilesToPool);
            tmp4.SetActive(false);
            pooledPlatformRightTiles.Add(tmp4);
        }

        //create object pool for tall paltform tiles

        pooledTallPlatformSingleTiles = new List<GameObject>();
        GameObject tmp5;
        for (int i = 0; i < numberOfTallPlatformSingleTilesToPool; i++)
        {
            tmp5 = Instantiate(tallPlatformSingleTilesToPool);
            tmp5.SetActive(false);
            pooledTallPlatformSingleTiles.Add(tmp5);
        }


        pooledTallPlatformMiddleTiles = new List<GameObject>();
        GameObject tmp6;
        for (int i = 0; i < numberOfTallPlatformMiddleTilesToPool; i++)
        {
            tmp6 = Instantiate(tallPlatformMiddleTilesToPool);
            tmp6.SetActive(false);
            pooledTallPlatformMiddleTiles.Add(tmp6);
        }


        pooledTallPlatformLeftTiles = new List<GameObject>();
        GameObject tmp7;
        for (int i = 0; i < numberOfTallPlatformLeftTilesToPool; i++)
        {
            tmp7 = Instantiate(tallPlatformLeftTilesToPool);
            tmp7.SetActive(false);
            pooledTallPlatformLeftTiles.Add(tmp7);
        }


        pooledTallPlatformRightTiles = new List<GameObject>();
        GameObject tmp8;
        for (int i = 0; i < numberOfTallPlatformRightTilesToPool; i++)
        {
            tmp8 = Instantiate(tallPlatformRightTilesToPool);
            tmp8.SetActive(false);
            pooledTallPlatformRightTiles.Add(tmp8);
        }

        //create object pool for foreground and foreground base

        pooledForeground_1 = new List<GameObject>();
        GameObject tmpF1;
        for (int i = 0; i < numberOfForegroundToPool; i++)
        {
            tmpF1 = Instantiate(foregroundToPool_1);
            tmpF1.SetActive(false);
            pooledForeground_1.Add(tmpF1);
        }

        pooledForeground_2 = new List<GameObject>();
        GameObject tmpF2;
        for (int i = 0; i < numberOfForegroundToPool; i++)
        {
            tmpF2 = Instantiate(foregroundToPool_2);
            tmpF2.SetActive(false);
            pooledForeground_2.Add(tmpF2);
        }

        pooledForeground_3 = new List<GameObject>();
        GameObject tmpF3;
        for (int i = 0; i < numberOfForegroundToPool; i++)
        {
            tmpF3 = Instantiate(foregroundToPool_3);
            tmpF3.SetActive(false);
            pooledForeground_3.Add(tmpF3);
        }

        pooledForeground_4 = new List<GameObject>();
        GameObject tmpF4;
        for (int i = 0; i < numberOfForegroundToPool; i++)
        {
            tmpF4 = Instantiate(foregroundToPool_4);
            tmpF4.SetActive(false);
            pooledForeground_4.Add(tmpF4);
        }


        pooledForegroundBase = new List<GameObject>();
        GameObject tempFB;
        for (int i = 0; i< numberOfForegourndBaseToPool; i++)
        {
            tempFB = Instantiate(foregourndBaseToPool);
            tempFB.SetActive(false);
            pooledForegroundBase.Add(tempFB);
        }


    }

    //get pooled objects//

    public GameObject GetPooledGroundTiles(int i)
    {


            if (i % 2 == 0)
            {
                for (int n = 0; n < numberOfGroundTilesToPool; n++)
                {
                    if (!pooledGroundTiles_1[n].activeInHierarchy)
                    { return pooledGroundTiles_1[n]; }
                }

            }
            if (i % 2 == 1)
            {
                for (int n = 0; n < numberOfGroundTilesToPool; n++)
                {
                    if (!pooledGroundTiles_2[n].activeInHierarchy)
                    { return pooledGroundTiles_2[n]; }
                }
            }

     return null;
    }


    public GameObject GetPooledWaterTiles()
    {

        for (int i = 0; i < numberOfWaterTilesToPool; i++)
        {
            if (!pooledWaterTiles[i].activeInHierarchy)
            {
                return pooledWaterTiles[i];
            }
        }
        return null;
    }

    public GameObject GetPooledPlatformSingleTiles()
    {
        for (int i = 0; i < numberOfPlatformSingleTilesToPool; i++)
        {
            if (!pooledPlatformSingleTiles[i].activeInHierarchy)
            {
                return pooledPlatformSingleTiles[i];
            }
        }
        return null;
    }

    public GameObject GetPooledPlatformMiddleTiles()
    {
        for (int i = 0; i < numberOfPlatformMiddleTilesToPool; i++)
        {
            if (!pooledPlatformMiddleTiles[i].activeInHierarchy)
            {
                return pooledPlatformMiddleTiles[i];
            }
        }
        return null;
    }

    public GameObject GetPooledPlatformLeftTiles()
    {
        for (int i = 0; i < numberOfPlatformLeftTilesToPool; i++)
        {
            if (!pooledPlatformLeftTiles[i].activeInHierarchy)
            {
                return pooledPlatformLeftTiles[i];
            }
        }
        return null;
    }


    public GameObject GetPooledPlatformRightTiles()
    {
        for (int i = 0; i < numberOfPlatformRightTilesToPool; i++)
        {
            if (!pooledPlatformRightTiles[i].activeInHierarchy)
            {
                return pooledPlatformRightTiles[i];
            }
        }
        return null;
    }
    // tall platforms

    public GameObject GetPooledTallPlatformSingleTiles()
    {
        for (int i = 0; i < numberOfTallPlatformSingleTilesToPool; i++)
        {
            if (!pooledTallPlatformSingleTiles[i].activeInHierarchy)
            {
                return pooledTallPlatformSingleTiles[i];
            }
        }
        return null;
    }

    public GameObject GetPooledTallPlatformMiddleTiles()
    {
        for (int i = 0; i < numberOfTallPlatformMiddleTilesToPool; i++)
        {
            if (!pooledTallPlatformMiddleTiles[i].activeInHierarchy)
            {
                return pooledTallPlatformMiddleTiles[i];
            }
        }
        return null;
    }

    public GameObject GetPooledTallPlatformLeftTiles()
    {
        for (int i = 0; i < numberOfTallPlatformLeftTilesToPool; i++)
        {
            if (!pooledTallPlatformLeftTiles[i].activeInHierarchy)
            {
                return pooledTallPlatformLeftTiles[i];
            }
        }
        return null;
    }


    public GameObject GetPooledTallPlatformRightTiles()
    {
        for (int i = 0; i < numberOfTallPlatformRightTilesToPool; i++)
        {
            if (!pooledTallPlatformRightTiles[i].activeInHierarchy)
            {
                return pooledTallPlatformRightTiles[i];
            }
        }
        return null;
    }


    public GameObject GetPooledForeground(int i)
    {

           if (i % 4 == 0)
            {
                for (int n = 0; n < numberOfForegroundToPool; n++)
                {
                    if (!pooledForeground_2[n].activeInHierarchy)
                    { return pooledForeground_2[n]; }
                }
                   
            }
            if (i % 4 == 1)
            {
                for (int n = 0; n < numberOfForegroundToPool; n++)
                {
                    if (!pooledForeground_3[n].activeInHierarchy)
                    { return pooledForeground_3[n]; }
                }
            }
            if (i % 4 == 2)
            {
                for (int n = 0; n < numberOfForegroundToPool; n++)
                {
                    if (!pooledForeground_4[n].activeInHierarchy)
                    { return pooledForeground_4[n]; }
                }
            }
            if (i % 4 == 3)
            {
                for (int n = 0; n < numberOfForegroundToPool; n++)
                {
                    if (!pooledForeground_1[n].activeInHierarchy)
                    { return pooledForeground_1[n]; }
                }
            }
            
          return null;
    }

    public GameObject GetPooledForegroundBase()
    {
        for (int i =0;  i<numberOfForegourndBaseToPool;i++)
        {
            if(!pooledForegroundBase[i].activeInHierarchy)
            {
                return pooledForegroundBase[i];
            }
        }
        return null;
    }





}

