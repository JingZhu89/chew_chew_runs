using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour

{

    public Transform target;
    public float smoothSpeed = 0.3f;

    // Start is called before the first frame update


    // Update is called once per frame
    void Update()
    {
        if(target.position.x>transform.position.x)
        {
            Vector3 newPos = new Vector3(target.position.x, transform.position.y, transform.position.z);
            transform.position = Vector3.Lerp(transform.position, newPos, smoothSpeed );
        }

    }
}
