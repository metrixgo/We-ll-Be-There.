using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.TestTools;

public class CleanUpClock : MonoBehaviour
{
    public static CleanUpClock clock;

    [SerializeField] private TextMeshProUGUI countdown;

    private float length = 120.0f;
    private float seconds;
    private int count = 0;
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
    }

    private void Start()
    {
        seconds = length;
        StartCoroutine(CountDown());
    }

    private IEnumerator CountDown()
    {
        yield return new WaitUntil(() => MainManager.instance.gameState == 1);
        yield return new WaitUntil(() => MainManager.instance.gameState == 1);

        while (seconds > 0)
        {
            if(MainManager.instance.AtPausedScreen())
            {
                countdown.text = "";
                yield return null;
                continue;
            }

            int sec = Mathf.CeilToInt(seconds);
            if(sec % 60 < 10) countdown.text = sec / 60 + ":0" + sec % 60;
            else countdown.text = sec / 60 + ":" + sec % 60;
            if(sec <= 20) countdown.color = Color.red;
            seconds -= Time.deltaTime;
            yield return null;
        }
        countdown.text = "0:00";
    }

    public bool OnlyBucket()
    {
        return status["covered"] && status["shovel"] && status["mop"] && count == 6;
    }

    public float GetProgress()
    {
        return 1.0f - seconds/length;
    }

    public void Clean(string s, bool b)
    {
        status[s] = b;
    }

    public void FinishedOne()
    {
        count++;
    }
}
