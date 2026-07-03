using UnityEngine;

public class NightTomb : MonoBehaviour
{
    public void dig()
    {
        if (!MainManager.instance.HasItem("Shovel"))
        {
            MainManager.instance.AddTrigger("dialogue;You;I need a shovel to dig open this.");
        }
        else
        {
            MainManager.instance.AddTrigger("dialogue;You;This is all for now! Follow me on itch to see how I work out this game!");
            MainManager.instance.AddTrigger("dialogue;You;Also, I'm a pretty new game developer, so if you have any suggestions, feel free to leave a comment!");
        }
    }
}
