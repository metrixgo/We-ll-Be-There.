using System.Collections;
using TMPro;
using UnityEngine;

public class CleanUpClock : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI countdown;

    private void Start()
    {
        StartCoroutine(CountDown());
    }

    private IEnumerator CountDown()
    {
        float seconds = 120;
        yield return new WaitUntil(() => MainManager.instance.gameState == 1);

        while(seconds > 0)
        {
            if(MainManager.instance.gameState != 1)
            {
                yield return null;
                continue;
            }

            int sec = Mathf.CeilToInt(seconds);
            if(sec % 60 < 10) countdown.text = sec / 60 + ":0" + sec % 60;
            else countdown.text = sec / 60 + ":" + sec % 60;
            if(sec <= 10) countdown.color = Color.red;
            seconds -= Time.deltaTime;
            yield return null;
        }
        countdown.text = "0:00";
    }
}
