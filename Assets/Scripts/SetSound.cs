using UnityEngine;

public class SetSound : MonoBehaviour
{
    [SerializeField] private bool isEffect = true;
    [SerializeField] private float multiplier = 1;
    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        Set();
    }

    public void Set()
    {
        if (isEffect) audioSource.volume = PlayerPrefs.GetFloat("Effects", 80.0f) * multiplier / 100.0f;
        else audioSource.volume = PlayerPrefs.GetFloat("Music", 30.0f) * multiplier / 100.0f;
    }
}
