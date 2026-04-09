using System.Collections;
using UnityEngine;

public class MonsterTrigger : MonoBehaviour
{
    [SerializeField] private PlayerController pc;
    [SerializeField] private GameObject monster;
    [SerializeField] private GameObject wall;
    [SerializeField] private GameObject hammer;
    [SerializeField] private GameObject classroom;
    [SerializeField] private GameObject normal;
    [SerializeField] private GameObject blood;
    [SerializeField] private AudioClip jumpScare;
    [SerializeField] private AudioClip chaseMusic;

    private bool triggered = false;

    private void OnTriggerEnter(Collider other)
    {
        int n = DreamNewspaper.numLooked;

        if (n == 2) MainManager.instance.AddTrigger("dialogue;You;I still want to see the last newspaper...");
        else if (n == 1) MainManager.instance.AddTrigger("dialogue;You;I still want to see the remaining two newspapers...");
        else if (n == 0) MainManager.instance.AddTrigger("dialogue;You;I really want to see all the newspapers...");
        else if (!triggered)
        {
            triggered = true;
            StartCoroutine(StartChase());
        }
    }

    private IEnumerator StartChase()
    {
        Destroy(wall);
        yield return new WaitForSeconds(2.0f);
        yield return new WaitUntil(() => MainManager.instance.gameState == 1);

        MainManager.instance.PlayEffect(jumpScare);
        MainManager.instance.AddTrigger("wait;1.5");
        pc.LookAt(monster.transform.position, 0.2f);
        monster.SetActive(true);

        float t = 0, v = RenderSettings.fogDensity;
        while(t < 1.0f)
        {
            RenderSettings.fogDensity = Mathf.Lerp(v, 0.2f, t / 1.0f);
            t += Time.deltaTime;
            yield return null;
        }
        RenderSettings.fogDensity = 0.2f;

        yield return new WaitForSeconds(0.5f);
        pc.CanRun(true);
        monster.GetComponent<Animator>().SetBool("Run", true);
        MainManager.instance.SetPrompt("Press [Shift] to run", true);
        hammer.transform.SetParent(classroom.transform);
        hammer.AddComponent<Rigidbody>();
        MainManager.instance.PlayMusic(chaseMusic);
        Destroy(normal);
        blood.SetActive(true);
        Destroy(gameObject);
    }
}
