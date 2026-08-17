using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CleanUpClock : MonoBehaviour
{
    public static CleanUpClock clock;
    public static string errorType;

    [SerializeField] private TextMeshProUGUI countdown;

    private float length = 180.0f;
    private float seconds;
    private int count = 0;
    private AudioSource ad;
    private Dictionary<string, bool> status = new Dictionary<string, bool>()
    {
        {"covered", false},
        {"shovel", false},
        {"mop", false},
        {"clothes", false},
        {"mopbucket", true},
    };

    private void Awake()
    {
        clock = this;
        ad = GetComponent<AudioSource>();
    }

    private void Start()
    {
        seconds = length;
        MainManager.instance.AddTask("Mop?");
        MainManager.instance.AddTask("Shovel?");
        MainManager.instance.AddTask("Clothes?");
        MainManager.instance.AddTask("Backyard?");
        MainManager.instance.AddTask("Blood?");
        MainManager.instance.AddTask("Mop bucket?");
        StartCoroutine(CountDown());
    }

    private IEnumerator CountDown()
    {
        yield return new WaitUntil(() => MainManager.instance.gameState == 1);
        yield return new WaitUntil(() => MainManager.instance.gameState == 1);

        while (seconds > 0)
        {
            if (MainManager.instance.AtPausedScreen())
            {
                countdown.text = "";
                yield return null;
                continue;
            }

            int sec = Mathf.CeilToInt(seconds);
            if (sec % 60 < 10) countdown.text = sec / 60 + ":0" + sec % 60;
            else countdown.text = sec / 60 + ":" + sec % 60;
            if (sec <= 20) countdown.color = Color.red;
            seconds -= Time.deltaTime;
            ad.volume = (1.0f - seconds / length) * PlayerPrefs.GetFloat("Music", 30.0f) / 100.0f;
            yield return null;
        }
        countdown.text = "0:00";
        StartCoroutine(TimeUp());
    }

    public bool GetStatus(string s)
    {
        return status[s];
    }

    public void Clean(string s, bool b)
    {
        status[s] = b;
        string type = "";
        if (s == "mop") type = "Mop?";
        else if (s == "shovel") type = "Shovel?";
        else if (s == "clothes") type = "Clothes?";
        else if (s == "covered") type = "Backyard?";
        else if (s == "blood") type = "Blood?";
        else if (s == "mopbucket") type = "Mop bucket?";

        if (b) MainManager.instance.RemoveTask(type);
        else MainManager.instance.AddTask(type);
    }

    public void FinishedOne()
    {
        count++;
        if (count == 6) MainManager.instance.RemoveTask("Blood?");
    }

    private IEnumerator TimeUp()
    {
        yield return new WaitForSeconds(1.0f);
        yield return new WaitUntil(() => !MainManager.instance.AtPausedScreen());
        bool flg = false;
        if (!status["mop"]) errorType = "mop";
        else if (!status["shovel"]) errorType = "shovel";
        else if (!status["clothes"]) errorType = "clothes";
        else if (!status["covered"]) errorType = "covered";
        else if (count != 6) errorType = "blood";
        else if (!status["mopbucket"]) errorType = "mopbucket";
        else flg = true;

        if (flg) MainManager.instance.LoadScene("AfterCleanUpScene2");
        else MainManager.instance.LoadScene("Ending2");
    }
}
