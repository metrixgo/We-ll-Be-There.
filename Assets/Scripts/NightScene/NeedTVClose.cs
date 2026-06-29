using System.Collections;
using UnityEngine;

public class NeedTVClose : MonoBehaviour
{
    [SerializeField] private PlayerController pc;
    [SerializeField] private Control c;
    [SerializeField] private GameObject monster;
    [SerializeField] private AudioClip outOfTV;
    [SerializeField] private AudioClip lightsOut;
    [SerializeField] private AudioClip jumpScare;
    [SerializeField] private AudioClip bite;

    private AudioSource monsterAd;
    private bool touched = false;

    private void Start()
    {
        monsterAd = monster.GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!c.IsClosed() && !touched)
        {
            touched = true;
            StartCoroutine(MonsterKill());
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    private IEnumerator MonsterKill()
    {
        pc.LookAt(new Vector3(-50.508f, 1.884f, -64.0f), 5.0f);
        MainManager.instance.AddTrigger("wait;12");
        float t = 0, subT = 0;
        while (t < 5.0f)
        {
            RenderSettings.fogDensity = Mathf.Lerp(0.6f, 0.1f, t / 5.0f);
            t += Time.deltaTime;
            yield return null;
        }

        monster.SetActive(true);
        monsterAd.clip = outOfTV;
        monsterAd.Play();
        pc.LookAt(new Vector3(-50.508f, 1.884f, -64.7f), 6.0f);
        t = 0;
        Vector3 startPos = monster.transform.position;
        Vector3 endPos = monster.transform.position + Vector3.forward * 1.5f;
        while (t < 6.0f)
        {
            if (subT <= 0 && Random.Range(1, 100) == 1) subT = Random.Range(0.1f, 0.3f);

            if (subT > 0) RenderSettings.fogDensity = 1.0f;
            else RenderSettings.fogDensity = 0.1f;
            monster.transform.position = Vector3.Lerp(startPos, endPos, t / 6.0f);
            subT -= Time.deltaTime;
            t += Time.deltaTime;
            yield return null;
        }
        
        MainManager.instance.PlayEffect(lightsOut);
        RenderSettings.fogDensity = 1.0f;
    }
}
