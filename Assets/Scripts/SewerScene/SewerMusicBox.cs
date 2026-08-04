using UnityEngine;

public class SewerMusicBox : MonoBehaviour
{
    [SerializeField] private GameObject trig;
    [SerializeField] private GameObject soundTrig;
    [SerializeField] private GameObject clock;
    [SerializeField] private GameObject bloods;
    [SerializeField] private GameObject box;

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
