using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GetCollectable : MonoBehaviour
{

    public static GetCollectable SharedInstance;
    public List<CollectableProbability> collectables;

    private void Awake()
    {
        SharedInstance = this;

    }

    public Collectable getCollectable()
    {
        int runningTotal = 0;
        int sum = collectables.Select(p => p.probability).Sum();
        int rand = Random.Range(0, sum);

        foreach (var collectable in collectables)
        {
            runningTotal = runningTotal + collectable.probability;
            if(rand<runningTotal)
            {
                return collectable.collectableObject;
            }
        }
        return null;    
    }




}
