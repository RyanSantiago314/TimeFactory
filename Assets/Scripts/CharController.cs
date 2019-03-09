using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharController : MonoBehaviour
{
    public Canvas inGameUI;
    private Animator anim;
    public AudioSource jump;
    public AudioSource run;
    public AudioSource rest;
    public AudioSource idle;
    public AudioSource starting;
    public AudioSource hooray;
    public AudioSource laugh;
    public AudioSource nuuu;
    public AudioSource hello;
    public AudioSource trip;
    public AudioSource slide;

    public float openSpeed = 4f;
    public GameObject HallDoor;
    public GameObject SecondDoor;
    public GameObject BigDoorLeft;
    public GameObject BigDoorRight;
    public GameObject Pedestal;
    public GameObject KeyCard;
    public GameObject Watch;

    public ParticleSystem sparkly;
    public ParticleSystem important;
    public ParticleSystem turnOn;

    public GameObject CopyChan;

    TimeController script;

    public Light watchSpot;

    private bool HallDoorOpen = false;
    private bool SecondDoorOpen = false;
    private bool BigDoorOpen = true;
    private bool SwitchedOn = false;

    public Text subtitle;
    public int textTimer;
    public int killCount = 0;

    bool GotCard = false;


    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        script = GetComponent<TimeController>();
        inGameUI.enabled = false;
        transform.GetChild(6).gameObject.SetActive(false);
        transform.GetChild(7).gameObject.SetActive(false);
        hello.PlayDelayed(1.5f);
        run.volume = 0f;
        subtitle.text = "";
        textTimer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if(anim.GetFloat("Speed") == 1f && !jump.isPlaying && !GetComponent<UnityChan.UnityChanControlScriptWithRgidBody>().attacking)
            run.volume += 0.1f;
        else
            run.volume -= 0.1f;

        if (anim.GetBool("Damage") || anim.GetBool("Poke"))
            trip.Play();
        else if (anim.GetBool("Jump"))
            jump.Play();
        else if (anim.GetBool("IdleAnim"))
            idle.PlayDelayed(.5f);
        else if (anim.GetBool("Rest"))
            rest.PlayDelayed(2f);
        else if (anim.GetBool("Attack"))
            slide.PlayDelayed(.1f);

        if (HallDoorOpen && HallDoor.transform.position.y < 10)
        {
            HallDoor.transform.position += new Vector3(0, openSpeed * Time.deltaTime * TimeScale.global, 0);
        }
        else if (HallDoorOpen){}
        else if (!HallDoorOpen && HallDoor.transform.position.y > 3.5)
        {
            HallDoor.transform.position -= new Vector3(0, openSpeed/2 * Time.deltaTime * TimeScale.global, 0);
        }

        if (SecondDoorOpen && SecondDoor.transform.position.y < 10)
        {
            SecondDoor.transform.position += new Vector3(0, openSpeed * Time.deltaTime * TimeScale.global, 0);
        }
        else if (SecondDoorOpen){}
        else if (!SecondDoorOpen && SecondDoor.transform.position.y > 3.5)
        {
            SecondDoor.transform.position -= new Vector3(0, openSpeed/2 * Time.deltaTime * TimeScale.global, 0);
        }
        if (BigDoorOpen && BigDoorLeft.transform.position.x > -11.8)
        {
            BigDoorLeft.transform.position -= new Vector3(openSpeed * Time.deltaTime * TimeScale.global, 0, 0);
            BigDoorRight.transform.position += new Vector3(openSpeed * Time.deltaTime * TimeScale.global, 0, 0);
        }
        else if (!BigDoorOpen && BigDoorLeft.transform.position.x < -4.37)
        {
            BigDoorLeft.transform.position += new Vector3(openSpeed * Time.deltaTime * TimeScale.global, 0, 0);
            BigDoorRight.transform.position -= new Vector3(openSpeed * Time.deltaTime * TimeScale.global, 0, 0);
        }
        if (killCount >= 5 && Pedestal.transform.position.y < 4.42)
        {
            Pedestal.transform.position += new Vector3(0, openSpeed * Time.deltaTime * TimeScale.global, 0);
        }
        else if (Pedestal.transform.position.y > .42)
        {
            Pedestal.transform.position -= new Vector3(0, openSpeed * Time.deltaTime * TimeScale.global, 0);
        }

        if (subtitle.text != "" && textTimer < 360)
            textTimer++;
        if (textTimer == 360)
        {
            subtitle.text = "";
            textTimer = 0;
        }
        if (subtitle.text == "Access granted. Welcome, Unreal-kun!" && textTimer > 180)
        {
            subtitle.text = "Of course... -.-";
        }
        else if (subtitle.text == "This appears to be some kind of card reader terminal..." && textTimer > 180)
        {
            subtitle.text = "I WONDER WHAT THIS COULD BE FOR....? HMMMMM...";
        }
        else if (subtitle.text == "Only authorized personnel may enter. Please present your keycard." && textTimer > 180)
        {
            if (GotCard)
            {
                subtitle.text = "Gotta put this keycard into the terminal.";
            }
            else
            {
                subtitle.text = "Maybe someone left their keycard lying around...";
            }
            
        }
        else if (subtitle.text == "What the-?! What's happening?!!" && textTimer > 180)
        {
            subtitle.text = "INTRUDER ALERT. INITIATING SECURITY PROTOCOL.";
        }
        else if (subtitle.text == "INTRUDER ALERT. INITIATING SECURITY PROTOCOL." && textTimer > 300)
        {
            trip.Play();
            subtitle.text = "Wha... WHAT ARE THESE THINGS?";
            textTimer = 180;
        }
        else if (subtitle.text == "The watch seems to be resonating with this device..." && textTimer > 180)
        {
            subtitle.text = "And turning on that door...";
        }
    }

    void OnTriggerEnter(Collider other) 
    {
        if(other.gameObject.CompareTag("Card"))
        {
            laugh.Play();
            GotCard = true;
            sparkly.Play();
            important.Play();
            KeyCard.SetActive(false);
            transform.GetChild(6).gameObject.SetActive(true);
            subtitle.text = "Sweet! A keycard! This'll let me open that door over there!";
        }
        else if (other.gameObject.CompareTag("Hall"))
        {
            HallDoorOpen = true;
        }
        else if (other.gameObject.CompareTag("Terminal"))
        {
            if (GotCard)
            {
                turnOn.Play();
                hooray.Play();
                transform.GetChild(6).gameObject.SetActive(false);
                SwitchedOn = true;
                subtitle.text = "Access granted. Welcome, Unreal-kun!";
                textTimer = 0;
            }
            else
            {
                subtitle.text = "This appears to be some kind of card reader terminal...";
                textTimer = 0;  
            }
        }
        else if (other.gameObject.CompareTag("Room2"))
        {
            if (SwitchedOn)
                SecondDoorOpen = true;
            else if (script.gotWatch)
            {
                nuuu.Play();
                subtitle.text = "Must've been locked when I tripped the alarm...";
                textTimer = 0;
            }
            else
            {
                nuuu.Play();
                subtitle.text = "Only authorized personnel may enter. Please present your keycard.";
                textTimer = 0;
            }
        }
        else if (other.gameObject.CompareTag("Watch"))
        {
            sparkly.transform.position = new Vector3(.064f, 1.65f, 57.595f);
            SwitchedOn = false;
            BigDoorOpen = false;
            Watch.SetActive(false);
            trip.Play();
            sparkly.Play();
            subtitle.text = "What the-?! What's happening?!!";
            textTimer = 0;
            watchSpot.spotAngle = 150;
            watchSpot.color = Color.red;

            for (int i = 0 ; i < 5; ++i)
            {
                switch(i)
                {
                    case 0:
                        Instantiate(CopyChan, new Vector3(0, 9, 63), Quaternion.identity);
                        break;
                    case 1:
                        Instantiate(CopyChan, new Vector3(6, 11, 61), Quaternion.identity);
                        break;
                    case 2:
                        Instantiate(CopyChan, new Vector3(-6, 11, 61), Quaternion.identity);
                        break;
                    case 3:
                        Instantiate(CopyChan, new Vector3(8, 12, 58.5f), Quaternion.identity);
                        break;
                    case 4:
                        Instantiate(CopyChan, new Vector3(-8, 12, 58.5f), Quaternion.identity);
                        break;
                    default:
                        break;
                }
            }

        }
        else if (other.gameObject.CompareTag("Terminal2"))
        {
            BigDoorOpen = true;
            subtitle.text = "The watch seems to be resonating with this device...";
            laugh.Play();
            textTimer = 0;
        }
    }

    void OnTriggerExit(Collider other) 
    {
        if (other.gameObject.CompareTag("Hall"))
        {
            HallDoorOpen = false;
        }
        else if (other.gameObject.CompareTag("Room2"))
        {
            SecondDoorOpen = false;
        }
        else if (other.gameObject.CompareTag("Terminal2"))
        {
            BigDoorOpen = false;
        }
    }

    public void openGame()
    {
        starting.PlayDelayed(.3f);
        inGameUI.enabled = true;
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void Restart()
    {
        var enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (var enemy in enemies)
        {
            Destroy(enemy);
        }
        script.health = 400;
        anim.speed = 1;
        SwitchedOn = false;
        turnOn.Stop();
        GotCard = false;
        script.timeSlow = false;
        BigDoorOpen = true;
        KeyCard.SetActive(true);
        Watch.SetActive(true);
        watchSpot.spotAngle = 50;
        killCount = 0;
        watchSpot.color = new Color(114, 255, 255, 1);
        transform.position = Vector3.zero;
        transform.rotation = Quaternion.identity;
    }

}
