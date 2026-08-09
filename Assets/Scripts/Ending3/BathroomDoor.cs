using UnityEngine;

public class BathroomDoor : MonoBehaviour
{
    [SerializeField] private AudioClip lockedDoor;
    [SerializeField] private AudioClip open;

    private AudioSource ad;
    private bool interacted = false;
    private bool keyInteracted = false;

    private void Start()
    {
        ad = GetComponent<AudioSource>();
    }

    public void TryOpen()
    {


        if (MainManager.instance.HasItem("Crowbar"))
        {
            Debug.Log("YOU'RE A FUCKING GENIUS");
        }
        else if (MainManager.instance.HasItem("Key"))
        {
            if (!keyInteracted)
            {
                keyInteracted = true;
                MainManager.instance.AddTrigger("wait;" + lockedDoor.length);
                MainManager.instance.AddTrigger("dialogue;You;Fuck... The key won't fit... It's over...");
            }
            if (!ad.isPlaying)
            {
                ad.clip = lockedDoor;
                ad.Play();
            }
        }
        else
        {
            if (!interacted)
            {
                interacted = true;
                MainManager.instance.AddTrigger("wait;" + lockedDoor.length);
                MainManager.instance.AddTrigger("dialogue;You;It's locked?! I need to find the key NOW.");
            }
            if (!ad.isPlaying)
            {
                ad.clip = lockedDoor;
                ad.Play();
            }
        }
    }
}
