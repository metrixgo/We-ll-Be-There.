using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Video;

public class TVStart : MonoBehaviour
{
    [SerializeField] private AudioClip getOnSofa;
    [SerializeField] private AudioClip turnOnTV;
    [SerializeField] private AudioSource screamAd;
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
        MainManager.instance.AddTrigger("wait;" + (getOnSofa.length + 1.2f + turnOnTV.length));
        MainManager.instance.AddTrigger("changescreen;#000000FF;#00000000;2");
        StartCoroutine(BeginShow());
    }

    private void Update()
    {
        if (getUp) return;

        t += Time.deltaTime;
        Vector3 pos = firstPlayer.transform.position;
        pos.y = 1.35f + Mathf.Sin(t) / 100.0f;
        pos.x = -50.85f + Mathf.Cos(2 * t) / 200.0f;
        firstPlayer.transform.position = pos;
    }

    private IEnumerator BeginShow()
    {
        MainManager.instance.PlayEffect(getOnSofa);
        yield return new WaitForSeconds(getOnSofa.length + 1.0f);
        MainManager.instance.PlayEffect(turnOnTV);
        yield return new WaitForSeconds(turnOnTV.length);
        vp.Play();
        yield return new WaitForSeconds((float) vp.length);
        rend.material = stat;
        tv.Play();
        yield return new WaitForSeconds(3.0f);
        yield return new WaitUntil(() => MainManager.instance.gameState == 1);
        MainManager.instance.AddTrigger("dialogue;You;What's wrong with the TV?");
        yield return new WaitForSeconds(1.0f);
        yield return new WaitUntil(() => MainManager.instance.gameState == 1);
        yield return new WaitForSeconds(2.0f);
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
        Vector3 endPos = new Vector3(-51.0f, 2.21965301f, -66.3f);
        Quaternion startRot = firstPlayer.transform.rotation;
        Quaternion endRot = Quaternion.Euler(20.0f, -20.0f, 0);
        getUp = true;
        t = 0;
        while(t < 2.0f)
        {
            firstPlayer.transform.position = Vector3.Lerp(startPos, endPos, t / 2.0f);
            firstPlayer.transform.rotation = Quaternion.Slerp(startRot, endRot, t / 2.0f);
            t += Time.deltaTime;
            yield return null;
        }
        MainManager.instance.AddTrigger("flashprompt;Press [Shift] to run");
        MainManager.instance.AddTrigger("canrun;1");
        Destroy(firstPlayer);
        player.SetActive(true);
    }
}