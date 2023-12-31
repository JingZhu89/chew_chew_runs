using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGroundFollow : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform target;
    public float smoothSpeed = 0.3f;
    public SpriteRenderer summerSprite;
    public SpriteRenderer fallSprite;
    public SpriteRenderer winterSprite;
    public SpriteRenderer springSprite;
    public float fadeSpeed = 0.03f;

    // Update is called once per frame
    void Update()
    {
        if (target.position.x > transform.position.x)
        {
            Vector3 newPos = new Vector3(target.position.x, transform.position.y, transform.position.z);
            transform.position = Vector3.Lerp(transform.position, newPos, smoothSpeed);
        }

        // check season and change background
        var currentSeason = GetCurrentSeason.SharedInstance.getCurrentSeason();
   
        if (currentSeason == "Summer")
        {
            summerSprite.color = new Vector4(1.0f, 1.0f, 1.0f, Mathf.Lerp(summerSprite.color.a, 1.0f, fadeSpeed));
            fallSprite.color = new Vector4(1.0f, 1.0f, 1.0f, Mathf.Lerp(fallSprite.color.a, 0.0f, fadeSpeed));
            winterSprite.color = new Vector4(1.0f, 1.0f, 1.0f, Mathf.Lerp(winterSprite.color.a, 0.0f,fadeSpeed));
            springSprite.color = new Vector4(1.0f, 1.0f, 1.0f, Mathf.Lerp(springSprite.color.a,0.0f,fadeSpeed));
        }

        if (currentSeason=="Fall")
        {
            summerSprite.color = new Vector4(1.0f, 1.0f, 1.0f, Mathf.Lerp(summerSprite.color.a, 0.0f, fadeSpeed));
            fallSprite.color = new Vector4(1.0f, 1.0f, 1.0f, Mathf.Lerp(fallSprite.color.a, 1.0f, fadeSpeed));
            winterSprite.color = new Vector4(1.0f, 1.0f, 1.0f, Mathf.Lerp(winterSprite.color.a, 0.0f, fadeSpeed));
            springSprite.color = new Vector4(1.0f, 1.0f, 1.0f, Mathf.Lerp(springSprite.color.a, 0.0f, fadeSpeed));
        }

        if (currentSeason =="Winter")
        {
            summerSprite.color = new Vector4(1.0f, 1.0f, 1.0f, Mathf.Lerp(summerSprite.color.a, 0.0f, fadeSpeed));
            fallSprite.color = new Vector4(1.0f, 1.0f, 1.0f, Mathf.Lerp(fallSprite.color.a, 0.0f, fadeSpeed));
            winterSprite.color = new Vector4(1.0f, 1.0f, 1.0f, Mathf.Lerp(winterSprite.color.a, 1.0f, fadeSpeed));
            springSprite.color = new Vector4(1.0f, 1.0f, 1.0f, Mathf.Lerp(springSprite.color.a, 0.0f, fadeSpeed));
        }
        
        if (currentSeason =="Spring")
        {

            summerSprite.color = new Vector4(1.0f, 1.0f, 1.0f, Mathf.Lerp(summerSprite.color.a, 0.0f, fadeSpeed));
            fallSprite.color = new Vector4(1.0f, 1.0f, 1.0f, Mathf.Lerp(fallSprite.color.a, 0.0f, fadeSpeed));
            winterSprite.color = new Vector4(1.0f, 1.0f, 1.0f, Mathf.Lerp(winterSprite.color.a, 0.0f, fadeSpeed));
            springSprite.color = new Vector4(1.0f, 1.0f, 1.0f, Mathf.Lerp(springSprite.color.a, 1.0f, fadeSpeed));

        }

        


    }


}
