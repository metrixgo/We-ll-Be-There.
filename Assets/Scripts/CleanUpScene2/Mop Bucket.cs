using UnityEngine;

public class MopBucket : MonoBehaviour
{
    [SerializeField] private AudioClip washing;

    public void Interact()
    {
        if (!MainManager.instance.HasItem("Mop") && !MainManager.instance.HasItem("Shovel"))
        {
            if (CleanUpClock.clock.OnlyBucket())
            {

            }
            else
            {
                MainManager.instance.AddTrigger("dialogue;You;I can wash things here. I also need to clean this up when I'm done using it.");
            }
        }
        else if (MainManager.instance.HasItem("Mop"))
        {
            MainManager.instance.PlayEffect(washing);
            MainManager.instance.AddTrigger("wait;"+washing.length);
            MainManager.instance.RemoveTask("Mop?");
            CleanUpClock.clock.Clean("mop", true);
            CleanUpClock.clock.Clean("mopbucket", false);
        }
        else if (MainManager.instance.HasItem("Shovel"))
        {
            MainManager.instance.PlayEffect(washing);
            MainManager.instance.AddTrigger("wait;" + washing.length);
            MainManager.instance.RemoveTask("Shovel?");
            CleanUpClock.clock.Clean("shovel", true);
            CleanUpClock.clock.Clean("mopbucket", false);
        }
    }
}
