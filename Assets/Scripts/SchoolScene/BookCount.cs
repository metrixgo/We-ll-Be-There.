using TMPro;
using UnityEngine;

public class BookCount : MonoBehaviour
{
    private TextMeshPro count;
    private bool first = false;
    [SerializeField] private GameObject trigger;

    private void Start()
    {
        count = GetComponent<TextMeshPro>();
    }

    private void Update()
    {
        int cnt = 9 - MainManager.instance.ItemCount("Book");
        if (cnt > 0) count.text = cnt.ToString();
        else
        {
            count.fontSize = 2;
            if (PlayerPrefs.GetString("Language", "English") == "English") count.text = "Thanks.";
            else count.text = "Ð»Ð»¡£";
            if (!first)
            {
                first = true;
                Destroy(trigger);
            }
        }
    }
}
