using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacles : MonoBehaviour
{
    private GameObject First;
    private GameObject Twins;
    private GameObject BigLug;
    private GameObject Masher;
    private GameObject Crasher;
    private GameObject Porter;
    private GameObject Speedy;

    private bool moveBackF = false;
    private bool moveBack = false;
    private bool drop = false;
    private bool dropCrash = true;
    private bool mash = true;
    private int impact = 0;
    private int impactCrash = 0;
    private int impactMash = 0;

    // Start is called before the first frame update
    void Start()
    {
        First = transform.GetChild(0).gameObject;
        Twins = transform.GetChild(1).gameObject;
        BigLug = transform.GetChild(2).gameObject;
        Masher = transform.GetChild(3).gameObject;
        Crasher = transform.GetChild(4).gameObject;
        Porter = transform.GetChild(5).gameObject;
        Speedy = transform.GetChild(6).gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (moveBackF)
        {
            First.transform.position += new Vector3(20 * Time.deltaTime * TimeScale.global, 0, 0);
        }
        else
        {
            First.transform.position -= new Vector3(20 * Time.deltaTime * TimeScale.global, 0, 0);
        }

        if (First.transform.position.x < -26)
        {
            moveBackF = true;
        }
        else if (First.transform.position.x > 25)
        {
            moveBackF = false;
        }

        if (moveBack)
        {
            Twins.transform.position += new Vector3(30 * Time.deltaTime * TimeScale.global, 0, 0);
        }
        else
        {
            Twins.transform.position -= new Vector3(30 * Time.deltaTime * TimeScale.global, 0, 0);
        }

        if (Twins.transform.position.x < -49)
        {
            moveBack = true;
        }
        else if (Twins.transform.position.x > 26)
        {
            moveBack = false;
        }

        if (drop)
        {
            if (BigLug.transform.position.y > 3.1)
                BigLug.transform.position -= new Vector3(0, 60 * Time.deltaTime * TimeScale.global, 0);
            else
            {
                BigLug.transform.position = new Vector3(BigLug.transform.position.x, 3.1f, BigLug.transform.position.z);
                impact++;
                if (impact == 12)
                {
                    drop = false;
                    impact = 0;
                }
            }
        }
        else
        {
            BigLug.transform.position += new Vector3(0, 30 * Time.deltaTime * TimeScale.global, 0);
            if (BigLug.transform.position.y > 35)
            {
                drop = true;
            }
        }

        if (dropCrash)
        {
            if (Crasher.transform.position.y > 6.3f)
                Crasher.transform.position -= new Vector3(0, 100 * Time.deltaTime * TimeScale.global, 0);
            else
            {
                Crasher.transform.position = new Vector3(Crasher.transform.position.x, 6.25f, Crasher.transform.position.z);
                impactCrash++;
                if (impactCrash == 24)
                {
                    dropCrash = false;
                    impactCrash = 0;
                }
            }
        }
        else
        {
            Crasher.transform.position += new Vector3(0, 35 * Time.deltaTime * TimeScale.global, 0);
            if (Crasher.transform.position.y > 39)
            {
                dropCrash = true;
            }
        }

        if (mash)
        {
            if (Masher.transform.position.x < -5.7f)
                Masher.transform.position += new Vector3(80 * Time.deltaTime * TimeScale.global, 0, 0);
            else
            {
                Masher.transform.position = new Vector3(-5.7f, Masher.transform.position.y, Masher.transform.position.z);
                impactMash++;
                if (impactMash == 24)
                {
                    mash = false;
                    impactMash = 0;
                }
            }
        }
        else
        {
            Masher.transform.position -= new Vector3(30 * Time.deltaTime * TimeScale.global, 0, 0);
            if (Masher.transform.position.x < -34 || (GameObject.Find("unitychan").transform.position.z > 126.8f && GameObject.Find("unitychan").transform.position.z < 131.4f))
            {
                mash = true;
            }
        }

        Porter.transform.position -= new Vector3(20 * Time.deltaTime * TimeScale.global, 0, 0);
        if (Porter.transform.position.x <= -29)
        {
            Porter.transform.position = new Vector3(25, Porter.transform.position.y, Porter.transform.position.z);
        }

        Speedy.transform.position += new Vector3(110 * Time.deltaTime * TimeScale.global, 0, 0);
        if (Speedy.transform.position.x >= 26)
        {
            Speedy.transform.position = new Vector3(-28, Speedy.transform.position.y, Speedy.transform.position.z);
        }
    }
}
