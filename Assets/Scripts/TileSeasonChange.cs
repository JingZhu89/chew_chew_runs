using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileSeasonChange : MonoBehaviour
{

    public SpriteRenderer summerSprite;
    public SpriteRenderer fallSprite;
    public SpriteRenderer winterSprite;
    public SpriteRenderer springSprite;
    private float fadeSpeed = 0.03f;
    // Start is called before the first frame update

    private void Start()
    {
        summerSprite.color = new Vector4(1.0f, 1.0f, 1.0f, 1.0f);
        fallSprite.color = new Vector4(1.0f, 1.0f, 1.0f, 0.0f);
        winterSprite.color = new Vector4(1.0f, 1.0f, 1.0f, 0.0f);
        springSprite.color = new Vector4(1.0f, 1.0f, 1.0f, 0.0f);
    }


    // Update is called once per frame
    void Update()
    {
        var currentSeason = GetCurrentSeason.SharedInstance.getCurrentSeason();

        if (currentSeason == "Summer")
        {
            summerSprite.color = new Vector4(1.0f, 1.0f, 1.0f, Mathf.Lerp(summerSprite.color.a, 1.0f, fadeSpeed));
            fallSprite.color = new Vector4(1.0f, 1.0f, 1.0f, Mathf.Lerp(fallSprite.color.a, 0.0f, fadeSpeed));
            winterSprite.color = new Vector4(1.0f, 1.0f, 1.0f, Mathf.Lerp(winterSprite.color.a, 0.0f, fadeSpeed));
            springSprite.color = new Vector4(1.0f, 1.0f, 1.0f, Mathf.Lerp(springSprite.color.a, 0.0f, fadeSpeed));
        }

        if (currentSeason == "Fall")
        {
            summerSprite.color = new Vector4(1.0f, 1.0f, 1.0f, Mathf.Lerp(summerSprite.color.a, 0.0f, fadeSpeed));
            fallSprite.color = new Vector4(1.0f, 1.0f, 1.0f, Mathf.Lerp(fallSprite.color.a, 1.0f, fadeSpeed));
            winterSprite.color = new Vector4(1.0f, 1.0f, 1.0f, Mathf.Lerp(winterSprite.color.a, 0.0f, fadeSpeed));
            springSprite.color = new Vector4(1.0f, 1.0f, 1.0f, Mathf.Lerp(springSprite.color.a, 0.0f, fadeSpeed));
        }

        if (currentSeason == "Winter")
        {
            summerSprite.color = new Vector4(1.0f, 1.0f, 1.0f, Mathf.Lerp(summerSprite.color.a, 0.0f, fadeSpeed));
            fallSprite.color = new Vector4(1.0f, 1.0f, 1.0f, Mathf.Lerp(fallSprite.color.a, 0.0f, fadeSpeed));
            winterSprite.color = new Vector4(1.0f, 1.0f, 1.0f, Mathf.Lerp(winterSprite.color.a, 1.0f, fadeSpeed));
            springSprite.color = new Vector4(1.0f, 1.0f, 1.0f, Mathf.Lerp(springSprite.color.a, 0.0f, fadeSpeed));
        }

        if (currentSeason == "Spring")
        {

            summerSprite.color = new Vector4(1.0f, 1.0f, 1.0f, Mathf.Lerp(summerSprite.color.a, 0.0f, fadeSpeed));
            fallSprite.color = new Vector4(1.0f, 1.0f, 1.0f, Mathf.Lerp(fallSprite.color.a, 0.0f, fadeSpeed));
            winterSprite.color = new Vector4(1.0f, 1.0f, 1.0f, Mathf.Lerp(winterSprite.color.a, 0.0f, fadeSpeed));
            springSprite.color = new Vector4(1.0f, 1.0f, 1.0f, Mathf.Lerp(springSprite.color.a, 1.0f, fadeSpeed));

        }

    }
}
