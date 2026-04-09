using TMPro;
using UnityEngine;

public class TextTranslation : MonoBehaviour
{
    private TextMeshProUGUI txt;
    private TextMeshPro txt2;

    private void OnEnable()
    {
        txt = GetComponent<TextMeshProUGUI>();
        if(txt == null) txt2 = GetComponent<TextMeshPro>();
        if (PlayerPrefs.GetString("Language", "English") == "Chinese")
        {
            string s = gameObject.name.ToLower();
            string r;
            if (s == "options") r = "选项";
            else if (s == "start") r = "开始";
            else if (s == "quit") r = "退出";
            else if (s == "we'll be there.") r = "我们会在。";
            else if (s == "language") r = "语言";
            else if (s == "sensitivity") r = "灵敏度";
            else if (s == "music") r = "音乐";
            else if (s == "sfx") r = "音效";
            else if (s == "back") r = "返回";
            else if (s == "paused") r = "暂停";
            else if (s == "main menu") r = "主菜单";
            else if (s == "esc to continue...") r = "按Esc继续...";
            else if (s == "nowhere to run :)") r = "无处可逃 :)";
            else r = s;
            if(txt != null) txt.text = r;
            else txt2.text = r;
        }
        else
        {
            if (txt != null) txt.text = gameObject.name;
            else txt2.text = gameObject.name;
        }
    }
}
