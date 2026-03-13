using UnityEngine;

public class SetSound : MonoBehaviour
{
    [SerializeField] private bool isEffect = true;
    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if(isEffect) audioSource.volume = PlayerPrefs.GetFloat("Effects", 80.0f) / 100.0f;
        else audioSource.volume = PlayerPrefs.GetFloat("Music", 30.0f) / 100.0f;
    }
}
