using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeController : MonoBehaviour
{
    public CanvasGroup screenFlash;
    public RectTransform healthBar;
    public RectTransform energyBar;

    public bool timeSlow = false;

    private bool flash;
    private bool gotWatch = false;
    private bool recharge = false;
    public Animator anim;
    public AudioSource jump;
    public AudioSource run;
    public AudioSource rest;
    public AudioSource idle;
    public AudioSource damage;
    public AudioSource slide;

    public AudioSource timeSlows;
    public AudioSource timeResumes;
    public AudioSource slowedTime;

    public ParticleSystem watchPowers;

    float healthBarWidth;
    float eBarWidth;
    float health = 400;
    float energy = 400;

    // Start is called before the first frame update
    void Start()
    {
        slowedTime.volume = 0;
        healthBarWidth = healthBar.sizeDelta.x;
        eBarWidth = energyBar.sizeDelta.x;
    }

    // Update is called once per frame
    void Update()
    {
        if (energy == 0)
        {
            recharge = true;
            timeSlow = false;
            flash = true;
            screenFlash.alpha = 1;
            timeResumes.Play();
        }
        if (energy >= 400)
        {
            energy = 400;
            recharge = false;
        }

        if (Input.GetButtonDown("Fire2") && gotWatch && !recharge)
        {
            timeSlow = !timeSlow;
            flash = true;
            screenFlash.alpha = 1;
            watchPowers.Play();
            if (timeSlow)
            {
                timeSlows.Play();
            }
            else
            {
                timeResumes.Play();
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
            energy -= 1;
            slowedTime.volume = 1;
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
            slowedTime.volume = 0;
            if (recharge)
                energy += .5f;
            else
                energy += 2;

            if (energy == 0)
            {
                recharge = false;
            }
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

        energyBar.sizeDelta = new Vector2(energy, healthBar.sizeDelta.y);
        healthBar.sizeDelta = new Vector2(health, healthBar.sizeDelta.y);
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
