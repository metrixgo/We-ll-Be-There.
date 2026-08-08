using UnityEngine;

public class BathroomDoor : MonoBehaviour
{
    [SerializeField] private AudioClip lockedDoor;
    [SerializeField] private AudioClip open;

    private AudioSource ad;
    private bool interacted = false;
    private bool locked = true;

    private void Start()
    {
        ad = GetComponent<AudioSource>();
    }

    public void TryOpen()
    {
        if (MainManager.instance.HasItem("Crowbar"))
        {
            if (locked)
            {
                Debug.Log("YOU'RE A FUCKING GENIUS");
            }
        }
        else
        {
            if (!interacted)
            {
                interacted = true;
                MainManager.instance.AddTrigger("wait;" + lockedDoor.length);
                MainManager.instance.AddTrigger("dialogue;You;Fuck... It is locked... It's over...");
            }
            if (!ad.isPlaying)
            {
                ad.clip = lockedDoor;
                ad.Play();
            }
        }
    }
}
