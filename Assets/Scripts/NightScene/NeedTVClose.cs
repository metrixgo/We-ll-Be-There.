using System.Collections;
using UnityEngine;
using UnityEngine.Device;
using UnityEngine.UI;

public class NeedTVClose : MonoBehaviour
{
    [SerializeField] private PlayerController pc;
    [SerializeField] private Control c;
    [SerializeField] private GameObject monster;
    [SerializeField] private GameObject monster2;
    [SerializeField] private GameObject tv;
    [SerializeField] private GameObject point;
    [SerializeField] private AudioClip breath;
    [SerializeField] private AudioClip lightsOut;
    [SerializeField] private AudioClip jumpScare;
    [SerializeField] private AudioClip fall;
    [SerializeField] private RawImage ri;
    [SerializeField] private Material mat;

    private bool touched = false;

    private void OnTriggerEnter(Collider other)
    {
        if (touched) return;
        touched = true;

        if (!c.IsClosed()) StartCoroutine(MonsterKill());
        else StartCoroutine(MonsterShow());
    }

    private IEnumerator MonsterShow()
    {
        yield return new WaitForSeconds(1.0f);
        for (int i = 1; i <= 10; i++)
        {
            monster2.SetActive(true);
            yield return new WaitForSeconds(0.1f);
            monster2.SetActive(false);
            yield return new WaitForSeconds(0.2f);
            monster2.transform.Translate(-0.16f, -0.24f, 0, Space.World);
        }
        Destroy(point);
        Destroy(monster);
        Destroy(monster2);
        Destroy(gameObject);
    }
    
    private IEnumerator MonsterKill()
    {
        pc.LookAt(new Vector3(-50.508f, 1.884f, -64.3f), 4.0f);
        MainManager.instance.AddTrigger("wait;11");
        MainManager.instance.PlayEffect(breath);
        float t = 0;
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
        float l = 0, w = 0;
        while (t < 6.0f)
        {
            if (l > 0)
            {
                RenderSettings.fogDensity = 1.0f;
                l -= Time.deltaTime;
            }
            else if (w > 0)
            {
                RenderSettings.fogDensity = 0.2f;
                w -= Time.deltaTime;
            }
            else
            {
                w = Random.Range(0.1f, 1.0f);
                l = Random.Range(0.1f, 0.3f);
            }
            monster.transform.position = Vector3.Lerp(startPos, endPos, t / 6.0f);
            monster.transform.localScale = Vector3.Lerp(startScale, endScale, t / 6.0f);
            t += Time.deltaTime;
            yield return null;
        }
        MainManager.instance.PlayEffect(lightsOut);
        monster.SetActive(false);
        Destroy(tv);
        RenderSettings.fogDensity = 1.0f;
        RenderSettings.ambientIntensity = 0.2f;
        MainManager.instance.AddTrigger("flashprompt;Press [Shift] to run");

        yield return new WaitForSeconds(5.0f);
        yield return new WaitUntil(() => MainManager.instance.gameState == 1);
        monster.SetActive(true);
        monster.transform.position = point.transform.position;
        monster.transform.rotation = point.transform.rotation;
        ri.material = mat;
        RenderSettings.fogDensity = 0.7f;
        RenderSettings.ambientIntensity = 1.0f;
        MainManager.instance.PlayEffect(jumpScare);
        MainManager.instance.AddTrigger("wait;0.8");
        MainManager.instance.AddTrigger("changescreen;#FF0000FF;#FF0000FF;1");
        MainManager.instance.AddTrigger("changescreen;#FF0000FF;#000000FF;4");
        MainManager.instance.AddTrigger("loadscene;NightScene");
        yield return new WaitForSeconds(0.8f);
        MainManager.instance.PlayEffect(fall);
    }
}