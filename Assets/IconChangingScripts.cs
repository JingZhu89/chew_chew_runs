using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class IconChangingScripts : MonoBehaviour
{

    public SpriteRenderer bluestarIcon;
    public SpriteRenderer pillbugIcon;
    public SpriteRenderer flyIcon;
    public Light2D powerupLight;
    public SpriteRenderer topbarRight;


    void Start()
    {
        bluestarIcon.color = new Vector4(1.0f, 1.0f, 1.0f, 0.0f);
        pillbugIcon.color = new Vector4(1.0f, 1.0f, 1.0f, 0.0f);
        flyIcon.color = new Vector4(1.0f, 1.0f, 1.0f, 0.0f);
        topbarRight.color = new Vector4(1.0f, 1.0f, 1.0f, 0.0f);
        powerupLight.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerMovement.SharedInstance.freeze == true)
        {
            topbarRight.color = new Vector4(1.0f, 1.0f, 1.0f, 1.0f);
            bluestarIcon.color = new Vector4(1.0f, 1.0f, 1.0f, 1.0f);
            powerupLight.enabled = true;
        }
        else if (PlayerMovement.SharedInstance.crashThroughEverything == true)
        {
            topbarRight.color = new Vector4(1.0f, 1.0f, 1.0f, 1.0f);
            pillbugIcon.color = new Vector4(1.0f, 1.0f, 1.0f, 1.0f);
            powerupLight.enabled = true;
        }
        else if (PlayerMovement.SharedInstance.flyingMode == true)
        {
            topbarRight.color = new Vector4(1.0f, 1.0f, 1.0f, 1.0f);
            flyIcon.color = new Vector4(1.0f, 1.0f, 1.0f, 1.0f);
            powerupLight.enabled = true;
        }
        else
        {
            topbarRight.color = new Vector4(1.0f, 1.0f, 1.0f, 0.0f);
            bluestarIcon.color = new Vector4(1.0f, 1.0f, 1.0f, 0.0f);
            pillbugIcon.color = new Vector4(1.0f, 1.0f, 1.0f, 0.0f);
            flyIcon.color = new Vector4(1.0f, 1.0f, 1.0f, 0.0f);
            powerupLight.enabled = false;

        }


    }
}
