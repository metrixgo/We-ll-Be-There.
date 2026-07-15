using System.Collections;
using UnityEngine;

public class SewerClock : MonoBehaviour
{
    [SerializeField] private AudioClip tick;
    [SerializeField] private AudioClip tock;

    private int beats = 0;
    private float t = 0;
    private int tockRemain = 0;
    private AudioSource ad;

    private void Start()
    {
        ad = GetComponent<AudioSource>();
        StartCoroutine(Prompting());
    }

    private void Update()
    {
        t += Time.deltaTime;
        if (t >= 0.5f)
        {
            t -= 0.5f;
            if (tockRemain == 0)
            {
                ad.clip = tick;
                tockRemain = 3;
            }
            else
            {
                ad.clip = tock;
                tockRemain--;
            }
            ad.Play();
            beats++;
        }
    }

    private IEnumerator Prompting()
    {
        MainManager.instance.AddTrigger("canrun;1");
        MainManager.instance.AddTrigger("flashprompt;Press [Shift] to run");
        yield return new WaitForSeconds(3.0f);
        MainManager.instance.AddTrigger("flashdialogue;???;A group ****** will arrive ****** three minutes ****** to ****** directly ****** you. Please ****** ****** ****** ****** ****** ****** ****** ****** ****** ****** ****** ****** ****** ****** ****** ****** ****** ****** ****** ****** ****** ****** ****** ****** ****** ****** ****** ****** ****** ******;0");
    }
}