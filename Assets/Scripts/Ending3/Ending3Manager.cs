using System.Collections;
using UnityEngine;

public class Ending3Manager : MonoBehaviour
{
    [SerializeField] private Transform head;
    [SerializeField] private AudioClip bang;

    private AudioSource headAd;

    private void Start()
    {
        headAd = head.GetComponent<AudioSource>();
        StartCoroutine(EndIt());
    }

    private IEnumerator EndIt()
    {
        MainManager.instance.AddTrigger("changescreen;#000000FF;#00000000;10");
        MainManager.instance.AddTrigger("wait;7");
        MainManager.instance.AddTrigger("changescreen;#000000FF;#000000FF;0.2");
        MainManager.instance.AddTrigger("ending;Ending 3/5 - Return;Your greatest fear came. They materialized.");
        float t = 0;
        while(t < 16.3f)
        {
            headAd.volume = Mathf.Lerp(0, PlayerPrefs.GetFloat("Effects", 80.0f) / 100.0f, t / 16.2f);
            t += Time.deltaTime;
            yield return null;
        }
        t = 0;
        float dis = Vector3.Distance(head.position, transform.position) - 0.25f;
        Vector3 startPos = head.position;
        Vector3 endPos = head.position + head.forward * dis;
        while(t < 0.7f)
        {
            head.position = Vector3.Lerp(startPos, endPos, t / 0.7f);
            t += Time.deltaTime;
            yield return null;
        }
        head.position = endPos;
        MainManager.instance.PlayEffect(bang);
        headAd.Stop();
    }
}
