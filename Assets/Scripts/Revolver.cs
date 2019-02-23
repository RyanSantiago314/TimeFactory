using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Revolver : MonoBehaviour
{
    public GameObject target;

    // Update is called once per frame
    void Update()
    {

        transform.Rotate(new Vector3(180, 210, 120) * Time.deltaTime);

        transform.RotateAround(target.transform.position, Vector3.up, 120 * Time.deltaTime);

        transform.position = new Vector3(transform.position.x, target.transform.position.y + 1 + .3f * Mathf.Sin(1f*Time.time), transform.position.z);
        
    }
}
