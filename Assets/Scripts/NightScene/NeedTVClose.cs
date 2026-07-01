using System.Collections;
using UnityEngine;

public class NeedTVClose : MonoBehaviour
{
    [SerializeField] private PlayerController pc;
    [SerializeField] private Control c;
    [SerializeField] private GameObject monster;
    [SerializeField] private GameObject tv;
    [SerializeField] private GameObject point;
    [SerializeField] private AudioClip breath;
    [SerializeField] private AudioClip lightsOut;
    [SerializeField] private AudioClip jumpScare;
    [SerializeField] private AudioClip fall;

    private bool touched = false;

    private void OnTriggerEnter(Collider other)
    {
        if (touched) return;

        if (!c.IsClosed())
        {
            touched = true;
            StartCoroutine(MonsterKill());
        }
        else
        {
            Destroy(point);
            Destroy(gameObject);
        }
    }
    
    private IEnumerator MonsterKill()
    {
        pc.LookAt(new Vector3(-50.508f, 1.884f, -64.3f), 4.0f);
        MainManager.instance.AddTrigger("wait;11");
        MainManager.instance.PlayEffect(breath);
        float t = 0, subT = 0;
        while (t < 4.0f)
        {
            RenderSettings.fogDensity = Mathf.Lerp(0.6f, 0.2f, t / 4.0f);
            t += Time.deltaTime;
            yield return null;
        }

        monster.SetActive(true);
        pc.LookAt(new Vector3(-50.508f, 2.1f, -64.6f), 6.0f);
        t = 0;
        Vector3 startPos = monster.transform.position;
        Vector3 endPos = monster.transform.position + Vector3.up * 1.6f;
        Vector3 startScale = monster.transform.localScale;
        Vector3 endScale = monster.transform.localScale * 1.6f;
        while (t < 6.0f)
        {
            if (subT <= 0 && Random.Range(1, 100) == 1) subT = Random.Range(0.1f, 0.3f);
            if (subT > 0) RenderSettings.fogDensity = 1.0f;
            else RenderSettings.fogDensity = 0.2f;
            monster.transform.position = Vector3.Lerp(startPos, endPos, t / 6.0f);
            monster.transform.localScale = Vector3.Lerp(startScale, endScale, t / 6.0f);
            subT -= Time.deltaTime;
            t += Time.deltaTime;
            yield return null;
        }
        MainManager.instance.PlayEffect(lightsOut);
        monster.SetActive(false);
        Destroy(tv);
        RenderSettings.fogDensity = 1.0f;
        RenderSettings.ambientIntensity = 0.2f;

        yield return new WaitForSeconds(5.0f);
        yield return new WaitUntil(() => MainManager.instance.gameState == 1);
        monster.SetActive(true);
        monster.transform.position = point.transform.position;
        monster.transform.rotation = point.transform.rotation;
        RenderSettings.fogDensity = 0.5f;
        RenderSettings.ambientIntensity = 0.6f;
        MainManager.instance.PlayEffect(jumpScare);
        MainManager.instance.AddTrigger("wait;0.8");
        MainManager.instance.AddTrigger("changescreen;#FF0000FF;#FF0000FF;1");
        MainManager.instance.AddTrigger("changescreen;#FF0000FF;#000000FF;4");
        MainManager.instance.AddTrigger("loadscene;NightScene");
        yield return new WaitForSeconds(0.8f);
        MainManager.instance.PlayEffect(fall);
    }
}