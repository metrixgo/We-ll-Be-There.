using UnityEngine;

public class Sink : MonoBehaviour
{
    [SerializeField] private AudioClip washing;

    public void Wash()
    {
        if(MainManager.instance.HasItem("Mop Bucket"))
        {
            MainManager.instance.PlayEffect(washing);
            MainManager.instance.AddTrigger("wait;" + washing.length);
            CleanUpClock.clock.Clean("mopbucket", true);
        }
        else
        {
            MainManager.instance.AddTrigger("dialogue;You;I can wash my mop bucket here. I believe it's somewhere on the first floor.");
        }
    }
}
