using System.Collections;
using TMPro;
using UnityEngine;

public class CleanUpClock : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI countdown;

    private float length = 120.0f;
    private float seconds;

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

    public float GetProgress()
    {
        return 1.0f - seconds/length;
    }
}
