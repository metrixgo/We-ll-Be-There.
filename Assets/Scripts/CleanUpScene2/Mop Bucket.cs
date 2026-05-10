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

        }
        else if (MainManager.instance.HasItem("Shovel"))
        {

        }
    }
}
