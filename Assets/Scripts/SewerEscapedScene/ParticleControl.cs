using System.Collections;
using UnityEngine;

public class ParticleControl : MonoBehaviour
{
    [SerializeField] private bool isButton = false;
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

    public void ChangeParticles()
    {
        if (!isChanging)
        {
            isChanging = true;
            StartCoroutine(Change());
        }
    }

    public bool IsOpened()
    {
        return opened;
    }

    private IEnumerator Change()
    {
        float t = 0;
        float l = 5.0f;
        ParticleSystem.EmissionModule e = ps.emission;
        selfAd.Play();
        if (opened)
        {
            if (isButton)
            {
                while (t < 0.5f)
                {
                    transform.Rotate(0, 90.0f * Time.deltaTime, 0);
                    t += Time.deltaTime;
                    yield return null;
                }
                t = 0;
            }

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
            if (isButton)
            {
                while (t < 0.5f)
                {
                    transform.Rotate(0, -90.0f * Time.deltaTime, 0);
                    t += Time.deltaTime;
                    yield return null;
                }
                t = 0;
            }

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
