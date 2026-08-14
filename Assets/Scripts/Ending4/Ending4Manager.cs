using UnityEngine;

public class Ending4Manager : MonoBehaviour
{
    private void Start()
    {
        MainManager.instance.AddTrigger("changescreen;#000000FF;#00000000;4");
        MainManager.instance.AddTrigger("dialogue;You;...");
    }
}
