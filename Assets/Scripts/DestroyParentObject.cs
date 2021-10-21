using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyParentObject : MonoBehaviour
{
    public void OnBecameInvisible()
    {

        Destroy(transform.parent.gameObject);
    }

}
