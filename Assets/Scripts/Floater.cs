using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floater : MonoBehaviour
{
    float y0;

    void Start()
    {
        y0 = transform.position.y;
    }
    // Update is called once per frame
    void Update()
    {
        transform.Rotate(new Vector3(.3f, 0, 0) * Mathf.Sin(1f*Time.time));

        transform.position = new Vector3(transform.position.x, y0+ .1f * Mathf.Sin(1f*Time.time), transform.position.z);
    }
}
