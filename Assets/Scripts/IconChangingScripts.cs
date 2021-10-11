using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class IconChangingScripts : MonoBehaviour
{

    public SpriteRenderer slowDownIcon;
    public SpriteRenderer crashthroughIcon;
    public SpriteRenderer flyIcon;
    public SpriteRenderer rocketIcon;
    public Light2D powerupLight;
    public SpriteRenderer topbarRight;


    void Start()
    {
        slowDownIcon.color = new Vector4(1.0f, 1.0f, 1.0f, 0.0f);
        crashthroughIcon.color = new Vector4(1.0f, 1.0f, 1.0f, 0.0f);
        flyIcon.color = new Vector4(1.0f, 1.0f, 1.0f, 0.0f);
        rocketIcon.color = new Vector4(1.0f, 1.0f, 1.0f, 0.0f);
        topbarRight.color = new Vector4(1.0f, 1.0f, 1.0f, 0.0f);
        powerupLight.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerMovement.SharedInstance.freeze == true)
        {
            topbarRight.color = new Vector4(1.0f, 1.0f, 1.0f, 1.0f);
            slowDownIcon.color = new Vector4(1.0f, 1.0f, 1.0f, 1.0f);
            powerupLight.enabled = true;
            crashthroughIcon.color = new Vector4(1.0f, 1.0f, 1.0f, 0.0f);
            rocketIcon.color = new Vector4(1.0f, 1.0f, 1.0f, 0.0f);
            flyIcon.color = new Vector4(1.0f, 1.0f, 1.0f, 0.0f);

        }
        else if (PlayerMovement.SharedInstance.crashThroughEverything == true)
        {
            topbarRight.color = new Vector4(1.0f, 1.0f, 1.0f, 1.0f);
            crashthroughIcon.color = new Vector4(1.0f, 1.0f, 1.0f, 1.0f);
            powerupLight.enabled = true;
            slowDownIcon.color = new Vector4(1.0f, 1.0f, 1.0f, 0.0f);
            rocketIcon.color = new Vector4(1.0f, 1.0f, 1.0f, 0.0f);
            flyIcon.color = new Vector4(1.0f, 1.0f, 1.0f, 0.0f);
        }
        else if (PlayerMovement.SharedInstance.flyingMode == true)
        {
            topbarRight.color = new Vector4(1.0f, 1.0f, 1.0f, 1.0f);
            flyIcon.color = new Vector4(1.0f, 1.0f, 1.0f, 1.0f);
            slowDownIcon.color = new Vector4(1.0f, 1.0f, 1.0f, 0.0f);
            crashthroughIcon.color = new Vector4(1.0f, 1.0f, 1.0f, 0.0f);
            rocketIcon.color = new Vector4(1.0f, 1.0f, 1.0f, 0.0f);
            powerupLight.enabled = true;
        }

        else if (PlayerMovement.SharedInstance.rocketMode == true)
        {
            topbarRight.color = new Vector4(1.0f, 1.0f, 1.0f, 1.0f);
            flyIcon.color = new Vector4(1.0f, 1.0f, 1.0f, 0.0f);
            slowDownIcon.color = new Vector4(1.0f, 1.0f, 1.0f, 0.0f);
            crashthroughIcon.color = new Vector4(1.0f, 1.0f, 1.0f, 0.0f);
            rocketIcon.color = new Vector4(1.0f, 1.0f, 1.0f, 1.0f);
            powerupLight.enabled = true;
        }


        else


        {
            topbarRight.color = new Vector4(1.0f, 1.0f, 1.0f, 0.0f);
            slowDownIcon.color = new Vector4(1.0f, 1.0f, 1.0f, 0.0f);
            crashthroughIcon.color = new Vector4(1.0f, 1.0f, 1.0f, 0.0f);
            flyIcon.color = new Vector4(1.0f, 1.0f, 1.0f, 0.0f);
            rocketIcon.color = new Vector4(1.0f, 1.0f, 1.0f, 0.0f);
            powerupLight.enabled = false;

        }


    }
}
