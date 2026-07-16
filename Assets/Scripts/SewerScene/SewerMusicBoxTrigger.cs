using UnityEngine;

public class SewerMusicBoxTrigger : MonoBehaviour
{
    private float vol;
    private AudioSource ad;

    private void Start()
    {
        ad = GetComponent<AudioSource>();
        vol = ad.volume;
        ad.volume = 0;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) ad.volume = vol;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player")) ad.volume = 0;
    }
}
