using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetInactive : MonoBehaviour
{

    public void OnBecameInvisible()
    {

        gameObject.transform.parent.gameObject.SetActive(false);
    }
   
}
