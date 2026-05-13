using UnityEngine;

public class MopBucket : MonoBehaviour
{
    [SerializeField] private AudioClip washing;
    [SerializeField] private AudioClip soundEffect;
    [SerializeField] private GameObject hold;
    [SerializeField] private Vector3 pos;
    [SerializeField] private Vector3 angle;
    [SerializeField] private GameObject putBack;

    public void Interact()
    {
        if (MainManager.instance.gameState != 1) return;

        if (!MainManager.instance.HasItem("Mop") && !MainManager.instance.HasItem("Shovel"))
        {
            if (CleanUpClock.clock.OnlyBucket())
            {
                if (hold.transform.childCount > 0)
                {
                    MainManager.instance.AddTrigger("dialogue;You;I don't want to hold two items at the same time...");
                }
                else
                {
                    MainManager.instance.AddItem(name);
                    MainManager.instance.PlayEffect(soundEffect);
                    transform.SetParent(hold.transform);
                    transform.localPosition = pos;
                    transform.localRotation = Quaternion.Euler(angle);
                    tag = "Untagged";
                    putBack.SetActive(true);
                }
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
