using UnityEngine;

public class AlarmClock : MonoBehaviour
{
    [SerializeField] private AudioClip stop;

    private AudioSource ad;
    private bool interacted;

    private void Start()
    {
        ad = GetComponent<AudioSource>();
    }

    public void Interact()
    {
        if (!interacted)
        {
            interacted = true;
            ad.Stop();
            ad.clip = stop;
            ad.loop = false;
            ad.Play();
        }
        else if (!ad.isPlaying)
        {
            ad.Play();
        }
    }
}
