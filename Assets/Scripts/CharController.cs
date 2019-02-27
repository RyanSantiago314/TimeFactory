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
    public GameObject KeyCard;

    public ParticleSystem sparkly;
    public ParticleSystem important;
    public ParticleSystem turnOn;

    private bool HallDoorOpen = false;
    private bool SecondDoorOpen = false;
    private bool SwitchedOn = false;

    public Text subtitle;
    int textTimer;

    bool GotCard = false;


    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        //inGameUI.enabled = false;
        transform.GetChild(6).gameObject.SetActive(false);
        transform.GetChild(7).gameObject.SetActive(false);
        hello.PlayDelayed(2.2f);
        run.volume = 0f;
        subtitle.text = "";
        textTimer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if(anim.GetFloat("Speed") == 1f && !jump.isPlaying)
            run.volume += 0.1f;
        else
            run.volume -= 0.1f;

        if (anim.GetBool("Damage"))
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
    }

    void OnTriggerEnter(Collider other) 
    {
        if(other.gameObject.CompareTag("Card"))
        {
            laugh.Play();
            GotCard = true;
            sparkly.Play();
            important.Play();
            other.gameObject.SetActive(false);
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
            trip.Play();
            sparkly.Play();
            subtitle.text = "What the-?! What's happening?!!";
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

    void Restart()
    {
        SwitchedOn = false;
        turnOn.Stop();
        GotCard = false;
        KeyCard.SetActive(true);
        transform.position = Vector3.zero;
        transform.rotation = Quaternion.identity;
    }

}
