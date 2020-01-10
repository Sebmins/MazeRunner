using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public Text timerText;
    public GameObject timesUp;
    private float startTime;
    int highscore;
    public static bool pause;
    public static bool gameOver;
    public static int time;
    private int minute;
    private int second;
    // Start is called before the first frame update
    void Start()
    {
        int level = SceneManager.GetActiveScene().buildIndex;
        if (level < 4)
            minute = level - 1;
        else if (level < 6)
            minute = 2;
        else if (level == 6)
            minute = 4;

        second = 59;
        timerText.text = "0:00";
        pause = true;
        startTime = Time.deltaTime;
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        if (pause == true)
        {
            return;
        }

        startTime -= Time.fixedDeltaTime;

        float t = startTime;

        string minutes = (minute+((int)t / 60)).ToString();
        string seconds = (second+(t % 60)).ToString("00");

        if (seconds == "-01")
            seconds = "00";

        timerText.text = minutes+":" +seconds ;


        if(minutes == "0" && seconds ==  "00")
        {
            timerText.text = "0:00";
            gameOver = true;
            pause = true;
            timesUp.SetActive(true);
            highscore = LevelText.levelCount;
        }
    }
}
