using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class SewerKiller : MonoBehaviour
{
    [SerializeField] private Transform player;

    private NavMeshAgent agent;
    private Animator animator;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        animator.SetInteger("State", 2);
    }

    private void Update()
    {
        if (MainManager.instance.gameState == 1) agent.isStopped = false;
        else agent.isStopped = true;

        if(Vector3.Distance(transform.position, player.position) < 0.2f)
        {
            animator.SetInteger("State", 3);
            StartCoroutine(KillIt());
        }

        agent.SetDestination(player.position);
    }

    private IEnumerator KillIt()
    {
        yield return null;
    }
}