using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeController : MonoBehaviour
{
    public CanvasGroup screenFlash;
    public bool timeSlow = false;

    private bool flash;
    private bool gotWatch = false;
    public Animator anim;
    public AudioSource jump;
    public AudioSource run;
    public AudioSource rest;
    public AudioSource idle;
    public AudioSource damage;
    public AudioSource slide;

    public AudioSource timeSlows;
    public AudioSource timeResumes;

    public ParticleSystem watchPowers;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire2") && gotWatch)
        {
            timeSlow = !timeSlow;
            flash = true;
            screenFlash.alpha = 1;
            if (timeSlow)
            {
                timeSlows.Play();
                watchPowers.Play();
            }
            else
            {
                timeResumes.Play();
                watchPowers.Stop();
            }
        }

        if (flash)
        {
            screenFlash.alpha -= Time.deltaTime;
            if (screenFlash.alpha <= 0)
            {
                screenFlash.alpha = 0;
                flash = false;
            }
        }

        if (timeSlow)
        {
            if(anim.GetBool("Attack"))
            {
                TimeScale.player = 2.8f;
            }
            else
            {
                TimeScale.player = .7f;
            }
            TimeScale.global = .4f;
            anim.speed = .75f;
            jump.pitch = .6f;
            run.pitch = .6f;
            rest.pitch = .6f;
            idle.pitch = .6f;
            damage.pitch = .6f;
            slide.pitch = .6f;
        }
        else if (anim.GetBool("Attack"))
        {
            TimeScale.player = 4;
        }
        else
        {
            TimeScale.player = 1f;
            TimeScale.global = 1f;
            anim.speed = 1.5f;
            jump.pitch = 1f;
            run.pitch = 1f;
            rest.pitch = 1f;
            idle.pitch = 1f;
            damage.pitch = 1f;
            slide.pitch = 1f;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Watch"))
        {
            gotWatch = true;
            other.gameObject.SetActive(false);
            transform.GetChild(7).gameObject.SetActive(true);
        }
    }
}
