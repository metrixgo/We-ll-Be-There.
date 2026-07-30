using KinoGlitch;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class SewerKiller : MonoBehaviour
{
    [SerializeField] private AudioClip jumpScare;
    [SerializeField] private AudioClip fall;
    [SerializeField] private Transform player;
    [SerializeField] private DigitalGlitchController dgc;
    [SerializeField] private GameObject jumpScareCam;
    [SerializeField] private RawImage ri;
    [SerializeField] private Material mat;

    private bool killed = false;
    private NavMeshAgent agent;
    private Animator animator;
    private Vector3 camPos;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        animator.SetInteger("State", 1);
        camPos = jumpScareCam.transform.localPosition;
    }

    private void Update()
    {
        if (MainManager.instance.gameState == 1 && !killed) agent.isStopped = false;
        else agent.isStopped = true;

        if (Vector3.Distance(transform.position, player.position) < 1.0f && MainManager.instance.gameState == 1)
        {
            killed = true;
            animator.SetInteger("State", 0);
            StartCoroutine(KillIt());
        }

        agent.SetDestination(player.position);
        dgc.SetIntensity((10.0f - Vector3.Distance(transform.position, player.position)) / 20.0f);
    }

    private IEnumerator KillIt()
    {
        player.gameObject.SetActive(false);
        jumpScareCam.SetActive(true);
        ri.material = mat;
        SewerMusicManager.instance.StopMusic();
        MainManager.instance.PlayEffect(jumpScare);
        MainManager.instance.AddTrigger("wait;0.8");
        MainManager.instance.AddTrigger("changescreen;#FF0000FF;#FF0000FF;1");
        MainManager.instance.AddTrigger("changescreen;#FF0000FF;#000000FF;4");
        MainManager.instance.AddTrigger("loadscene;SewerScene");

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