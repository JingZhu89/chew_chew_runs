using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayCountDownSound : MonoBehaviour
{
    // Start is called before the first frame update
    public void PlayCoundDownSound()
    {
        FindObjectOfType<AudioManager>().PlaySound("CountDown");
    }

}
