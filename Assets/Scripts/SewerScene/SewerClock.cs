using UnityEngine;

public class SewerClock : MonoBehaviour
{
    [SerializeField] private AudioClip tick;
    [SerializeField] private AudioClip tock;
    [SerializeField] private int seconds = 0;

    private float t = 0;
    private bool isTick = true;
    private AudioSource ad;

    private void Start()
    {
        ad = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        MainManager.instance.AddTrigger("flashwait;3");
        MainManager.instance.AddTrigger("flashdialogue;???;A group ****** will arrive ****** three minutes ****** to ****** directly ****** you. Please ****** ****** ****** ****** ****** ****** ****** ****** ****** ****** ****** ****** ****** ****** ****** ****** ****** ****** ****** ****** ****** ****** ****** ****** ****** ****** ****** ****** ****** ****** ****** ****** ****** ****** ****** ****** ****** ****** ****** ****** ****** ****** ****** ****** ****** ****** ****** ******;0.1");
    }

    private void Update()
    {
        t += Time.deltaTime;
        if (t >= 1.0f)
        {
            t -= 1.0f;
            if (isTick) ad.clip = tick;
            else ad.clip = tock;
            ad.Play();
            isTick = !isTick;
            seconds++;
        }
    }
}