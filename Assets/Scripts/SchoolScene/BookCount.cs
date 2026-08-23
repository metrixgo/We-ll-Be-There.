using TMPro;
using UnityEngine;

public class BookCount : MonoBehaviour
{
    [SerializeField] private TextMeshPro count;
    [SerializeField] private GameObject trigger;

    private string a;
    private string b;
    private bool first = false;

    private void Start()
    {
        a = MainManager.instance.Translate("...these books contain all the important information I need! Please, can you help me collect my remaining");
        b = MainManager.instance.Translate("books? Please... There are so many things inside...");
    }

    private void Update()
    {
        int cnt = 9 - MainManager.instance.ItemCount("Book");
        if (cnt > 0) count.text = cnt.ToString();
        else
        {
            count.fontSize = 2;
            count.text = MainManager.instance.Translate("Thanks.");
            if (!first)
            {
                first = true;
                Destroy(trigger);
            }
        }
    }

    public void LookAt()
    {
        string s = "";
        if (MainManager.instance.ItemCount("Book") == 9) s = "Thank you.";
        else s = a + " " + (9 - MainManager.instance.ItemCount("Book")) + " " + b;
        MainManager.instance.AddTrigger("dialogue;Poster;" + s);
    }
}
