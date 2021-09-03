using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetInactive : MonoBehaviour
{
    private bool objectSeenByPlayer = false;
    public void OnBecameVisible()
    {
        objectSeenByPlayer = true;
        print(gameObject.transform.parent.name + " seen by player");
    }

    public void OnBecameInvisible()
    {
        if (objectSeenByPlayer==true)
        {
            print(gameObject.transform.parent.name + " deactivated for going off screen");
            gameObject.transform.parent.gameObject.SetActive(false);

        }

    }
}
