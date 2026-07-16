using System.Collections;
using UnityEngine;

public class SewerMusicManager : MonoBehaviour
{
    public static SewerMusicManager instance;

    private float vol;
    private AudioSource ad;
    private Coroutine prev = null;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        ad = GetComponent<AudioSource>();
        vol = ad.volume;
    }

    public void ChangeTo(AudioClip newClip, float l, float newV)
    {
        if (prev != null) StopCoroutine(prev);
        prev = StartCoroutine(ChangeToClip(newClip, l, newV));
    }

    private IEnumerator ChangeToClip(AudioClip newClip, float l, float newV)
    {
        l /= 2;
        float t = 0;
        while (t < l)
        {
            ad.volume = Mathf.Lerp(vol, 0, t / l);
            t += Time.deltaTime;
            yield return null;
        }

        ad.clip = newClip;
        float newVol = newV / 100.0f * PlayerPrefs.GetFloat("Music", 30.0f) / 100.0f;
        ad.volume = newVol;
        vol = newVol;
        ad.Play();

        t = 0;
        while (t < l)
        {
            ad.volume = Mathf.Lerp(0, vol, t / l);
            t += Time.deltaTime;
            yield return null;
        }

        ad.volume = vol;
    }
}
