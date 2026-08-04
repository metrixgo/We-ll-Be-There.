using UnityEngine;

public class SewerMusicBox : MonoBehaviour
{
    [SerializeField] private bool closed = false;
    [SerializeField] private GameObject trig;
    [SerializeField] private GameObject soundTrig;
    [SerializeField] private GameObject clock;
    [SerializeField] private GameObject bloods;
    [SerializeField] private GameObject box;

    private AudioSource ad;

    private void Start()
    {
        ad = GetComponent<AudioSource>();
    }

    public void Close()
    {
        if (!closed)
        {
            closed = true;
            bloods.SetActive(true);
            box.tag = "Interactable";
            Destroy(trig);
            Destroy(soundTrig);
            Destroy(clock);
        }
        if (!ad.isPlaying) ad.Play();
    }
}
