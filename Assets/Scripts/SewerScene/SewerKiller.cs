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
    private float reach= 1.3f;
    private NavMeshAgent agent;
    private Animator animator;
    private Vector3 camPos;
    private Vector3 ropePos = new Vector3(-1.99596214f, -6.16445589f, -0.37992692f);
    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        camPos = jumpScareCam.transform.localPosition;
    }

    private void Update()
    {
        if (MainManager.instance.gameState == 1 && state > 0) agent.isStopped = false;
        else agent.isStopped = true;

        bool inReach = (Vector3.Distance(transform.position, player.position) < reach && state == 1) || (Vector3.Distance(transform.position, player2.position) < reach * 3.0f && state == 2);

        if (inReach && MainManager.instance.gameState == 1)
        {
            state = 0;
            animator.SetBool("Killed", true);
            StartCoroutine(KillIt());
        }

        if (state == 1)
        {
            agent.SetDestination(player.position);
            dgc.SetIntensity(Mathf.Max((10.0f - Vector3.Distance(transform.position, player.position)) / 20.0f, 0));
        }
        else
        {
            agent.SetDestination(ropePos);
            dgc2.SetIntensity(Mathf.Max((10.0f - Vector3.Distance(transform.position, player2.position)) / 20.0f, 0));
        }

        if (state == 2 && Vector3.Distance(transform.position, ropePos) < reach * 2.5f)
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