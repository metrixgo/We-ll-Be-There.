using KinoGlitch;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CorpseHeadChase : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private Transform playerCam;
    [SerializeField] private Image screen;
    [SerializeField] private GameObject jumpscareCam;
    [SerializeField] private AudioClip jumpscare;

    private bool catched = false;
    private float origV;
    private AudioSource ad;
    private DigitalGlitchController dgc;
    private DigitalGlitchController jumpscareDgc;

    private void Start()
    {
        MainManager.instance.SetPrompt("Press [Shift] to run", true);
        MainManager.instance.AddTrigger("canrun;1");
        ad = GetComponent<AudioSource>();
        origV = ad.volume;
        dgc = playerCam.GetComponent<DigitalGlitchController>();
        jumpscareDgc = jumpscareCam.GetComponent<DigitalGlitchController>();
    }

    private void Update()
    {
        if (MainManager.instance.gameState != 1 || catched) return;

        transform.LookAt(playerCam);
        transform.Translate(transform.forward * 4.9f * Time.deltaTime, Space.World);
        float dist = Vector3.Distance(transform.position, playerCam.position);
        dgc.SetIntensity(Mathf.Max((15.0f - dist) / 60.0f, 0));
        screen.color = Color.red * Mathf.Max((20.0f - dist) / 60.0f, 0);

        if (dist < 0.4f)
        {
            catched = true;
            StartCoroutine(KillIt());
        }
    }

    private IEnumerator KillIt()
    {
        player.SetActive(false);
        jumpscareCam.SetActive(true);
        jumpscareDgc.SetIntensity(0.2f);
        ad.volume = origV * 2.0f;
        MainManager.instance.StopMusic();
        MainManager.instance.PlayEffect(jumpscare);
        MainManager.instance.SetPrompt("");
        MainManager.instance.AddTrigger("wait;6");
        MainManager.instance.AddTrigger("loadscene;SewerEscapedScene");
        float t = 0, l = 0, w = 0;
        while (t < 6.0f)
        {
            if (l > 0)
            {
                screen.color = Color.black;
                l -= Time.deltaTime;
            }
            else if (w > 0)
            {
                screen.color = Color.red * 0.35f;
                w -= Time.deltaTime;
            }
            else
            {
                w = Random.Range(0.1f, 1.5f);
                l = Random.Range(0.1f, 0.3f);
            }
            t += Time.deltaTime;
            yield return null;
        }
    }

}
