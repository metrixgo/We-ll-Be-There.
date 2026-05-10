using UnityEngine;

public class MopBucket : MonoBehaviour
{
    [SerializeField] private AudioClip washing;

    public void Interact()
    {
        if (!MainManager.instance.HasItem("Mop") && !MainManager.instance.HasItem("Shovel"))
        {
            MainManager.instance.AddTrigger("dialogue;You;I can wash things here.");
        }
        else if (MainManager.instance.HasItem("Mop"))
        {
            MainManager.instance.PlayEffect(washing);
            MainManager.instance.AddTrigger("wait;"+washing.length);
            MainManager.instance.RemoveTask("Mop?");
            CleanUpClock.clock.Clean("mop", true);
        }
        else if (MainManager.instance.HasItem("Shovel"))
        {
            MainManager.instance.PlayEffect(washing);
            MainManager.instance.AddTrigger("wait;" + washing.length);
            MainManager.instance.RemoveTask("Shovel?");
            CleanUpClock.clock.Clean("shovel", true);
        }
    }
}
