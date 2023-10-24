using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereBS : MonoBehaviour
{
    [SerializeField] float recenterThreshold = 15.0f;

    void Update()
    {
        if (transform.position.x < -recenterThreshold || transform.position.x > recenterThreshold) Recenter();
        if (transform.position.y < -recenterThreshold || transform.position.y > recenterThreshold) Recenter();
        if (transform.position.z < -recenterThreshold || transform.position.z > recenterThreshold) Recenter();
    }

    void Recenter()
    {
        GetComponent<Rigidbody>().isKinematic = true;
        transform.position = new Vector3(-0.2f, 0.42f, 0.23f);
        GetComponent<Rigidbody>().isKinematic = false;
    }
}
