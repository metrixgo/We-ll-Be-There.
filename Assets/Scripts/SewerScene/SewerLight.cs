using UnityEngine;

public class SewerLight : MonoBehaviour
{
    [SerializeField] private Material normalMat;
    [SerializeField] private Material flickerMat;
    [SerializeField] private Light bulb;

    private Renderer rend;
    private AudioSource ad;
    private float t = 0;

    private void Start()
    {
        rend = GetComponent<Renderer>();
        ad = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (t != 0)
        {
            t -= Time.deltaTime;
            if (t <= 0)
            {
                t = 0;
                bulb.enabled = true;
                ad.Play();
                rend.material = normalMat;
            }
        }
        
        if (t == 0 && Random.Range(0, 300) == 0)
        {
            ad.Stop();
            rend.material = flickerMat;
            t = Random.Range(0.1f, 0.5f);
            bulb.enabled = false;
        }
    }
}
