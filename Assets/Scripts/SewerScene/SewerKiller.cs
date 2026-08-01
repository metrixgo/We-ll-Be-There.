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
    [SerializeField] private Transform player2;
    [SerializeField] private DigitalGlitchController dgc2;
    [SerializeField] private GameObject jumpScareCam;
    [SerializeField] private GameObject climbKiller;
    [SerializeField] private RawImage ri;
    [SerializeField] private Material mat;

    private int state = 1;
    private float reach= 1.2f;
    private NavMeshAgent agent;
    private Animator animator;
    private Vector3 camPos;
    private Vector3 ropePos = new Vector3(-1.74596214f, -1147.48999977f, -0.37992692f);

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        animator.SetInteger("State", 1);
        camPos = jumpScareCam.transform.localPosition;
    }

    private void Update()
    {
        if (MainManager.instance.gameState == 1 && state > 0) agent.isStopped = false;
        else agent.isStopped = true;

        bool inReach = (Vector3.Distance(transform.position, player.position) < reach && state == 1) || (Vector3.Distance(transform.position, player2.position) < reach && state == 2);

        if (inReach && MainManager.instance.gameState == 1)
        {
            state = 0;
            animator.SetInteger("State", 0);
            StartCoroutine(KillIt());
        }

        if (state == 1)
        {
            agent.SetDestination(player.position);
            dgc.SetIntensity((10.0f - Vector3.Distance(transform.position, player.position)) / 20.0f);
        }
        else
        {
            agent.SetDestination(ropePos);
            dgc2.SetIntensity((10.0f - Vector3.Distance(transform.position, player2.position)) / 20.0f);
        }

        if (state == 2 && Vector3.Distance(transform.position, ropePos) < reach)
        {
            climbKiller.SetActive(true);
            Destroy(gameObject);
        }
    }

    public void SecondStage()
    {
        state = 2;
    }

    private IEnumerator KillIt()
    {
        player.gameObject.SetActive(false);
        player2.gameObject.SetActive(false);
        jumpScareCam.SetActive(true);
        ri.material = mat;
        SewerMusicManager.instance.StopMusic();
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