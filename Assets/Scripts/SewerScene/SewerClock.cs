using UnityEngine;
using UnityEngine.UI;

public class SewerClock : MonoBehaviour
{
    [SerializeField] private AudioClip tick;
    [SerializeField] private AudioClip tock;
    [SerializeField] private AudioClip die;
    [SerializeField] private Image screen;
    [SerializeField] private AudioSource whispersAd;
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject playerHead;

    private int beats = 0;
    private float t = 0;
    private float clockV = 0;
    private bool muted = false;
    private int tockRemain = 0;
    private AudioSource ad;

    private void Start()
    {
        ad = GetComponent<AudioSource>();
        clockV = ad.volume;
        MainManager.instance.AddTrigger("canrun;1");
        MainManager.instance.AddTrigger("flashprompt;Press [Shift] to run");
    }

    private void Update()
    {
        if (MainManager.instance.AtPausedScreen() || muted) return;

        t += Time.deltaTime;
        if (t >= 0.5f)
        {
            t -= 0.5f;
            if (tockRemain == 0)
            {
                ad.clip = tick;
                tockRemain = 3;
            }
            else
            {
                ad.clip = tock;
                tockRemain--;
            }
            ad.Play();
            beats++;
            if(beats % 20 == 1) SewerSubText.instance.DisplayText((181 - beats).ToString());
            if(beats >= 181 && player.activeSelf && MainManager.instance.gameState == 1)
            {
                playerHead.SetActive(true);
                playerHead.transform.parent = null;
                playerHead.GetComponent<Rigidbody>().AddForce(0, 0.5f, 0, ForceMode.Impulse);
                playerHead.GetComponent<Rigidbody>().AddTorque(Vector3.up / 3.0f, ForceMode.Impulse);
                player.SetActive(false);
                MainManager.instance.PlayEffect(die);
                screen.color = Color.red / 2.0f;
                MainManager.instance.AddTrigger("wait;3");
                MainManager.instance.AddTrigger("loadscene;SewerScene;3");
            }
        }

        whispersAd.volume = beats / 180.0f * PlayerPrefs.GetFloat("Music", 30.0f) / 100.0f;
    }

    public void Mute(bool b)
    {
        if (b) ad.volume = 0;
        else ad.volume = clockV;
        muted = b;
    }
}