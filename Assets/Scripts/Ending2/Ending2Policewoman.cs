using System.Collections;
using UnityEngine;

public class Ending2Policewoman : MonoBehaviour
{
    [SerializeField] private AudioClip glassBreak;
    [SerializeField] private AudioClip doorKnock;
    [SerializeField] private GameObject firstPlayer;
    [SerializeField] private GameObject player;

    private AudioSource ad;

    private void Start()
    {
        ad = GetComponent<AudioSource>();
        StartCoroutine(EndIt());
    }

    private IEnumerator EndIt()
    {
        MainManager.instance.AddTrigger("wait;2");
        MainManager.instance.AddTrigger("changescreen;#000000ff;#00000000;1");
        MainManager.instance.AddTrigger("wait;10");
        ad.clip = glassBreak;
        ad.Play();
        yield return new WaitForSeconds(13.0f);
        Destroy(firstPlayer);
        player.SetActive(true);
    }

    
}
