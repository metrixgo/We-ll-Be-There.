using System.Collections;
using UnityEngine;
using UnityEngine.Video;

public class TVStart : MonoBehaviour
{
    [SerializeField] private AudioSource tv;
    [SerializeField] private AudioClip getOnSofa;
    [SerializeField] private AudioClip turnOnTV;
    [SerializeField] private GameObject firstPlayer;
    [SerializeField] private GameObject player;
    [SerializeField] private Material stat;

    private Renderer rend;
    private VideoPlayer vp;

    private void Start()
    {
        rend = GetComponent<Renderer>();
        vp = GetComponent<VideoPlayer>();
        MainManager.instance.AddTrigger("wait;6");
        MainManager.instance.AddTrigger("changescreen;#000000FF;#00000000;2");
        StartCoroutine(BeginShow());
    }

    private IEnumerator BeginShow()
    {
        MainManager.instance.PlayEffect(getOnSofa);
        yield return new WaitForSeconds(getOnSofa.length + 1.0f);
        MainManager.instance.PlayEffect(turnOnTV);
        yield return new WaitForSeconds(turnOnTV.length);
        vp.Play();
        yield return new WaitForSeconds((float) vp.length);
        Material[] mats = rend.materials;
        mats[1] = stat;
        rend.materials = mats;
        tv.Play();
    }
}