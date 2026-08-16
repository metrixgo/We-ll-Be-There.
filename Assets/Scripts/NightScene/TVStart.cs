using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Video;

public class TVStart : MonoBehaviour
{
    [SerializeField] private AudioClip getOnSofa;
    [SerializeField] private AudioClip turnOnTV;
    [SerializeField] private AudioSource screamAd;
    [SerializeField] private TextMeshProUGUI timePrompt;
    [SerializeField] private GameObject stupidThing;
    [SerializeField] private GameObject firstPlayer;
    [SerializeField] private GameObject player;
    [SerializeField] private Material stat;
    [SerializeField] private Material redStat;

    private float t = 0;
    private AudioSource tv;
    private Renderer rend;
    private VideoPlayer vp;
    private bool getUp = false;

    private void Start()
    {
        tv = GetComponent<AudioSource>();
        rend = GetComponent<Renderer>();
        vp = GetComponent<VideoPlayer>();
        MainManager.instance.AddTrigger("wait;" + (getOnSofa.length + 1.0f));
        MainManager.instance.AddTrigger("changescreen;#000000FF;#00000000;2");
        StartCoroutine(BeginShow());
    }

    private void Update()
    {
        if (getUp) return;

        t += Time.deltaTime;
        Vector3 pos = firstPlayer.transform.position;
        pos.y = 1.35f + Mathf.Sin(t) / 100.0f;
        pos.x = -50.85f + Mathf.Cos(1.7f * t) / 200.0f;
        firstPlayer.transform.position = pos;
    }

    private IEnumerator BeginShow()
    {
        MainManager.instance.PlayEffect(getOnSofa);
        yield return new WaitForSeconds(getOnSofa.length);
        MainManager.instance.PlayEffect(turnOnTV);
        vp.Play();
        float t = 0;
        while(t < 0.5f)
        {
            timePrompt.color = Color.Lerp(Color.clear, Color.white, t / 0.5f);
            t += Time.deltaTime;
            yield return null;
        }
        yield return new WaitForSeconds(0.5f);
        t = 0;
        while (t < 2.0f)
        {
            timePrompt.color = Color.Lerp(Color.white, Color.clear, t / 2.0f);
            t += Time.deltaTime;
            yield return null;
        }
        Destroy(stupidThing);
        yield return new WaitForSeconds((float)vp.length - 3.0f);
        Destroy(vp);
        rend.material = stat;
        tv.Play();
        yield return new WaitForSeconds(3.0f);
        yield return new WaitUntil(() => MainManager.instance.gameState == 1);
        MainManager.instance.AddTrigger("dialogue;You;What's wrong with the TV?");
        yield return new WaitForSeconds(1.0f);
        yield return new WaitUntil(() => MainManager.instance.gameState == 1);
        yield return new WaitForSeconds(2.0f);
        RenderSettings.fogDensity = 0.6f;
        rend.material = redStat;
        screamAd.Play();
        yield return new WaitForSeconds(3.0f);
        yield return new WaitUntil(() => MainManager.instance.gameState == 1);
        MainManager.instance.AddTrigger("dialogue;???;L-E-T-U-S-P-L-A-Y-A-G-A-M-E-?");
        MainManager.instance.AddTrigger("dialogue;You;What?");
        MainManager.instance.AddTrigger("chaosdialogue;???");
        MainManager.instance.AddTrigger("dialogue;You;I think I need to get out of here...");
        MainManager.instance.AddTrigger("chaosdialogue;???");
        MainManager.instance.AddTrigger("chaosdialogue;???");
        MainManager.instance.AddTrigger("dialogue;You;Now.");
        yield return new WaitForSeconds(1.0f);
        yield return new WaitUntil(() => MainManager.instance.gameState == 1);
        Vector3 startPos = firstPlayer.transform.position;
        Quaternion startRot = firstPlayer.transform.rotation;
        Vector3 endPos = new Vector3(-50.85f, 1.3f, -66.7f);
        Quaternion endRot = Quaternion.Euler(15.0f, -20.0f, 0);
        getUp = true;
        t = 0;
        while (t < 1.0f)
        {
            firstPlayer.transform.position = Vector3.Lerp(startPos, endPos, t);
            firstPlayer.transform.rotation = Quaternion.Slerp(startRot, endRot, t);
            t += Time.deltaTime;
            yield return null;
        }
        startPos = firstPlayer.transform.position;
        startRot = firstPlayer.transform.rotation;
        endPos = new Vector3(-51.0f, 2.21965301f, -66.3f);
        endRot = Quaternion.Euler(30.0f, -30.0f, 0);
        t = 0;
        while(t < 2.0f)
        {
            firstPlayer.transform.position = Vector3.Lerp(startPos, endPos, Mathf.SmoothStep(0, 1.0f, t / 2.0f));
            firstPlayer.transform.rotation = Quaternion.Slerp(startRot, endRot, Mathf.SmoothStep(0, 1.0f, t / 2.0f));
            t += Time.deltaTime;
            yield return null;
        }
        MainManager.instance.AddTrigger("flashprompt;Press [Shift] to run");
        MainManager.instance.AddTrigger("canrun;1");
        Destroy(firstPlayer);
        player.SetActive(true);
    }
}