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
    [SerializeField] private AudioClip glitch;

    private bool catched = false;
    private DigitalGlitchController dgc;
    private DigitalGlitchController jumpscareDgc;

    private void Start()
    {
        MainManager.instance.SetPrompt("Press [Shift] to run");
        MainManager.instance.AddTrigger("canrun;1");
        dgc = playerCam.GetComponent<DigitalGlitchController>();
        jumpscareDgc = jumpscareCam.GetComponent<DigitalGlitchController>();
    }

    private void Update()
    {
        if (MainManager.instance.gameState != 1 || catched) return;

        Vector3 dir = (playerCam.position - transform.position).normalized;
        float y = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg;
        float x = -Mathf.Asin(dir.y) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(x, y, 0);
        transform.Translate(transform.forward * -5.0f * Time.deltaTime);
        float dist = Vector3.Distance(transform.position, playerCam.position);
        dgc.SetIntensity(Mathf.Max((8.0f - dist) / 20.0f, 0));
        screen.color = Color.red * Mathf.Max((15.0f - dist) / 45.0f, 0);

        if (dist < 0.2f)
        {
            catched = true;
            StartCoroutine(KillIt());
        }
    }

    private IEnumerator KillIt()
    {
        player.SetActive(false);
        jumpscareCam.SetActive(true);
        screen.color = Color.clear;
        MainManager.instance.StopMusic();
        MainManager.instance.PlayEffect(glitch);
        MainManager.instance.AddTrigger("wait;6");
        MainManager.instance.AddTrigger("loadscene;SewerEscapedScene");
        float t = 0, l = 0, w = 0;
        while (t < 6.0f)
        {
            if (l > 0)
            {
                screen.color = Color.red * 0.4f;
                jumpscareDgc.SetIntensity(0.5f);
                l -= Time.deltaTime;
            }
            else if (w > 0)
            {
                screen.color = Color.clear;
                jumpscareDgc.SetIntensity(0);
                w -= Time.deltaTime;
            }
            else
            {
                w = Random.Range(0.1f, 1.0f);
                l = Random.Range(0.1f, 0.3f);
            }
            t += Time.deltaTime;
            yield return null;
        }
    }

}
