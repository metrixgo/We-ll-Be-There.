using UnityEngine;

public class SewerMusicBox : MonoBehaviour
{
    [SerializeField] private AudioClip tense;
    [SerializeField] private GameObject trig;
    [SerializeField] private GameObject soundTrig;

    private AudioSource ad;
    private bool closed = false;

    private void Start()
    {
        ad = GetComponent<AudioSource>();
    }

    public void Close()
    {
        if (!closed)
        {
            SewerMusicManager.instance.ChangeTo(tense, 3.0f, 50.0f);
            closed = true;
            Destroy(trig);
            Destroy(soundTrig);
        }
        if (!ad.isPlaying) ad.Play();
    }
}
