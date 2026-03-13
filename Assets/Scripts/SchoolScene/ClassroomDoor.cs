using UnityEngine;

public class ClassroomDoor : MonoBehaviour 
{
    private bool interacted = false;
    private AudioSource ad;
    [SerializeField] private AudioClip lockedDoor;
    [SerializeField] private AudioClip unlockDoor;

    private void Start()
    {
        ad = GetComponent<AudioSource>();
    }

    public void OpenDoor()
    {
        if (MainManager.instance.gameState != 1) return ;
        if (MainManager.instance.HasItem("Classroom Key"))
        {
            ad.clip = unlockDoor;
            Vector3 pos = transform.position - transform.right * 0.8f;
            MainManager.instance.AddTrigger("changescreen;#00000000;#000000FF;0.5");
            MainManager.instance.AddTrigger("moveplayerto;"+pos.x+";1.33;"+pos.z);
            MainManager.instance.AddTrigger("changescreen;#000000FF;#00000000;0.5");
        }
        else if (!interacted)
        {
            ad.clip = lockedDoor;
            interacted = true;
            MainManager.instance.AddTrigger("dialogue;You;The door is locked... I knew it.");
            MainManager.instance.AddTrigger("dialogue;You;They must've not noticed me when they locked this classroom...");
        }
        else ad.clip = lockedDoor;
        
        if (!ad.isPlaying || ad.clip == unlockDoor) ad.Play();
    }
}
