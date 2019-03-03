using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeController : MonoBehaviour
{
    public CanvasGroup screenFlash;
    public Canvas InGameUI;
    public GameObject GameOverScreen;
    public RectTransform healthBar;
    public RectTransform healthFeedback;
    public RectTransform energyBar;
    public RawImage energyGraphic;

    public bool timeSlow = false;

    private bool flash;
    public bool gotWatch = false;
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
    public ParticleSystem sparkly;

    float healthBarWidth;
    float eBarWidth;
    public float health = 400;
    float energy = 400;
    float ghosthealth = 400;

    // Start is called before the first frame update
    void Start()
    {
        slowedTime.volume = 0;
        healthBarWidth = healthBar.sizeDelta.x;
        eBarWidth = energyBar.sizeDelta.x;
        anim.speed = 1.5f;
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
        if (ghosthealth > health)
        {
            ghosthealth -= 1;
        }
        else if (ghosthealth < health)
        {
            ghosthealth = health;
        }
        if (recharge)
        {
            energyGraphic.color = new Vector4(149, 0, 154, .7f);
        }
        else
            energyGraphic.color = Color.white;

        GameOverScreen.SetActive(Time.timeScale == 0 && health <= 0);

        //gameover when health hits zero
        if (health <= 0)
        {
            GameOver();
        }
        else
        {
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
                if (anim.GetCurrentAnimatorStateInfo(0).fullPathHash == Animator.StringToHash("Base Layer.Slide"))
                {
                    TimeScale.player = 1.7f;
                }
                else
                {
                    TimeScale.player = .8f;
                }
                TimeScale.global = .3f;
                TimeScale.enemy = .3f;
                slide.pitch = .6f;
                slide.pitch = .6f;
                jump.pitch = .6f;
                run.pitch = .6f;
                rest.pitch = .6f;
                idle.pitch = .6f;
                damage.pitch = .6f;
                anim.speed = .75f;

            }
            else if (anim.GetCurrentAnimatorStateInfo(0).fullPathHash == Animator.StringToHash("Base Layer.Slide"))
            {
                TimeScale.player = 2.5f;
                slide.pitch = 1;
                anim.speed = 1.5f;
            }
            else
            {
                slowedTime.volume = 0;
                if (recharge)
                {
                    energy += .5f;
                }
                else
                    energy += 2;

                if (energy == 0)
                {
                    recharge = false;
                }
                TimeScale.player = 1f;
                TimeScale.global = 1f;
                TimeScale.enemy = 1f;
                slide.pitch = 1;
                jump.pitch = 1;
                run.pitch = 1;
                rest.pitch = 1;
                idle.pitch = 1;
                damage.pitch = 1;
            }
        }       

        

        energyBar.sizeDelta = new Vector2(energy, healthBar.sizeDelta.y);
        healthBar.sizeDelta = new Vector2(health, healthBar.sizeDelta.y);
        healthFeedback.sizeDelta = new Vector2(ghosthealth, healthFeedback.sizeDelta.y);
    }


    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Watch"))
        {
            gotWatch = true;
            other.gameObject.SetActive(false);
            transform.GetChild(7).gameObject.SetActive(true);
        }
        if (other.gameObject.CompareTag("Enemy"))
        {
            if ((timeSlow && GetComponent<UnityChan.UnityChanControlScriptWithRgidBody>().attacking) ||
                (GetComponent<UnityChan.UnityChanControlScriptWithRgidBody>().attacking && !other.gameObject.GetComponent<CopyChan.CopyChanLocomotion>().attacking))
            {
                other.gameObject.GetComponent<Animator>().SetBool("Damage", true);
                sparkly.transform.position = new Vector3(other.gameObject.transform.position.x,
                    other.gameObject.transform.position.y, other.gameObject.transform.position.z);
                sparkly.Play();
                Destroy(other.gameObject, .5f);

            }
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            if (timeSlow && GetComponent<UnityChan.UnityChanControlScriptWithRgidBody>().attacking)
            {
                KillEnemy(other);
            }
            else if (GetComponent<UnityChan.UnityChanControlScriptWithRgidBody>().attacking && !other.gameObject.GetComponent<CopyChan.CopyChanLocomotion>().attacking)
            {
                KillEnemy(other);
            }
            else if (other.gameObject.GetComponent<CopyChan.CopyChanLocomotion>().attacking && !GetComponent<UnityChan.UnityChanControlScriptWithRgidBody>().damaged)
            {
                anim.SetBool("Damage", true);
                health -= 10;
                GetComponent<Rigidbody>().AddForce(Vector3.up * 10);
                GetComponent<Rigidbody>().AddForce(Vector3.forward * -10);
            }
        }
    }

    void KillEnemy(Collider other)
    {
        other.gameObject.GetComponent<Animator>().SetBool("Damage", true);
        sparkly.transform.position = new Vector3(other.gameObject.transform.position.x,
            other.gameObject.transform.position.y, other.gameObject.transform.position.z);
        sparkly.Play();
        Destroy(other.gameObject, .5f);
    }

    void GameOver()
    {
        InGameUI.enabled = false;
        gotWatch = false;
        transform.GetChild(7).gameObject.SetActive(false);
        if (TimeScale.player > 0)
        {
            TimeScale.player -= .02f;
            TimeScale.enemy -= .02f;
            TimeScale.global -= .02f;
        }
            
        else if (TimeScale.player < 0)
        {
            TimeScale.player = 0;
            TimeScale.enemy = 0;
            TimeScale.global = 0;
        }
        if (TimeScale.player == 0)
        {
            Time.timeScale = 0;
        }
    }
}
