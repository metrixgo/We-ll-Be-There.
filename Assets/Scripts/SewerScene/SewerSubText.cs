using TMPro;
using UnityEngine;

public class SewerSubText : MonoBehaviour
{
    public static SewerSubText instance;

    private float opacity = 0;
    private AudioSource ad;
    private TextMeshProUGUI txt;
    private float txtOpacity = 0.5f;
    private Color txtColor = Color.red;

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
            txt.color = txtColor * opacity;
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
        txt.text = MainManager.instance.Translate(s);
        opacity = txtOpacity;
        ad.Play();
    }

    public void Climbed()
    {
        txtColor = Color.white;
        txtOpacity = 0.8f;
    }
}
