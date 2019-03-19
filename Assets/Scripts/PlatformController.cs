using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformController : MonoBehaviour
{
    public GameObject first;
    public GameObject second;
    public GameObject third;
    public GameObject fourth;
    public GameObject fifth;
    public GameObject floater;
    public GameObject floater1;
    public GameObject floater2;
    public GameObject axle1;
    public GameObject axle2;
    public GameObject skater1;
    public GameObject skater2;
    public GameObject skater3;
    public GameObject skater4;
    public GameObject skater5;
    public GameObject skater6;


    float fy0;
    float f1y0;
    private float rotateSpeed;
    private float timePassing = 0;
    // Start is called before the first frame update
    void Start()
    {
        fy0 = floater.transform.position.y;
        f1y0 = floater1.transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        rotateSpeed = 90 * Time.deltaTime * TimeScale.global;
        timePassing += Time.deltaTime * TimeScale.global;
        first.transform.position = new Vector3(first.transform.position.x, -5.275f + 4.275f * Mathf.Sin(.75f * timePassing), first.transform.position.z);
        second.transform.position = new Vector3(14 + 13f * Mathf.Sin(.75f * timePassing), second.transform.position.y, second.transform.position.z);
        third.transform.position = new Vector3(third.transform.position.x, third.transform.position.y, 434 + 15f * Mathf.Sin(-.75f * timePassing));
        fourth.transform.position = new Vector3(fourth.transform.position.x, fourth.transform.position.y, 441.5f + 22.5f * Mathf.Sin(.5f * timePassing));
        fifth.transform.position = new Vector3(fifth.transform.position.x, fifth.transform.position.y, 441.5f + 22.5f * Mathf.Sin(-.5f * timePassing));

        floater.transform.position = new Vector3(floater.transform.position.x, fy0 + .2f * Mathf.Sin(1f * timePassing), floater.transform.position.z);
        floater1.transform.position = new Vector3(floater1.transform.position.x, f1y0 + .2f * Mathf.Sin(1f * timePassing), floater1.transform.position.z);
        floater2.transform.position = new Vector3(floater2.transform.position.x, 1 + .2f * Mathf.Sin(1f * timePassing), floater2.transform.position.z);
        axle2.transform.position = new Vector3(axle2.transform.position.x, 7 + 8 * Mathf.Sin(1f * timePassing), axle2.transform.position.z);

        skater1.transform.position = new Vector3(0 + 17.5f * Mathf.Sin(1 * timePassing), skater1.transform.position.y, skater1.transform.position.z);
        skater2.transform.position = new Vector3(0 + 17.5f * Mathf.Sin(-1 * timePassing), skater2.transform.position.y, skater2.transform.position.z);
        skater3.transform.position = new Vector3(0 + 17.5f * Mathf.Sin(1 * timePassing), skater3.transform.position.y, skater3.transform.position.z);
        skater4.transform.position = new Vector3(0 + 17.5f * Mathf.Sin(-1 * timePassing), skater4.transform.position.y, skater4.transform.position.z);
        skater5.transform.position = new Vector3(0 + 17.5f * Mathf.Sin(1 * timePassing), skater5.transform.position.y, skater5.transform.position.z);
        skater6.transform.position = new Vector3(0 + 17.5f * Mathf.Sin(-1 * timePassing), skater6.transform.position.y, skater6.transform.position.z);

        //axle1.transform.Rotate(0, -rotateSpeed, 0);
        //axle2.transform.Rotate(0, rotateSpeed, 0);
    }
}
