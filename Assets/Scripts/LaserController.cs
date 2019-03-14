using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserController : MonoBehaviour
{
    public GameObject Laser11;
    public GameObject Laser12;
    public GameObject Laser21;
    public GameObject Laser22;
    public GameObject Laser23;
    public GameObject Laser24;
    public GameObject Array3;
    public GameObject Array4;
    public GameObject Laser51;
    public GameObject Laser52;
    public GameObject Array6;
    public GameObject Array7;
    public GameObject Array8;
    public GameObject Array9;
    public GameObject Array10;
    public GameObject FinalLaser;

    private float laserSpeed;
    private float laserVSpeed;
    private float rotateSpeed;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        rotateSpeed = 60 * Time.deltaTime * TimeScale.global;
        //Array1
        if (Laser11.transform.position.x <= -24)
        {
            laserSpeed = 95 * Time.deltaTime * TimeScale.global;
        }
        else if (Laser11.transform.position.x >= 24)
        {
            laserSpeed = -95 * Time.deltaTime * TimeScale.global;
        }
        Laser11.transform.position += new Vector3(laserSpeed, 0, 0);
        Laser12.transform.position -= new Vector3(laserSpeed, 0, 0);
        //Array2
        if (Laser23.transform.position.y <= -5)
        {
            laserVSpeed = 45 * Time.deltaTime * TimeScale.global;
        }
        else if (Laser23.transform.position.y >= 12.25)
        {
            laserVSpeed = -45 * Time.deltaTime * TimeScale.global;
        }

        Laser21.transform.position += new Vector3(laserSpeed, 0, 0);
        Laser22.transform.position -= new Vector3(laserSpeed, 0, 0);
        Laser23.transform.position += new Vector3(0, laserVSpeed, 0);
        Laser24.transform.position -= new Vector3(0, laserVSpeed, 0);

        //Array3
        Array3.transform.Rotate(0, rotateSpeed, 0);

        //Array4
        Array4.transform.Rotate(0, -rotateSpeed, 0);

        //Array5
        Laser51.transform.position += new Vector3(laserSpeed, 0, 0);
        Laser52.transform.position -= new Vector3(laserSpeed, 0, 0);

        //Array6
        Array6.transform.Rotate(0, -rotateSpeed, 0);

        //Arrays 7-10
        Array7.transform.Rotate(0, rotateSpeed, 0);
        Array8.transform.Rotate(0, -rotateSpeed, 0);
        Array9.transform.Rotate(0, -rotateSpeed, 0);
        Array10.transform.Rotate(0, rotateSpeed, 0);
        FinalLaser.transform.position += new Vector3(0, laserVSpeed, 0);
    }
}
