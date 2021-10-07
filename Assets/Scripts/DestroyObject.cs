using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyObject : MonoBehaviour
{
    public void OnBecameInvisible()
    {
        if (gameObject.name.Contains("Wings"))
        {
            FindObjectOfType<AudioManager>().StopSound("Propeller");
        }
        Destroy(gameObject);

    }

    public void OnBecameVisible()
    {
        if (gameObject.name.Contains("Wings"))
        {
            FindObjectOfType<AudioManager>().PlaySound("Propeller");
        }

    }


}

