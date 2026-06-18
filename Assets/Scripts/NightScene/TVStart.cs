using System.Collections;
using UnityEngine;
using UnityEngine.Video;

public class TVStart : MonoBehaviour
{
    [SerializeField] private AudioClip getOnSofa;
    [SerializeField] private AudioClip turnOnTV;
    [SerializeField] private GameObject firstPlayer;
    [SerializeField] private GameObject player;
    [SerializeField] private Material stat;

    private float t = 0;
    private AudioSource tv;
    private Renderer rend;
    private VideoPlayer vp;

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
        MainManager.instance.AddTrigger("wait;10");
    }
}