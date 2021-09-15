using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetInactive : MonoBehaviour
{
    private bool objectSeenByPlayer = false;
    public void OnBecameVisible()
    {
        objectSeenByPlayer = true;
    }

    public void OnBecameInvisible()
    {
        if (objectSeenByPlayer==true)
        {
            gameObject.transform.parent.gameObject.SetActive(false);

        }

    }
}
