using UnityEngine;

public class CheckRoomsManager : MonoBehaviour
{
    private int cnt = 0;
    private bool touched = false;
    private string header;
    private string enclose;

    private void Start()
    {
        if (PlayerPrefs.GetString("Language", "English") == "English")
        {
            header = "Check Rooms (";
            enclose = ")";
        }
        else
        {
            header = "¼ì²é·¿¼ä £¨";
            enclose = "£©";
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!touched && MainManager.instance.gameState == 1)
        {
            touched = true;
            MainManager.instance.AddTrigger("dialogue;You;Where did they go? I don't see them leaving at all.");
            MainManager.instance.AddTrigger("dialogue;You;That's so weird. I need to go check all the rooms on the second floor.");
            MainManager.instance.AddTrigger("task;" + header + "0/6" + enclose);
        }
    }
}
