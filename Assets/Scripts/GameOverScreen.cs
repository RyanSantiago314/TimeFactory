using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverScreen : MonoBehaviour
{
    public Text title;
    public Text subtitle;

    public GameObject unitychan;
    TimeController script;
    CharController charCon;

    public int subNumber;
    // Start is called before the first frame update
    void Start()
    {
        script = unitychan.GetComponent<TimeController>();
        charCon = unitychan.GetComponent<CharController>();
        subNumber = Random.Range(1000, 9999);
    }

    // Update is called once per frame
    void Update()
    {
        if (script.health <= 0)
        {
            subtitle.color = Color.white;
            title.color = Color.white;
            title.text = "Project Unity Subject " + subNumber + " Test Failed";

            if (!script.gotWatch)
                subtitle.text = "Rogue Unit, Replace immediately";
            else if (charCon.enemyCount == 1)
                subtitle.text = "Consider adjusting: Composure in one-on-one situations";
            else if (charCon.enemyCount > 1)
                subtitle.text = "Consider adjusting: Composure in crowds";
            else if (charCon.hitObstacle > 5)
                subtitle.text = "Consider adjusting: Dodging Protocol";
            else if (charCon.falls > 5)
                subtitle.text = "Consider adjusting: Motor Skills Functionality";
            else
                subtitle.text = "Test Subject is kind of just bad at living.";
        }
        else
        {
            subtitle.color = Color.black;
            title.color = Color.black;

            title.text = "Project Unity Subject " + subNumber + " Test Successful";
            if (charCon.killCount > 32)
                subtitle.text = "Terminate Immediately, Genocidal Behavior Detected";
            else if (charCon.killCount > 10)
                subtitle.text = "Note: Check for murderous behavior in programming";
            else if (charCon.killCount >= 5)
                subtitle.text = "Note: Kills only if deemed necessary";
            else if (charCon.killCount == 0)
                subtitle.text = "Note: Pacifist";
            else if (!script.tutorial)
                subtitle.text = "Note: Inherited Knowledge or Extreme Intelligence";
            else if (charCon.hitObstacle > 8)
                subtitle.text = "Note: Possible masochistic tendencies";
            else if (charCon.falls > 5)
                subtitle.text = "Note: Likes Falling";
            else if (charCon.falls > 3)
                subtitle.text = "Note: Somewhat Clumsy";
            else
                subtitle.text = "Note: Nothing outstanding, Completely ordinary";
        }
    }
}
