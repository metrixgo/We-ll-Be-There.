using System.Collections;
using UnityEngine;

public class Stove : MonoBehaviour
{
    [SerializeField] private ParticleSystem ps;
    [SerializeField] private AudioSource ad;

    private bool opened = true;
    private bool isChanging = false;
    private float psE;
    private float adV;
    private AudioSource selfAd;

    private void Start()
    {
        psE = ps.emission.rateOverTime.constant;
        adV = ad.volume;
        selfAd = GetComponent<AudioSource>();
    }

    public void ChangeFire()
    {
        if (!isChanging)
        {
            isChanging = true;
            StartCoroutine(Change());
        }
    }

    private IEnumerator Change()
    {
        float t = 0;
        float l = 5.0f;
        ParticleSystem.EmissionModule e = ps.emission;
        selfAd.Play();
        if (opened)
        {
            while (t < l)
            {
                e.rateOverTime = Mathf.Lerp(psE, 0, t / l);
                ad.volume = Mathf.Lerp(adV, 0, t / l);
                t += Time.deltaTime;
                yield return null;
            }
            ps.Stop();
            ad.Stop();
        }
        else
        {
            ps.Play();
            ad.Play();
            while (t < l)
            {
                e.rateOverTime = Mathf.Lerp(0, psE, t / l);
                ad.volume = Mathf.Lerp(0, adV, t / l);
                t += Time.deltaTime;
                yield return null;
            }
        }
        isChanging = false;
        opened = !opened;
    }
}
