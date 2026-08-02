using TMPro;
using UnityEngine;

public class SewerSubText : MonoBehaviour
{
    public static SewerSubText instance;

    private float opacity = 0;
    private AudioSource ad;
    private TextMeshProUGUI txt;

    private void Awake()
    {
        instance = this;
        ad = GetComponent<AudioSource>();
        txt = GetComponent<TextMeshProUGUI>();
    }

    private void Update()
    {
        if(opacity > 0)
        {
            txt.color = Color.red * opacity;
            opacity -= Time.deltaTime * 0.3f;
            if (opacity <= 0)
            {
                txt.color = Color.clear;
                opacity = 0;
            }
        }
    }

    public void DisplayText(string s)
    {
        txt.text = s;
        opacity = 0.3f;
        ad.Play();
    }
}
