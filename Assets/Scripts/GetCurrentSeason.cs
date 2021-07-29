using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetCurrentSeason : MonoBehaviour
{

    public static GetCurrentSeason SharedInstance;
    public int seasonLength;

    private void Awake()
    {
        SharedInstance = this;
    }


    public string getCurrentSeason()
    {
        var seasonTime = (int)Time.timeSinceLevelLoad / seasonLength % 4;
        if (seasonTime == 0 )
        {
            return "Summer";
        }
        else if (seasonTime == 1 )
        {
            return "Fall";
        }
        else if (seasonTime == 2 )
        {
            return "Winter";
        }
        else if (seasonTime == 3 )
        {
            return "Spring";
        }
        else
        {
            return null;
        }

    }
}
