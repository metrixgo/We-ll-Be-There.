using UnityEngine;

public class Whispers : MonoBehaviour
{
    [SerializeField] private CleanUpClock c;

    private AudioSource ad;

    private void Start()
    {
        ad = GetComponent<AudioSource>();
    }

    private void Update()
    {
        ad.volume = c.GetProgress() * PlayerPrefs.GetFloat("Music", 30.0f) / 100.0f;
    }
}
