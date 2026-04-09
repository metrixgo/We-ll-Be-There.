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
            MainManager.instance.AddTrigger("dialogue;You;This is all for now! Follow me on itch to see how I work out this game!");
            MainManager.instance.AddTrigger("dialogue;You;Also, I'm a pretty new game developer, so if you have any suggestions, feel free to leave a comment!");
        }
        else if (!ad.isPlaying)
        {
            ad.Play();
            MainManager.instance.AddTrigger("dialogue;You;This is all for now! Follow me on itch to see how I work out this game!");
            MainManager.instance.AddTrigger("dialogue;You;Also, I'm a pretty new game developer, so if you have any suggestions, feel free to leave a comment!");
        }
    }
}
