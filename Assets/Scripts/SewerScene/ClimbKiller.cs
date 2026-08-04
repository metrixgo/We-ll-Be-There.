using KinoGlitch;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ClimbKiller : MonoBehaviour
{
    [SerializeField] private AudioClip jumpScare;
    [SerializeField] private AudioClip fall;
    [SerializeField] private Transform player;
    [SerializeField] private DigitalGlitchController dgc;
    [SerializeField] private GameObject jumpScareCam;
    [SerializeField] private RawImage ri;
    [SerializeField] private Material mat;
    [SerializeField] private Image screen;

    private int state = 1;
    private float reach = 3.0f;
    private Animator animator;
    private Vector3 camPos;

    private void Start()
    {
        animator = GetComponent<Animator>();
        camPos = jumpScareCam.transform.localPosition;
    }

    private void Update()
    {
        if (MainManager.instance.gameState != 1) return;

        if (Mathf.Abs(player.position.y - transform.position.y) < reach && state == 1 && MainManager.instance.gameState == 1)
        {
            state = 0;
            animator.SetBool("Killed", true);
            StartCoroutine(KillIt());
        }

        transform.Translate(Vector3.up * 0.55f * Time.deltaTime);
        dgc.SetIntensity(Mathf.Max((5.0f - Mathf.Abs(player.position.y - transform.position.y)) / 10.0f, 0));
        screen.color = Color.red * Mathf.Max((7.0f - Mathf.Abs(player.position.y - transform.position.y)) / 5.0f, 0);
    }

    private IEnumerator KillIt()
    {
        player.gameObject.SetActive(false);
        jumpScareCam.SetActive(true);
        ri.material = mat;
        SewerMusicManager.instance.StopMusic();
        MainManager.instance.StopMusic();
        MainManager.instance.PlayEffect(jumpScare);
        MainManager.instance.AddTrigger("wait;0.8");
        MainManager.instance.AddTrigger("changescreen;#FF0000FF;#FF0000FF;1");
        MainManager.instance.AddTrigger("changescreen;#FF0000FF;#000000FF;4");
        MainManager.instance.AddTrigger("loadscene;SewerKilledScene");

        float t = 0;
        while (t < 0.8f)
        {
            jumpScareCam.transform.localPosition = camPos + Vector3.up * Mathf.Sin(t * 50) * 0.1f;
            t += Time.deltaTime;
            yield return null;
        }
        MainManager.instance.PlayEffect(fall);
    }
}
